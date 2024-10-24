using ConMaster.Buffers;
using ConMaster.Deepslate.Network.InternalPackets;
using System.Net;

namespace ConMaster.Deepslate.Network
{
    public class Client
    {
        //private const int BUFFER_SIZE = short.MaxValue;
        //internal bool HasEncryption = false;

        // Getters
        public IPEndPoint Address => Connection.Address;
        public ulong GuildId => Connection.GuildId;
        public IConnectionProvider Connection { get; private init; }
        public Server Server { get; private init; }
        public bool HasCompression { get; private set; }

        // Public Methods
        public void SendPacket(IEnumerable<IPacket> packets)
        {
            using RentedBuffer buffer = Server.BuildPacketsGamePayload(packets);
            InternalSendRawGamePayload(buffer);
        }
        public void InternalSendRawGamePayload(ReadOnlySpan<byte> data)
        {
            using RentedBuffer buffer = RentedBuffer.Alloc(data.Length + 10);
            int length = data.Length + 1;
            //buffer.Span[0] = Raknet.Server.GAME_PACKET;
            if(HasCompression)
            {
                buffer.Span[0] = (byte)CompressionMethod.ZLib;
                length = Server.Deflater.Compress(data, buffer.Span.Slice(1)) + 1;
            }
            else
            {
                data.CopyTo(buffer.Span);
                length = data.Length;
            }
            SendRawPayload(buffer.Span.Slice(0, length));
        }
        public void SendRawPayload(ReadOnlySpan<byte> data) => Connection.SendPacket(data);
        /*public void Disconnect(string message, DisconnectReason reason = DisconnectReason.NoReason, bool hideMesage = false)
        {
            using DisconnectPacket disconnect = DisconnectPacket.Create();
            disconnect.Message = message;
            disconnect.Reason = reason;
            disconnect.SkipMessage = hideMesage;
            SendPacket([disconnect]);
        }*/


        // Internal
        private Client(Server server, IConnectionProvider connection)
        {
            Connection = connection;
            Server = server;
            connection.DataReceived += (__, data) =>
            {
                ProccessRawPayload(data);
            };
        }
        private void ProccessRawPayload(ReadOnlySpan<byte> data)
        {
            ReadOnlySpan<byte> payload = data;
            //if (HasEncryption) ; Encryption not implemented

            if (HasCompression)
            {
                CompressionMethod method = (CompressionMethod)payload[0];
                if (method == CompressionMethod.None) payload = payload.Slice(1);
                else if (method == CompressionMethod.ZLib)
                {
                    int length = Server.Deflater.Decompress(payload.Slice(1), Server._DecompressBuffer.Span);
                    payload = Server._DecompressBuffer.Span.Slice(0, length);
                }
                else
                {
                    Console.WriteLine("Unsuported Compression type: " + method);
                    return;
                }
            }
            int offset = 0;
            ConstantMemoryBufferReader reader = new(payload, ref offset);
            while (!reader.IsEndOfStream)
            {
                int payloadSize = (int)reader.ReadUVarInt32();
                ProcessPacket(reader.ReadSlice(payloadSize));
            }
        }
        private void ProcessPacket(ReadOnlySpan<byte> data)
        {
            Server.Protocol.HandlePacketPayload(data, this);
        }


        public void Disconnect(int reasonId, string? reason = default)
        {
            var disconect = new DisconnectPacketInternal()
            {
                Reason = reasonId,
                SkipMessage = true,
            };
            if(reason != null)
            {
                disconect.Message = reason;
                disconect.SkipMessage = false;
            }
            SendPacket([disconect]);
            Connection.Disconnect();
        }
        // Public Setters
        //public static void SetConnectionState(Client client, ConnectionState state) => client.State = state;
        //public static void SetHasCompression(Client client, bool hasCompression) => client.HasCompression = hasCompression;
        internal static Client Create(Server server, IConnectionProvider provider)
        {
            return new(server, provider);
        }
        internal static void SetBaseHandlers(Server server, BaseProtocol protocol)
        {
            protocol.TryAddPacketHandler(new RequestNetworkSettingsHandler());
        }
        struct RequestNetworkSettingsHandler : IHandleRawPacketHandler
        {
            public readonly int PacketId => RequestNetworkSettingsPacketInternal.PACKET_ID;
            public bool HandleRawPacket(BaseProtocol proto, Client client, ConstantMemoryBufferReader reader)
            {
                RequestNetworkSettingsPacketInternal packet = default;
                packet.Read(reader);
                if (packet.ProtocolVersion > client.Server.Protocol.ProtocolVersion)
                {
                    client.Disconnect(
                        (int)DisconnectReason.OutdatedServer,
                        $"Protocol Mismatch \n{packet.ProtocolVersion} > {client.Server.Protocol.ProtocolVersion}, Outdated Server, this version of client is not supported yet"
                    );
                }
                else if (packet.ProtocolVersion < client.Server.Protocol.ProtocolVersion)
                {
                    client.Disconnect(
                        (int)DisconnectReason.OutdatedClient,
                        $"Protocol Mismatch \n{packet.ProtocolVersion} < {client.Server.Protocol.ProtocolVersion}, Outdated client, please update your client"
                    );
                }
                else
                {
                    client.SendPacket([new NetworkSettingsPacketInternal()]);
                    client.HasCompression = true;
                    Server.ClientJoin(client.Server, client);
                }
                return true;
            }
        }

        public override int GetHashCode() => GuildId.GetHashCode();
    }
}
/*
public void SendPacket(params IPacket[] packets)
{
    //Helper Buffer for compression
    using RentedBuffer main_buffer = RentedBuffer.Alloc(BUFFER_SIZE << 2);

    //Final Buffer to send
    using RentedBuffer compressed = RentedBuffer.Alloc(BUFFER_SIZE << 2);
    compressed.Span[0] = Raknet.Server.GAME_PACKET;


    // Setup Main Writer depending on compression
    int main_offset = 0;
    ConstantMemoryBufferWriter main_writer = new(
        HasCompression?main_buffer.Span:compressed.Span.Slice(1), 
        ref main_offset
    );

    //Write Packets to Main-Writer
    int current_offset = 0;
    using RentedBuffer current_buffer = RentedBuffer.Alloc(BUFFER_SIZE);
    ConstantMemoryBufferWriter current_writer = new(current_buffer.Span, ref current_offset);
    foreach (IPacket packet in packets)
    {
        current_offset = 0;
        current_writer.WriteUVarInt32((uint)packet.Id);
        packet.Write(current_writer);


        ReadOnlySpan<byte> data = current_writer.GetWritenBytes();
        main_writer.WriteUVarInt32((uint)data.Length);
        main_writer.Write(data);
    }


    //Setup final buffer span
    ReadOnlySpan<byte> finalSpan;
    if (HasCompression)
    {
        compressed.Span[1] = (byte)CompressionMethod.ZLib;
        Server.Deflate.Compress(main_writer.GetWritenBytes(), compressed.Span.Slice(2), out int writen);
        finalSpan = compressed.Span.Slice(0, writen + 2); // GamePacket + CompressionMethod
    }
    else finalSpan = compressed.Span.Slice(0, main_offset + 1); //GamePacketId + Buffer

    Connection.Send(finalSpan, FrameReliability.ReliableOrdered);
}*/