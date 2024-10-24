using ConMaster.Buffers;

namespace ConMaster.Deepslate.Network
{
    public interface IHandleRawPacketHandler
    {
        public int PacketId { get; }
        public bool HandleRawPacket(BaseProtocol proto, Client client, ConstantMemoryBufferReader reader);
    }
    /*
    public class ProtocolPacketHandler<T>(Action<Client, T> action, int packetId): IHandleRawPacketHandler
        where T : class,
        IPoolable<T>,
        IPacket,
        new()
    {
        public Action<Client, T> Action { get; init; } = action;
        public int PacketId { get; init; } = packetId;
        public void HandleRawPacket(Protocol proto, Client client, ConstantMemoryBufferReader reader)
        {
            T packet = IPoolable<T>.Rent();
            packet.Read(reader);
            Action(client, packet);
            packet.Dispose();
        }
    }
    public class ProtocolPacketAsyncHandler<T>(Func<Client, T, ValueTask> action, int packetId) : IHandleRawPacketHandler
        where T : class,
        IPoolable<T>,
        IPacket,
        new()
    {
        public Func<Client, T, ValueTask> Action { get; init; } = action;
        public int PacketId { get; init; } = packetId;
        public void HandleRawPacket(Protocol proto, Client client, ConstantMemoryBufferReader reader)
        {
            T packet = IPoolable<T>.Rent();
            packet.Read(reader);
            RunAction(proto, client, packet);
        }
        public async void RunAction(Protocol proto,Client client, T packet)
        {
            try
            {
                //Awaits for free thread to invoke action on
                //Awaits for actual task to complete
                await await Task.Run(() => Action(client, packet));
            }
            catch (Exception ex)
            {
                proto.Log.Error(ex.Message + " " + ex.StackTrace ?? "");
            }
            finally
            {
                packet.Dispose();
            }
        }
    }
    public class ProtocolPacketWorkerHandler<T>(Action<Client, T> action, int packetId) : IHandleRawPacketHandler
        where T : class,
        IPoolable<T>,
        IPacket,
        new()
    {
        public Action<Client, T> Action { get; init; } = action;
        public int PacketId { get; init; } = packetId;
        public void HandleRawPacket(Protocol proto, Client client, ConstantMemoryBufferReader reader)
        {
            T packet = IPoolable<T>.Rent();
            packet.Read(reader);
            RunAction(proto, client, packet);
        }
        public async void RunAction(Protocol proto, Client client, T packet)
        {
            try
            {

                await Task.Run(() => Action(client, packet));
            }
            catch (Exception ex)
            {
                proto.Log.Error(ex.Message + " " + ex.StackTrace ?? "");
            }
            finally
            {
                packet.Dispose();
            }
        }
    }*/
}
