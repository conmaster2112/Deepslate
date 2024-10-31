using System.Net;

namespace ConMaster
{
    public delegate void DataReceivedEventHandler(object sender, ReadOnlySpan<byte> data);
    public interface IConnectionProvider
    {
        public event DataReceivedEventHandler DataReceived;
        public ulong GuildId { get; }
        public IPEndPoint Address { get; }
        public ConnectionState State { get; }
        public IServerProvider Server { get; }
        public void SendPacket(ReadOnlySpan<byte> data);
        public void Disconnect();
    }
}