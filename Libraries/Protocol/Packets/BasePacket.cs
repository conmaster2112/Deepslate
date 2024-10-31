using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public abstract class BasePacket<T>: IPoolable<T>, IPacket where T : BasePacket<T>, IPoolable<T>, new()
    {
        public static T Create()
        {
            return IPoolable<T>.Rent();
        }
        protected static readonly T Default = Create();
        public static int PacketId => Default.Id;
        public abstract int Id { get; }
        public abstract void Clean();
        public abstract void Read(ProtocolMemoryReader reader);
        public abstract void Write(ProtocolMemoryWriter writer);
        public static T CreateWith(ProtocolMemoryReader reader)
        {
            T value = Create();
            value.Read(reader);
            return value;
        }
        public static void Read(T value, ProtocolMemoryReader reader)=>value.Read(reader);
        public static void Write(T value, ProtocolMemoryWriter writer)=>value.Write(writer);
        // virtual void Dispose() => ((IPoolable<T>)this).Dispose();
    }
}
