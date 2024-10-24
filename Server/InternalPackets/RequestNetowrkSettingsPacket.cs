using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Deepslate.Network.InternalPackets
{
    public struct RequestNetworkSettingsPacketInternal : IPacket, INetworkType
    {
        public const int PACKET_ID = 193;
        public readonly int Id => PACKET_ID;
        public int ProtocolVersion;
        public void Read(ProtocolMemoryReader reader)
        {
            ProtocolVersion = reader.ReadInt32BE();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteBE(ProtocolVersion);
        }
    }
}
