using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public struct PacketFactoryPacket<T>(Func<BasePacket<T>> builder) : IPacket where T : BasePacket<T>, new()
    {
        public Func<BasePacket<T>> Builder = builder;
        public BasePacket<T>? Cache;
        public BasePacket<T> GetPacket() => Cache ??= Builder();
        public int Id => GetPacket().Id;
        public void Read(ProtocolMemoryReader reader)
        {
            GetPacket().Read(reader);
        }
        //Packet Wrapper is only outgoing type
        public void Write(ProtocolMemoryWriter writer)
        {
            using BasePacket<T> packet = GetPacket();
            packet.Write(writer);
        }
    }
}
