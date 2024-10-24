using ConMaster.Buffers;
using ConMaster.Raknet.Packets;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;

namespace ConMaster.Raknet
{
    public class Connection: IDisposable, IConnectionProvider
    {
        //Constants
        private const int GAME_PACKET = 0xfe;
        private const long IDLE_TIMEOUT = 5_000_0000; //5s


        // Public APIs
        public int MAX_PACKET_SIZE => _MTU - FrameSetHeader.HEADER_SIZE - FrameInfo.MAX_SIZE;
        public ConnectionState State { get; internal set; } = ConnectionState.Unconnected;
        public bool IsDisposed { get; private set; } = false;
        public ulong GuildId { get; private set; }
        public int Hash { get; private set; }
        public Server Server { get; private set; }
        public IPEndPoint Address { get; private set; }
        public long UpTime { get => Server.UpTime - _upTime; }
        public long ConnectionTime { get => _upTime; }
        public event DataReceivedEventHandler? DataReceived;

        ////// Privates
        private Connection(Server server, IPEndPoint endpoint, short mtu, int addressHash)
        {
            Hash = addressHash;
            Server = server;
            Address = endpoint;
            _MTU = mtu;
            _intrenalBuffer = ArrayPool<byte>.Shared.Rent(_MTU);
        }
        private readonly short _MTU;
        private long _upTime;
        private readonly byte[] _intrenalBuffer;
        private readonly AckQueueBuffer _incomingRecievedIndexes = new(GC.AllocateUninitializedArray<AckRecord>(60)); 
        private readonly AckQueueBuffer _incomingLostedIndexes = new(GC.AllocateUninitializedArray<AckRecord>(60));
        // Not concurent as other threads doesn't have access to this dictionary
        private readonly Dictionary<uint, FragmentInfo> _incomingOutOfOrderBuffers = [];
        private readonly Dictionary<int, FragmentCompoud> _fragments = [];
        private volatile ushort _outgoingCurrentIndexId = 0;
        private volatile ushort _outgoingFragmentId = 0;
        private volatile ushort _outgoingReliableIndex = 0;
        private volatile ushort _incomingCurrentIndexId = 0;
        private readonly int[] _outgoingOrderIndex = new int[32];
        private readonly int[] _outgoingSequenceIndex = new int[32];
        private readonly int[] _incomingHighestSequenceIndex = new int[32];
        private readonly int[] _incomingOrderIndex = new int[32];
        private readonly ConcurrentQueue<RentedBuffer> _sendQueue = new();
        private readonly ConcurrentDictionary<int, RentedBuffer> _outgoingBackupBuffers = new();
        private long _lastInteraction = Stopwatch.GetTimestamp();


        ////// Public Methods
        public void SendPacket(ReadOnlySpan<byte> data)
        {
            //Maybe could be done without buffer renting?
            //but this micro optimalization could not be woth of it
            //but still, this method is heavely used
            // Stack alloc is not the way, it's usage size is here unknown as this method is public and it could be executed anywhere
            using RentedBuffer buff = RentedBuffer.Alloc(data.Length + 1);
            buff.Span[0] = GAME_PACKET;
            data.CopyTo(buff.Span.Slice(1));
            SendFrame(FrameInfo.ReliableOrdered, buff.Span);
        }
        public void Disconnect()
        {
            SendFrame(FrameInfo.None, DisconnectPacket.PACKET_DATA);
            State = ConnectionState.Disconnected;
        }

        /// Internal Calls for Server
        internal static void UpdateConnectionTime(Connection connection) => connection._upTime = connection.Server.UpTime;
        internal static Connection Create(Server server, IPEndPoint address, short mtu, int addressHash)
        {
            return new Connection(server, address, mtu, addressHash);
        }
        internal static void ExecuteTick(Connection connection) => connection.RunTick();
        internal static void ReceivedPayload(Connection connection, ReadOnlySpan<byte> bytes) => 
            connection.HandleIncomming(bytes);
        internal static void SetGuildId(Connection connection, ulong guildId) => connection.GuildId = guildId;

