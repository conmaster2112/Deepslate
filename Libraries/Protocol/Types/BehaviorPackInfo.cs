using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct BehaviorPackInfo : INetworkType
    {
        public PackInfo Info;
        public void Read(ProtocolMemoryReader reader) => Info.Read(reader);
        public void Write(ProtocolMemoryWriter writer) => Info.Write(writer);
    }
}
