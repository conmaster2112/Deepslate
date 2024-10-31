using ConMaster.Buffers;

namespace ConMaster.Deepslate.Network
{
    public abstract class BaseProtocol(int version = default)
    {
        public event ErrorEventHandler? OnError;
        public event Action<object, string>? OnWarn;
        public int ProtocolVersion { get; init; } = version;
        public abstract void HandlePacketPayload(ReadOnlySpan<byte> packetPayload, Client client);
        public abstract T AddPacketHandler<T>(T handler) where T: IHandleRawPacketHandler;
        public virtual bool TryAddPacketHandler(IHandleRawPacketHandler handler)
        {
            try
            {
                AddPacketHandler(handler);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected void InvokeError(Exception ex) => OnError?.Invoke(this, new ErrorEventArgs(ex));
        protected void InvokeWarn(string ex) => OnWarn?.Invoke(this, ex);
    }
}
