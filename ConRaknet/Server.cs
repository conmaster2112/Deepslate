using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using ConMaster.Raknet.Packets;
using System.Buffers;

namespace ConMaster.Raknet
{
    public class Server: IDisposable, IServerProvider
    {
        /////////// CONSTANTS
        public const ushort MAX_MTU = 1_500;
        public const byte VALID_PACKET = 0x80;
        public const int CANDIDATE_TIMEOUT = 10_000_0000; //(3s * 1000ms * 10000 ticks) => 3s
        public const byte GAME_PACKET = 0xfe;
        public const byte RAKNET_VERSION = 11;



        ////////// EVENTS
        public event Action<object, Connection>? OnClientConnected;
        public event Action<object, Connection>? OnClientDisconnected;
        public event ErrorEventHandler? OnError;

        // Is not Concurent as it access only main server thread
        private readonly Dictionary<int, Connection> _connections = [];
        private readonly Dictionary<int, Connection> _candidates = [];
        private readonly Socket _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private readonly IPEndPoint _endpointFactory;
        private readonly long _initializationTime = Stopwatch.GetTimestamp();
        private CancellationTokenSource? _tokenSrc;
        private Task? _task;
        private string _motd = "MCPE;Dedicated Server;390;1.14.60;3;10;13253860892328930865;Bedrock level;Survival;1;19132;19133;";
        private readonly byte[] _buffer;


        public ulong GuildId { get; private init; } = (ulong)(DateTime.UtcNow.Ticks ^ 5468465 ^ 546445169 + 654651);
        public long UpTime => Stopwatch.GetTimestamp() - _initializationTime;
        public IPAddress ListenAddress { get; private init; }
        public ushort ListenPort { get; private init; }
        public bool IsDisposed { get; private set; } = false;
        public bool IsRunning => _tokenSrc != null;
        public int ConnectionCount => _connections.Count;

        public Server(int port = 19132, IPAddress? listenFor = default)
        {
            ListenPort = (ushort)port;
            ListenAddress = listenFor ?? IPAddress.Any;

            _endpointFactory = new(ListenAddress, ListenPort);
            _socket.Bind(_endpointFactory);
            _buffer = ArrayPool<byte>.Shared.Rent(MAX_MTU + 64); //Add some padding just in case
        }

