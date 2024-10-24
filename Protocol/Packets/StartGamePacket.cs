using ConMaster.Buffers;
using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Protocol.Enums;


using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class StartGamePacket : BasePacket<StartGamePacket>
    {
        public int PACKET_ID = 11;
        public override int Id => PACKET_ID;

        public long UniqueId = default; //VarInt64
        public ulong RuntimeId = default; //UVarInt64
        public GameMode GameMode = default; //VarInt
        public Vec3f SpawnPosition = default;
        public Vec2f Rotation = default;
        public LevelSettings LevelSettings = new();
        public string LevelId = string.Empty;
        public string LevelName = string.Empty;
        public string TemplateContentIdentity = string.Empty;
        public bool IsTrail = default;
        public SyncPlayerMovementSettings PlayerMovementSettings;
        public ulong CurrentTick = default;
        public int EnchantmentsSeed = default;
        public BlockPropertyDefinition[] BlockProperties = [];
        public ItemData[] ItemList = [];
        public string MultiplayerCorrelationId = string.Empty;
        public bool EnableItemStackManagement = default;
        public string ServerVersion = string.Empty;
        public EmptyNBTCompoudRoot PlayerNBTData = default;
        public ulong BlockRegistryCheckSum = default;
        public Guid WorldTemplateId = default;
        public bool EnabledClientSideGen = default;
        public bool BlockNetworkIdsAreHashed = default;
        public NetworkPermission NetworkPermission = default;

        public override void Clean()
        {
            UniqueId = default;
            RuntimeId = default;
            GameMode = default;
            SpawnPosition = default;
            Rotation = default;
            // LevelSettings = null;
            LevelId = string.Empty;
            LevelName = string.Empty;
            TemplateContentIdentity = Guid.Empty.ToString();
            IsTrail = default;
            PlayerMovementSettings = default;
            CurrentTick = default;
            EnchantmentsSeed = default;
            BlockProperties = [];
            ItemList = [];
            MultiplayerCorrelationId = string.Empty;
            EnableItemStackManagement = default;
            ServerVersion = string.Empty;
            PlayerNBTData = default;
            BlockRegistryCheckSum = default;
            WorldTemplateId = Guid.Empty;
            EnabledClientSideGen = default;
            BlockNetworkIdsAreHashed = default;
            NetworkPermission = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            UniqueId = reader.ReadSignedVarLong();
            RuntimeId = reader.ReadUnsignedVarLong();

            GameMode = NetworkEnums.ReadGameMode(reader);

            reader.Read(ref SpawnPosition);
            reader.Read(ref Rotation);
            reader.Read(ref LevelSettings);
            LevelId = reader.ReadVarString();
            LevelName = reader.ReadVarString();
            TemplateContentIdentity = reader.ReadVarString();
            IsTrail = reader.ReadBool();
            reader.Read(ref PlayerMovementSettings);
            CurrentTick = reader.ReadUInt64();
            EnchantmentsSeed = reader.ReadSignedVarInt();

            BlockProperties = reader.ReadVarArray<BlockPropertyDefinition>();
            ItemList = reader.ReadVarArray<ItemData>();
            MultiplayerCorrelationId = reader.ReadVarString();
            EnableItemStackManagement = reader.ReadBool();
            ServerVersion = reader.ReadVarString();
            reader.Read(ref PlayerNBTData);
            BlockRegistryCheckSum = reader.ReadUInt64();
            WorldTemplateId = new Guid(reader.Reader.ReadSlice(16));
            EnabledClientSideGen = reader.ReadBool();
            BlockNetworkIdsAreHashed = reader.ReadBool();
            reader.Read(ref NetworkPermission);
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarLong(UniqueId);
            writer.WriteUnsignedVarLong(RuntimeId);
            NetworkEnums.Write(writer, GameMode);
            writer.Write(SpawnPosition);
            writer.Write(Rotation);

            writer.Write(LevelSettings);
            
            writer.WriteVarString(LevelId);
            writer.WriteVarString(LevelName);
            writer.WriteVarString(TemplateContentIdentity);
            writer.Write(IsTrail);

            writer.Write(PlayerMovementSettings);
            
            writer.Write(CurrentTick);
            writer.WriteSignedVarInt(EnchantmentsSeed);
            
            writer.WriteVarArray(BlockProperties);
            writer.WriteVarArray(ItemList);
            
            writer.WriteVarString(MultiplayerCorrelationId);
            
            writer.Write(EnableItemStackManagement);
            
            writer.WriteVarString(ServerVersion);

            writer.Write(PlayerNBTData);

            writer.Write(BlockRegistryCheckSum);

            writer.Writer.Write(WorldTemplateId.ToByteArray());

            writer.Write(EnabledClientSideGen);
            writer.Write(BlockNetworkIdsAreHashed);
            
            writer.Write(NetworkPermission);
        }
    }
}