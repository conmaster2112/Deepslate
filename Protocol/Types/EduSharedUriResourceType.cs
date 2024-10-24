using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct EduSharedUriResourceType: INetworkType
    {
        public string ButtonName;
        public string LinkUri;

        public void Read(ProtocolMemoryReader reader)
        {
            ButtonName = reader.ReadVarString();
            LinkUri = reader.ReadVarString();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(ButtonName);
            writer.WriteVarString(LinkUri);
        }
    }
}