        public void Start()
        {
            lock (this)
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                if (IsRunning) throw new RaknetException("Server is already running.");
                _tokenSrc = new();
                _task = RunReceiveCylce(_tokenSrc.Token);
            }
        }
        public async Task Stop()
        {
            lock (this)
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                if (!IsRunning) throw new RaknetException("Can't stop, server is no longer running.");
                if (_tokenSrc?.IsCancellationRequested??false) throw new RaknetException("Server is already about to stop.");
                _tokenSrc?.Cancel();
            }
            if(_task != null) await _task;
            lock (this) _tokenSrc = null;
        }
        public void SetMotd(ServerMotdInfo info)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            _motd = $"{(info.IsEducationEdition ? "MCEE" : "MCPE")};{info.Name};{info.ProtocolVersion};{info.GameVersion};{info.CurrentPlayerCount};{info.MaxPlayerCount};{GuildId};{info.LevelName};Survival;1;{ListenPort};19133;";
        }
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            if (IsRunning) Stop().Wait();
            _socket.Dispose();
            _tokenSrc?.Dispose();
            ArrayPool<byte>.Shared.Return(_buffer);
            GC.SuppressFinalize(this);
        }

        //////// PRIVATE
        private void RunTick()
        {

            long TICK_TIME = UpTime;
            foreach (Connection connection in _connections.Values) Connection.ExecuteTick(connection);
            foreach (Connection connection in _candidates.Values)
            {
                if (TICK_TIME - connection.ConnectionTime > CANDIDATE_TIMEOUT)
                {
                    _candidates.Remove(connection.GetHashCode(), out Connection? _);
                    connection.Dispose();
                }
                else Connection.ExecuteTick(connection);
            }
        }
        private async Task RunTickCycle(CancellationToken token)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        RunTick();
                    }
                    catch (Exception ex)
                    {
                        ThrowError(this, ex);
                    }
                    if (stopwatch.ElapsedTicks < 8_0000) await Task.Delay(10 - (int)stopwatch.ElapsedMilliseconds, token);
                    stopwatch.Restart();
                }
            }
            catch (Exception)
            {
                //Canceled Task its OK
            }
        }
        private async Task RunReceiveCylce(CancellationToken token)
        {
            SocketAddress address = new(_socket.AddressFamily);
            Task task = RunTickCycle(token);
            while (!token.IsCancellationRequested)
            {
                try
                {
                    int bytesReceived = await _socket.ReceiveFromAsync(_buffer, SocketFlags.None, address, token);
                    if (bytesReceived <= 0) continue;
                    ProccessIncoming(bytesReceived, address);
                }
                catch (Exception er)
                {
                    if (token.IsCancellationRequested) break;
                    ThrowError(this, er);
                }
            }
            await task;
        }
        private void ProccessIncoming(int bytesReceived, SocketAddress address)
        {
            int HASH = address.GetHashCode();
            byte THE_HEAD = _buffer[0];
            bool IS_VALID = (THE_HEAD & VALID_PACKET) == VALID_PACKET;
            ReadOnlySpan<byte> data = _buffer.AsSpan(0, bytesReceived);
            if (IS_VALID)
            {
                if (_connections.TryGetValue(HASH, out Connection? connection) || _candidates.TryGetValue(HASH, out connection))
                {
                    Connection.ReceivedPayload(connection, data);
                }
            }
            else switch (data[0])
                {
                    case UnconnectedPing.PacketId: HandlePing(data, address); break;
                    case OpenConnectionRequest1.PacketId: HandleOpenConnection1(data, address); break;
                    case OpenConnectionRequest2.PacketId: HandleOpenConnection2(data, address); break;
                    default:
                        ThrowError(this, new RaknetException("Unknow offline packet: " + data[0]));
                        break;
                }
        }
        void IServerProvider.Start() => Start();
        void IServerProvider.Stop() => Stop().Wait(); 
        event Action<object, IConnectionProvider> IServerProvider.OnClientConnected
        {
            add => OnClientConnected += value;
            remove => OnClientConnected -= value;
        }
        event Action<object, IConnectionProvider> IServerProvider.OnClientDisconnected
        {
            add => OnClientDisconnected += value;
            remove => OnClientDisconnected -= value;
        }

        event ErrorEventHandler IServerProvider.OnError
        {
            add => OnError += value;
            remove => OnError -= value;
        }

        //////// OFFLINE
        private void HandlePing(ReadOnlySpan<byte> data, SocketAddress address)
        {
            UnconnectedPing ping = new UnconnectedPing().Deserialize(data);
            UnconnectedPong pong = new()
            {
                Time = ping.Time,
                Guid = GuildId,
                MOTD = _motd
            };
            _socket.SendTo(pong.Serialize(stackalloc byte[pong.PACKET_SIZE]), SocketFlags.None, address);
        }
        private void HandleOpenConnection1(ReadOnlySpan<byte> data, SocketAddress address)
        {
            OpenConnectionRequest1 request = new OpenConnectionRequest1().Deserialize(data);
            if (RAKNET_VERSION != request.ProtocolVersion)
            {
                IncompatibleProtocol incompatibleProtocol = new()
                {
                    ProtocolVersion = request.ProtocolVersion,
                    ServerGuid = GuildId
                };
                _socket.SendTo(incompatibleProtocol.Serialize(stackalloc byte[incompatibleProtocol.PACKET_SIZE]), SocketFlags.None, address);
                return;
            }
            int HASH = address.GetHashCode();
            Connection connectionCandidate = Connection.Create(this, (IPEndPoint)_endpointFactory.Create(address), request.MTU, HASH);
            Connection.UpdateConnectionTime(connectionCandidate);
            _candidates.TryAdd(HASH, connectionCandidate);
            OpenConnectionReply1 pong = new()
            {
                Cookie = 0,
                MTU = request.MTU,
                ServerGuid = GuildId,
                UseSecurity = false,
            };
            _socket.SendTo(pong.Serialize(stackalloc byte[pong.PACKET_SIZE]), SocketFlags.None, address);
        }
        private void HandleOpenConnection2(ReadOnlySpan<byte> data, SocketAddress address)
        {
            //Deserialize Packet
            OpenConnectionRequest2 request = new OpenConnectionRequest2().Deserialize(data);

            //Return Unknow candidate, we should ignore this case, bc we don't have enought information to build new connection instance
            if (!_candidates.TryGetValue(address.GetHashCode(), out Connection? candidate)) return;
            Connection.SetGuildId(candidate, request.ClientGuid);
            Connection.UpdateConnectionTime(candidate);
            // Send Response
            OpenConnectionReply2 reply = new()
            {
                ClientAdress = candidate.Address,
                Encrypted = false,
                MTU = request.MTU,
                ServerGuid = GuildId
            };
            _socket.SendTo(reply.Serialize(stackalloc byte[reply.PACKET_SIZE]), SocketFlags.None, address);
        }


        //////// Internal Calls for client
        internal static void SendData(Server server, ReadOnlySpan<byte> data, EndPoint address) => server._socket.SendTo(data, address);
        internal static void ThrowError(Server server, Exception exception, object? sender = default) => server.OnError?.Invoke(sender??server,new(exception));
        // Call Only from thread related to server tick
        internal static void NewConnection(Server server, Connection connection)
        {
            server._candidates.Remove(connection.Hash);
            server._connections[connection.Hash] = connection;
            server.OnClientConnected?.Invoke(server, connection);
        }
        // Call Only from thread related to server tick
        internal static void ConnectionDisconnected(Server server, Connection connection)
        {
            server._candidates.Remove(connection.Hash);
            if (server._connections.Remove(connection.Hash))
                server.OnClientDisconnected?.Invoke(server, connection);
        }
    }
}