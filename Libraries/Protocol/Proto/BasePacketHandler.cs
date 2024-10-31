using ConMaster.Buffers;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Packets;

namespace ConMaster.Deepslate.Protocol.Proto
{
    public class BasePacketHandler<T> : IHandleRawPacketHandler where T : BasePacket<T>, new()
    {
        public int PacketId => BasePacket<T>.PacketId;
        public static T Create() => BasePacket<T>.Create();
        public event Action<BaseProtocol, Client, T>? OnRecieved;
        public bool HandleRawPacket(BaseProtocol proto, Client client, ConstantMemoryBufferReader reader)
        {
            using T packet = Create();
            packet.Read(reader);
            if (OnRecieved != null)
            {
                OnRecieved(proto, client, packet);
                return true;
            }
            return false;
        }
    }
}