        ///////// Private And Interface
        
        //Proccessing Level 0 - Raw Data
        private void HandleIncomming(ReadOnlySpan<byte> data)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            _lastInteraction = Stopwatch.GetTimestamp();
            if ((data[0] & Server.VALID_PACKET) != Server.VALID_PACKET) return;
            if (data[0] > 0x8d) switch (data[0])
                {
                    case AckPacket.AckPackedId:
                        HandleAck(data);
                        return;
                    case AckPacket.NackPackedId:
                        HandleNack(data);
                        return;
                    default:
                        Console.WriteLine("RAKNET: Unknown valid packet: " + data[0] + " from " + Address);
                        return;
                }
            int index = FrameSetHeader.ReadSequenceIndex(data);
            if (index < _incomingCurrentIndexId) return;
            else if (index > _incomingCurrentIndexId)
            {
                for (int i = _incomingCurrentIndexId; i < index; i++) _incomingLostedIndexes.Add(i);
                _incomingCurrentIndexId = (ushort)(index + 1);
                return;
            }
            else _incomingCurrentIndexId++;
            _incomingRecievedIndexes.Add(index);
            ReadOnlySpan<byte> buf = data.Slice(FrameSetHeader.HEADER_SIZE);
            while (true)
            {
                FrameInfo info = FrameSetHeader.ReadFrameInfo(buf, out int bytesReaded);
                ReadOnlySpan<byte> current = buf.Slice(bytesReaded, info.BodyLength);
                if (info.IsFragmented) HandleFragment(info, current);
                else HandleFrame(info, current);
                if (bytesReaded + info.BodyLength >= buf.Length) break;
                buf = buf.Slice(bytesReaded + info.BodyLength);
            }
        }
        //Proccessing Level 1 - Data
        private void HandleFragment(FrameInfo frame, ReadOnlySpan<byte> data)
        {
            int id = frame.FragmentCompoudId;
            int index = frame.FragmentIndex;
            int size = frame.FragmentSize;
            if (!_fragments.TryGetValue(id, out FragmentCompoud fragment))
            {
                fragment = new()
                {
                    Buffers = GC.AllocateUninitializedArray<RentedBuffer>(size),
                    Size = size,
                    Id = id,
                    CurrentLength = 0
                };
            }
            fragment.Buffers[index] = RentedBuffer.From(data);
            fragment.CurrentLength++;

            if (fragment.CurrentLength != size)
            {
                // Update
                _fragments[id] = fragment;
                return;
            }
            _fragments.Remove(id, out _);

            RentedBuffer[] buffers = fragment.Buffers;

            //Count Final Size
            int fragmentSizeInBytes = 0;
            for (int i = 0; i < buffers.Length; i++)
            {
                int s = buffers[i].Length;
                fragmentSizeInBytes += s;
            }

            //Initialize Final RentBuffer
            using RentedBuffer payload = RentedBuffer.Alloc(fragmentSizeInBytes);
            Span<byte> current = payload;

            //Write fragments to single payload
            for (int i = 0; i < buffers.Length; i++)
            {
                using RentedBuffer currentBuffer = buffers[i];
                currentBuffer.Span.CopyTo(current);
                current = current.Slice(currentBuffer.Length);
            }
            HandleFrame(frame, payload);
        }
        //Proccessing Level 2 - Full Frame Data
        private void HandleFrame(FrameInfo frame, ReadOnlySpan<byte> data)
        {
            if (FrameInfo.GetIsSequenced(frame.Reliability))
            {
                Console.WriteLine("DEBUG RAKNET: was Sequenced this sequenced frames are not well tested");
                if (
                    frame.SequencedFrameIndex < _incomingHighestSequenceIndex[frame.OrderChannel] ||
                    frame.OrderFrameIndex < _incomingOrderIndex[frame.OrderChannel]
                   )
                {
                    Console.WriteLine("RAKNET: Received out of order frame");
                    return;
                }
                _incomingHighestSequenceIndex[frame.OrderChannel] = frame.SequencedFrameIndex + 1;
                HandlePayload(data);
            }
            else if (FrameInfo.GetIsOrdered(frame.Reliability))
            {
                if (frame.OrderFrameIndex == _incomingOrderIndex[frame.OrderChannel])
                {
                    _incomingHighestSequenceIndex[frame.OrderChannel] = 0;
                    //_IncomingOrderIndex[frame.OrderChannel]++;


                    // Handle the packet
                    HandlePayload(data);
                    uint index = (uint)_incomingOrderIndex[frame.OrderChannel] | ((uint)frame.OrderChannel << 27);
                    while (_incomingOutOfOrderBuffers.Remove(++index, out FragmentInfo info))
                    {
                        HandlePayload(info.RentedBuffer.Span);
                        info.RentedBuffer.Dispose();
                    }

                    // Update the queue
                    _incomingOrderIndex[frame.OrderChannel] = (int)(index & 0b00000111_11111111_11111111_11111111);
                }
                else if (frame.OrderFrameIndex > _incomingOrderIndex[frame.OrderChannel])
                {
                    if (!_incomingOutOfOrderBuffers.TryAdd((uint)frame.OrderFrameIndex | ((uint)frame.OrderChannel << 27), new(frame, RentedBuffer.From(data))))
                        throw new Exception("Unexpected existance of Unordered memory buffer");
                }
            }
            else HandlePayload(data);
        }
        //Proccessing Level 3 - Final Level data are ready for being consumed
        private void HandlePayload(ReadOnlySpan<byte> data)
        {
            switch (data[0])
            {
                case ConnectionRequest.PackedId:
                    HandleConnectionRequest(new ConnectionRequest().Deserialize(data));
                    break;
                case NewIncomingConnection.PackedId:
                    HandleNewIncommingConnection(/*new NewIncomingConnection().Deserialize(data)*/);
                    break;
                case ConnectedPing.PacketId:
                    HandleConnectedPing(new ConnectedPing().Deserialize(data));
                    break;
                case ConnectedPong.PacketId:
                    //HandleConnectedPong(new ConnectedPong().Deserialize(data));
                    break;
                case DisconnectPacket.PackedId:
                    HandleDisconnect(/*default*/);
                    break;
                case GAME_PACKET: //Game Packet
                    HandleGamePacket(data);
                    break;
                default:
                    Console.WriteLine("RAKNET: Unknown packet: " + data[0]);
                    break; ;
            }
        }
        //Proccessing Packets
        private void HandleAck(ReadOnlySpan<byte> data)
        {
            foreach (AckRecord record in AckPacket.GetAckEnumerator(data)) //On Stack based enumerator, so its fast tho
            {
                if (record.Low != record.High)
                {
                    for (int index = record.Low; index <= record.High; index++)
                    {
                        _outgoingBackupBuffers.TryRemove(index, out RentedBuffer buffer);
                        buffer.Dispose();
                    }
                }
                else
                {
                    _outgoingBackupBuffers.TryRemove(record.Low, out RentedBuffer buffer);
                    buffer.Dispose();
                }
            }
        }
        private void HandleNack(ReadOnlySpan<byte> data)
        {
            foreach (AckRecord record in AckPacket.GetAckEnumerator(data))
            {
                if (record.Low != record.High)
                {
                    for (int index = record.Low; index <= record.High; index++)
                    {
                        _outgoingBackupBuffers.TryRemove(index, out RentedBuffer buffer);
                        buffer.Span.CopyTo(_intrenalBuffer);
                        int length = buffer.Length;
                        buffer.Dispose();//We can release this buffer bc its content was already copied

                        RunSendCurrentData(length);
                    }
                }
                else
                {
                    _outgoingBackupBuffers.TryRemove(record.Low, out RentedBuffer buffer);
                    buffer.Span.CopyTo(_intrenalBuffer);
                    int length = buffer.Length;
                    buffer.Dispose();//We can release this buffer bc its content was already copied

                    RunSendCurrentData(length);
                }
            }
        }
        private void HandleConnectionRequest(ConnectionRequest request)
        {
            if (State == ConnectionState.Connected) return;
            State = ConnectionState.Connecting;
            ConnectionRequestAccepted accept = new()
            {
                RequestTime = request.Time,
                Time = Server.UpTime,
                ClientAddress = Address,
                SystemIndex = 0
            };
            Span<byte> buffer = stackalloc byte[accept.PACKET_SIZE];
            SendFrame(FrameInfo.ReliableOrdered, accept.Serialize(buffer));
            UpdateConnectionTime(this);
        }
        private void HandleNewIncommingConnection(/*NewIncomingConnection connection*/)
        {
            if (State == ConnectionState.Connected) return;
            State = ConnectionState.Connected;
            Server.NewConnection(Server, this);
        }
        private void HandleGamePacket(ReadOnlySpan<byte> data)
        {
            if (State != ConnectionState.Connected) return;
            try
            {
                DataReceived?.Invoke(this, data.Slice(1)); //First byte is GAME_PACKET id
            }
            catch (Exception ex)
            {
                Server.ThrowError(Server, ex);
            }
        }
        private void HandleConnectedPing(ConnectedPing ping)
        {
            ConnectedPong pong = new()
            {
                PingTime = ping.Time,
                PongTime = Server.UpTime,
            };
            Span<byte> buffer = stackalloc byte[pong.PACKET_SIZE];
            SendFrame(new FrameInfo() { Reliability = FrameReliability.Unreliable }, pong.Serialize(buffer));
        }
        /*
        private void HandleConnectedPong(ConnectedPong pong)
        {
            //IDK
        }*/
        private void HandleDisconnect(/*DisconnectPacket packet*/)
        {
            State = ConnectionState.Disconnected;
        }
        //Send frame
        //Consider using Interlocked increment for outgointSequences
        private void SendFrame(FrameInfo frame, ReadOnlySpan<byte> data)
        {
            if (IsDisposed) return;
            // Check if the packet is sequenced or ordered
            if (FrameInfo.GetIsSequenced(frame.Reliability))
            {
                // Set the order index and the sequence index
                frame.OrderFrameIndex = _outgoingOrderIndex[frame.OrderChannel];
                frame.SequencedFrameIndex = _outgoingSequenceIndex[frame.OrderChannel]++;
            }
            else if (FrameInfo.GetIsOrderedExclusive(frame.Reliability))
            {
                // Set the order index and the sequence index
                frame.OrderFrameIndex = _outgoingOrderIndex[frame.OrderChannel]++;
                _outgoingSequenceIndex[frame.OrderChannel] = 0;
            }

            int DATA_LENGTH = data.Length;

            if (DATA_LENGTH > MAX_PACKET_SIZE)
            {
                ReadOnlySpan<byte> current = data;
                frame.IsFragmented = true;
                frame.FragmentCompoudId = (short)_outgoingFragmentId++;
                frame.FragmentSize = (int)Math.Ceiling(DATA_LENGTH / (double)MAX_PACKET_SIZE);
                for (int i = 0; true; i++)
                {
                    ReadOnlySpan<byte> currentData = current.Slice(0, Math.Min(MAX_PACKET_SIZE, current.Length));
                    RentedBuffer buffer = RentedBuffer.Alloc(currentData.Length + FrameInfo.MAX_SIZE);

                    frame.FragmentIndex = i;
                    frame.ReliableFrameIndex = _outgoingReliableIndex++;
                    frame.BodyLength = currentData.Length;
                    int writen = frame.Serialize(buffer.Span);

                    currentData.CopyTo(buffer.Span.Slice(writen));
                    RentedBuffer.SetLength(ref buffer, writen + currentData.Length);
                    _sendQueue.Enqueue(buffer);

                    if (current.Length == currentData.Length) break;
                    current = current.Slice(current.Length);
                };
            }
            else
            {
                frame.BodyLength = DATA_LENGTH;
                frame.ReliableFrameIndex = _outgoingReliableIndex++;
                RentedBuffer buffer = RentedBuffer.Alloc(DATA_LENGTH + FrameInfo.MAX_SIZE);
                int writen = frame.Serialize(buffer.Span);
                data.CopyTo(buffer.Span.Slice(writen));
                RentedBuffer.SetLength(ref buffer, writen + DATA_LENGTH);
                _sendQueue.Enqueue(buffer);
            }
        }
        private void RunFlush()
        {
            if (_sendQueue.IsEmpty) return;
            Span<byte> dataToSend = _intrenalBuffer.AsSpan(FrameSetHeader.HEADER_SIZE);
            int length = FrameSetHeader.HEADER_SIZE;
            while (_sendQueue.TryDequeue(out RentedBuffer memory))
            {
                if (memory.Length > dataToSend.Length)
                {
                    RunSendCurrentData(length);
                    dataToSend = _intrenalBuffer.AsSpan(FrameSetHeader.HEADER_SIZE);
                    length = FrameSetHeader.HEADER_SIZE;
                }
                else
                {
                    memory.Span.CopyTo(dataToSend);
                    dataToSend = dataToSend.Slice(memory.Length);
                    length += memory.Length;
                }
                memory.Dispose();
            }
            if (length > FrameSetHeader.HEADER_SIZE) RunSendCurrentData(length);
        }
        private void RunSendCurrentData(int length)
        {
            int indexId = _outgoingCurrentIndexId++;
            FrameSetHeader.SetHeader(_intrenalBuffer, indexId);
            Span<byte> data = _intrenalBuffer.AsSpan(0, length);
            _outgoingBackupBuffers[indexId] = RentedBuffer.From(data);
            Server.SendData(Server, data, Address);
        }
        private void RunTick()
        {
            if (IsDisposed) return;
            ConnectionState was = State;
            if (_incomingRecievedIndexes.Length > 0) SendAckOrNack(_incomingRecievedIndexes, true);
            if (_incomingLostedIndexes.Length > 0) SendAckOrNack(_incomingLostedIndexes, false);
            RunFlush();
            if (was == ConnectionState.Disconnected)
            {
                Server.ConnectionDisconnected(Server, this);
                Dispose();
            }
            //If no packets are send for more than 5s
            else if (Stopwatch.GetTimestamp() > (IDLE_TIMEOUT + _lastInteraction))
                Disconnect();
        }
        private void SendAckOrNack(AckQueueBuffer indexes, bool isAck)
        {
            ReadOnlySpan<AckRecord> ackRecords = indexes.GetRecords();
            Span<byte> bytes = stackalloc byte[AckPacket.GetPacketSize(ackRecords)];
            AckPacket.Serialize(bytes, indexes.GetRecords(), isAck ? AckPacket.AckPackedId : AckPacket.NackPackedId);
            Server.SendData(Server, bytes, Address);
            indexes.Clear();
        }
        IServerProvider IConnectionProvider.Server => Server;
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            foreach (var a in _outgoingBackupBuffers.Values) a.Dispose();
            foreach (var a in _fragments.Values)
            {
                for (int i = 0; i < a.Buffers.Length; i++)
                {
                    a.Buffers[i].Dispose();
                }
            }
            foreach (var a in _incomingOutOfOrderBuffers.Values) a.RentedBuffer.Dispose();
            foreach (var a in _sendQueue) a.Dispose();
            _outgoingBackupBuffers.Clear();
            _fragments.Clear();
            _incomingOutOfOrderBuffers.Clear();
            _sendQueue.Clear();
            ArrayPool<byte>.Shared.Return(_intrenalBuffer);
            GC.SuppressFinalize(this);
        }
        public override int GetHashCode() => Hash.GetHashCode();
    }
    internal struct FragmentInfo(FrameInfo info, RentedBuffer buffer)
    {
        public FrameInfo FrameInfo = info;
        public RentedBuffer RentedBuffer = buffer;
    }
    internal struct FragmentCompoud
    {
        public int Id;
        public int Size;
        public int CurrentLength;
        public RentedBuffer[] Buffers;
    }
}
