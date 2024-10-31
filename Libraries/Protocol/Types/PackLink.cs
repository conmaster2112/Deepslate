using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct PackLink : INetworkType
    {
        public string Id;
        public string Url;
        public void Read(ProtocolMemoryReader reader)
        {
            Id = reader.ReadVarString();
            Url = reader.ReadVarString();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Id);
            writer.WriteVarString(Url);
        }
    }
}
