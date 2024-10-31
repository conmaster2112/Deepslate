using System.Net.Sockets;
using System.Net;

namespace ConMaster.Raknet
{
#if DEBUG
    public class Proxy
    {
        private readonly Socket ListenSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private readonly Socket ForwardSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public readonly IPEndPoint ServerEndpoint;
        private readonly IPEndPoint EndPointFactory = new(IPAddress.Any, 0);
        private EndPoint? ClientEndPoint;
        private UnconnectedPing lastPing;
        public Proxy(IPAddress forwardEndpoint, IPAddress listenEndpoint, ushort serverPort, ushort clientPort)
        {
            ServerEndpoint = new IPEndPoint(forwardEndpoint, serverPort);
            ListenSocket.Bind(new IPEndPoint(listenEndpoint, clientPort));
            ForwardSocket.Bind(new IPEndPoint(IPAddress.Any, 28501));
        }
        public Task Start()
        {
            return Task.WhenAll(ClientLoop(), ServerLoop());
        }
        private async Task ClientLoop()
        {
            byte[] buffer = GC.AllocateUninitializedArray<byte>(2048);
            SocketAddress address = new(ListenSocket.AddressFamily);
            while (true)
            {
                int receivedLength = await ListenSocket.ReceiveFromAsync(buffer, SocketFlags.None, address);
                ClientEndPoint ??= EndPointFactory.Create(address);
                await ForwardSocket.SendToAsync(buffer.AsMemory(0, receivedLength), ServerEndpoint);
            }
        }
        private async Task ServerLoop()
        {
            byte[] buffer = GC.AllocateUninitializedArray<byte>(2048);
            SocketAddress address = new(ForwardSocket.AddressFamily);
            while (true)
            {
                int receivedLength = await ForwardSocket.ReceiveFromAsync(buffer, SocketFlags.None, address);
                if (ClientEndPoint != null) await ListenSocket.SendToAsync(buffer.AsMemory(0, receivedLength), SocketFlags.None, ClientEndPoint);
            }
        }
    }
#endif
}
