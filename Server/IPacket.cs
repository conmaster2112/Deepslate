namespace ConMaster.Deepslate.Network
{
    public interface IPacket: INetworkType, IPostable
    {
        public int Id { get; }
        IPacket IPostable.AsPacket() => this;
    }
    public interface IPostable
    {
        public IPacket AsPacket();
    }
    public interface ISerializable
    {
        public INetworkType AsNetworkType();
    }
}
