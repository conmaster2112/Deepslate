using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct SpawnSettings: INetworkType
    {
        public short Type;
        public string BiomeName;
        public int DimensionNetwrokId;

        public void Read(ProtocolMemoryReader reader)
        {
            Type = reader.ReadInt16();
            BiomeName =  reader.ReadVarString();
            DimensionNetwrokId = reader.ReadSignedVarInt();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(Type);
            writer.WriteVarString(BiomeName);
            writer.WriteSignedVarInt(DimensionNetwrokId);
        }
    }
}
