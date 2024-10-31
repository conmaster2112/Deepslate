using ConMaster.Buffers;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Packets;
using ConMaster.Deepslate.Protocol.Proto;

namespace ConMaster.Deepslate.Protocol
{
    public class DeepslateProtocol : BaseProtocol
    {
        protected readonly Dictionary<int, IHandleRawPacketHandler> _packets = [];
        public override void HandlePacketPayload(ReadOnlySpan<byte> packetPayload, Client client)
        {
            int offset = 0;
            ConstantMemoryBufferReader reader = new(packetPayload, ref offset);
            int packetId = (int)reader.ReadUVarInt32();
            if (_packets.TryGetValue(packetId, out IHandleRawPacketHandler? handler))
            {
                try
                {
                    if (!handler.HandleRawPacket(this, client, reader))
                    {
                        InvokeWarn("No Packet Handler for: " + packetId);
                    }
                }
                catch (Exception ex)
                {
                    InvokeError(ex);
                }
            }
            else
            {
                InvokeWarn("No Packet Desrializer for: " + packetId);
            }
        }
        public override T AddPacketHandler<T>(T handler)
        {
            if (_packets.ContainsKey(handler.PacketId)) throw new Exception("Protocol with this id already exists, id: " + handler.PacketId);
            _packets[handler.PacketId] = handler;
            return handler;
        }
        public BasePacketHandler<MovePlayerPacket> PlayerMoveHandler { get; init; }
        public BasePacketHandler<LoginPacket> PlayerLogin { get; init; }
        public BasePacketHandler<SetPlayerGameModePacket> SetPlayerGameMode { get; init; }
        public BasePacketHandler<CommandRequestPacket> CommandRequest { get; init; }
        public BasePacketHandler<SetLocalPlayerAsInitializedPacket> PlayerInitialized { get; init; }
        public BasePacketHandler<LoadingScreenInfoPacket> LoadingScreenInfo { get; init; }
        public BasePacketHandler<ClientCacheStatusPacket> ClientCacheStatus { get; init; }
        public BasePacketHandler<PacketViolationWarningPacket> PacketViolationWarning { get; init; }
        public BasePacketHandler<ResourcePackClientResponsePacket> ResourcePacksResponse { get; init; }
        
        public DeepslateProtocol(int version): base(version)
        {
            AddPacketHandler(PlayerMoveHandler = new());
            AddPacketHandler(PlayerLogin = new());
            AddPacketHandler(SetPlayerGameMode = new());
            AddPacketHandler(CommandRequest = new());
            AddPacketHandler(PlayerInitialized = new());
            AddPacketHandler(LoadingScreenInfo = new());
            AddPacketHandler(ClientCacheStatus = new());
            AddPacketHandler(PacketViolationWarning = new());
            AddPacketHandler(ResourcePacksResponse = new());
        }
    }
}
