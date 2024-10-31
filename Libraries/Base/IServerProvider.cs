namespace ConMaster
{
    public interface IServerProvider
    {
        public event Action<object, IConnectionProvider> OnClientConnected;
        public event Action<object, IConnectionProvider> OnClientDisconnected;
        public event ErrorEventHandler OnError;
        public void Start();
        public void Stop();
    }
}