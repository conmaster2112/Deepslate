using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct ItemData : INetworkType
    {
        public string TypeId;
        public short NetworkId;
        public bool IsComponentBase;
        public void Read(ProtocolMemoryReader reader)
        {
            TypeId = reader.ReadVarString();
            NetworkId = reader.ReadInt16();
            IsComponentBase = reader.ReadBool();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(TypeId);
            writer.Write(NetworkId);
            writer.Write(IsComponentBase);
        }
    }
}
