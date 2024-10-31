using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    //unsigned varint array
    public struct PackDefinition() : INetworkType
    {
        public string Uuid = string.Empty;
        public string Version = string.Empty;
        public string Name = string.Empty;
        public void Read(ProtocolMemoryReader reader)
        {
            Uuid = reader.ReadVarString();
            Version = reader.ReadVarString();
            Name = reader.ReadVarString();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Uuid);
            writer.WriteVarString(Version);
            writer.WriteVarString(Name);
        }
    }
}
