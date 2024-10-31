using ConMaster.Buffers;
using ConMaster.Compression;
using ConMaster.Deepslate.Network.Events.Client;
using System.Collections.Concurrent;


namespace ConMaster.Deepslate.Network
{
    public class Server: IDisposable
    {
        public event Action<object, string>? OnWarn;
        public event ErrorEventHandler? OnError;
        public const int BUFFER_SIZE = short.MaxValue;
        public IServerProvider ServerProvider { get; private set; }
        public BaseProtocol Protocol { get; private set; }
        public int OnlineCount => _clients.Count;
        private readonly ConcurrentDictionary<ulong, Client> _clients = new();
        private readonly ConcurrentDictionary<ulong, Client> _candidates = new();
        // Events
        public SequencedEventManager<ClientConnectedEventArgs> ClientConnected { get; protected init; } = new();
        public SequencedEventManager<ClientDisconnectedEventArgs> ClientDisconnected { get; protected init; } = new();



        //Data Recieve is sequenced so we can use single decompression buffer for all clients
        internal readonly RentedBuffer _DecompressBuffer = RentedBuffer.Alloc(1 << 22); //4MB
        public readonly DeflateCompressor Deflater = new() { CompressionLevel = System.IO.Compression.CompressionLevel.Fastest, MemoryLevel = 9};
        public Server(IServerProvider provider, BaseProtocol proto)
        {
            Protocol = proto;
            ServerProvider = provider;
            ServerProvider.OnClientConnected += (__, connection) =>
            {
                Client client = Client.Create(this, connection);
                _candidates[connection.GuildId] = client;
                //Lets keep the handle for initialized client
            };
            ServerProvider.OnClientDisconnected += (__, connection) =>
            {
                _candidates.TryRemove(connection.GuildId, out _);
                if (_clients.TryRemove(connection.GuildId, out Client? client))
                {
                    ClientDisconnected.Invoke(this, new(client));
                }
            };
            Client.SetBaseHandlers(this, proto);
        }

        public void Start() => ServerProvider.Start();
        public void Stop() => ServerProvider.Stop();
        public static RentedBuffer BuildPacketsCompressedPayload(IEnumerable<IPacket> packets, DeflateCompressor deflate)
        {
            using RentedBuffer payload = BuildPacketsGamePayload(packets);
            RentedBuffer final = RentedBuffer.Alloc(payload.Length + 1 + 64); //NoCompressionPossibility?
            //final.Span[0] = Deepslate.Server.GAME_PACKET;
            final.Span[0] = (byte)CompressionMethod.ZLib;
            int writen = deflate.Compress(payload, final.Span.Slice(1));
            RentedBuffer.SetLength(ref final, writen + 1); // GamePacket + CompressionMethod = 2bytes
            return final;
        }
        public static RentedBuffer BuildPacketsGamePayload(IEnumerable<IPacket> packets)
        {
            RentedBuffer base_buffer = RentedBuffer.Alloc(BUFFER_SIZE << 2);
            using RentedBuffer per_pack_buffer = RentedBuffer.Alloc(BUFFER_SIZE);
            //Write Packets to Main-Writer
            int current_offset = 0;
            int main_offset = 0;
            ConstantMemoryBufferWriter main_writer = new(base_buffer, ref main_offset);
            ConstantMemoryBufferWriter current_writer = new(per_pack_buffer.Span, ref current_offset);
            foreach (IPacket packet in packets)
            {
                current_offset = 0;
                current_writer.WriteUVarInt32((uint)packet.Id);
                packet.Write(current_writer);

                ReadOnlySpan<byte> data = current_writer.GetWritenBytes();
                main_writer.WriteUVarInt32((uint)data.Length);
                main_writer.Write(data);
            }
            RentedBuffer.SetLength(ref base_buffer, main_writer.Offset);
            return base_buffer;
        }
        public void Dispose()
        {
            _DecompressBuffer.Dispose();
            if (ServerProvider is IDisposable dis) dis.Dispose();
            GC.SuppressFinalize(this);
        }
        internal static void ClientJoin(Server server, Client client)
        {
            if(server._candidates.TryRemove(client.GuildId, out Client? __))
            {
                try
                {
                    if (server.ClientConnected.Invoke(server, new(__)))
                    {
                        __.Connection.Disconnect();
                    }
                    else
                    {
                        server._clients[client.GuildId] = __;
                    }
                }
                catch (Exception)
                {
                    client.Connection.Disconnect();
                    throw;
                }
            }
        }
    }
}
