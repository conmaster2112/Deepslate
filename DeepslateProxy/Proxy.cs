using ConMaster.Raknet;
using System.Net;
using System.Net.Sockets;

namespace DeepslateProxy
{
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
                if(ClientEndPoint != null) await ListenSocket.SendToAsync(buffer.AsMemory(0, receivedLength), SocketFlags.None, ClientEndPoint);
            }
        }
        public class ProxyClient
        {
            public readonly EndPoint ClientEndpoint;
            public readonly EndPoint ServerEndpoint;
            public readonly Socket ClientPipe;
            public readonly Socket ServerPipe = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            protected ProxyClient(IPEndPoint client, Socket clientPipe, EndPoint serverEndpoint)
            {
                ClientEndpoint = client;
                ClientPipe = clientPipe;
                ServerPipe.Bind(new IPEndPoint(IPAddress.Any, client.Port + 31));
            }
        }
    }
    public class UdpProxy
    {
        private const int listenPort1 = 3000; // Port to listen on
        private const int forwardPort1 = 19138; // Port to forward to
        private const int listenPort2 = 19138; // Port to listen on for reverse direction
        private const int forwardPort2 = 3000; // Port to forward to for reverse direction

        public static async Task StartProxy2()
        {
            Socket listener1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket forwarder1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket listener2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket forwarder2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            listener1.Bind(new IPEndPoint(IPAddress.Any, listenPort1));
            listener2.Bind(new IPEndPoint(IPAddress.Any, listenPort2));
            Console.WriteLine($"Listening for UDP packets on ports {listenPort1} and {listenPort2}...");

            byte[] buffer1 = new byte[1024];
            byte[] buffer2 = new byte[1024];

            Task receiveTask1 = Task.Run(async () =>
            {
                while (true)
                {
                    EndPoint remoteEP1 = new IPEndPoint(IPAddress.Any, 0);
                    SocketReceiveFromResult receiveResult1 = await listener1.ReceiveFromAsync(new ArraySegment<byte>(buffer1), SocketFlags.None, remoteEP1);

                    Console.WriteLine($"Received packet from {receiveResult1.RemoteEndPoint} on port {listenPort1}");

                    // Forward the packet to the forwardPort1 only if it's not from the forwardPort2
                    if (!((IPEndPoint)receiveResult1.RemoteEndPoint).Port.Equals(forwardPort2))
                    {
                        EndPoint forwardEP1 = new IPEndPoint(IPAddress.Loopback, forwardPort1);
                        await forwarder1.SendToAsync(new ArraySegment<byte>(buffer1, 0, receiveResult1.ReceivedBytes), SocketFlags.None, forwardEP1);

                        Console.WriteLine($"Forwarded packet to 127.0.0.1:{forwardPort1}");
                    }
                }
            });

            Task receiveTask2 = Task.Run(async () =>
            {
                while (true)
                {
                    EndPoint remoteEP2 = new IPEndPoint(IPAddress.Any, 0);
                    SocketReceiveFromResult receiveResult2 = await listener2.ReceiveFromAsync(new ArraySegment<byte>(buffer2), SocketFlags.None, remoteEP2);

                    Console.WriteLine($"Received packet from {receiveResult2.RemoteEndPoint} on port {listenPort2}");

                    // Forward the packet to the forwardPort2 only if it's not from the forwardPort1
                    if (!((IPEndPoint)receiveResult2.RemoteEndPoint).Port.Equals(forwardPort1))
                    {
                        EndPoint forwardEP2 = new IPEndPoint(IPAddress.Loopback, forwardPort2);
                        await forwarder2.SendToAsync(new ArraySegment<byte>(buffer2, 0, receiveResult2.ReceivedBytes), SocketFlags.None, forwardEP2);

                        Console.WriteLine($"Forwarded packet to 127.0.0.1:{forwardPort2}");
                    }
                }
            });

            await Task.WhenAll(receiveTask1, receiveTask2);
        }

        public static Dictionary<int, UdpProxyClient> Clients = [];
        public static async Task StartProxy()
        {
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint endpoint = new IPEndPoint(IPAddress.Loopback, listenPort1);
            EndPoint server = new IPEndPoint(IPAddress.Loopback, forwardPort1);
            SocketAddress address = new(AddressFamily.InterNetwork);
            socket.Bind(endpoint);
            byte[] buffer = new byte[2048];
            while (true)
            {
                int dataReceived = await socket.ReceiveFromAsync(buffer, SocketFlags.None, address);
                if(!Clients.TryGetValue(address.GetHashCode(), out UdpProxyClient? client)) client = new UdpProxyClient(endpoint.Create(address), server);
                await client.Send(buffer.AsMemory(0, dataReceived));
            }
        }

        public static IPEndPoint m_listenEp = new IPEndPoint(IPAddress.Any, 3000);
        public static EndPoint m_connectedClientEp = new IPEndPoint(IPAddress.Any, 3000);
        public static EndPoint m_connectedClientEp2 = new IPEndPoint(IPAddress.Any, 3000);
        public static IPEndPoint m_sendEp = new IPEndPoint(IPAddress.Loopback, 19138);
        public static Socket m_UdpListenSocket = new Socket(m_listenEp.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        public static Socket m_UdpSendSocket = null;


        public static void MainProxyTest(string[] args)
        {

            // Creates Listener UDP Server
            m_UdpListenSocket.Bind(m_listenEp);

            byte[] data = new byte[2024];

            while (true)
            {
                if (m_UdpListenSocket.Available > 0)
                {

                    int size = m_UdpListenSocket.ReceiveFrom(data, ref m_connectedClientEp); //client to listener

                    if (m_UdpSendSocket == null)
                    {
                        // Connect to UDP Game Server.
                        m_UdpSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    }

                    m_UdpSendSocket.SendTo(data, size, SocketFlags.None, m_sendEp); //listener to server.

                }

                if (m_UdpSendSocket != null && m_UdpSendSocket.Available > 0)
                {
                    int size = 0;
                    try
                    {
                        size = m_UdpSendSocket.ReceiveFrom(data, ref m_connectedClientEp2); //server to client.
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Was throwen");
                    }
                    m_UdpListenSocket.SendTo(data, size, SocketFlags.None, m_connectedClientEp); //listner
                }
                Thread.Sleep(10);
                if (Console.KeyAvailable)
                {
                    // Wait for any key to terminate application
                    var a = Console.ReadKey();
                    if (a.Key == ConsoleKey.Enter)
                    {
                        m_UdpListenSocket.Close();
                        m_UdpSendSocket?.Close();
                    }
                }
            }


        }
    }
    public class UdpProxyClient(EndPoint endPoint, EndPoint server)
    {
        public EndPoint EndPoint { get; set; } = endPoint;
        public EndPoint Server { get; set; } = server;
        public Socket Socket { get; set; } = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public async Task Send(Memory<byte> data)
        {
            Console.WriteLine("Send: " + data.Length);
            await Socket.SendToAsync(data, Server);
        }
        public async Task<UdpProxyClient> Init()
        {
            Socket.Bind(EndPoint);
            return this;
        } 
    }
}
