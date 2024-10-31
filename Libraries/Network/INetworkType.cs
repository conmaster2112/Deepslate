namespace ConMaster.Deepslate.Network
{
    public interface INetworkType: ISerializable
    {
        public void Write(ProtocolMemoryWriter writer);
        public void Read(ProtocolMemoryReader reader);
        INetworkType ISerializable.AsNetworkType() => this;
    }
}
