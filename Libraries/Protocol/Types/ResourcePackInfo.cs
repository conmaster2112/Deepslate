using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct ResourcePackInfo : INetworkType
    {
        public PackInfo Info;
        public bool RTXEnabled;
        public string CdnUrl;
        public void Read(ProtocolMemoryReader reader)
        {
            reader.Read(ref Info);
            RTXEnabled = reader.ReadBool();
            CdnUrl = reader.ReadVarString();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(Info);
            writer.Write(RTXEnabled);
            writer.WriteVarString(CdnUrl);
        }
    }
}
