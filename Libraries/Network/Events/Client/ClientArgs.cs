namespace ConMaster.Deepslate.Network.Events.Client
{
    public class ClientArgs(Network.Client client): EventArgs
    {
        public Network.Client Client { get; protected init; } = client;
        public Server Server { get; protected init; } = client.Server;
    }
    public class ClientDisconnectedEventArgs(Network.Client client) : ClientArgs(client);
    public class ClientConnectedEventArgs(Network.Client client) : ClientArgs(client);
}