using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Buffers;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class ClientCacheStatusPacket : BasePacket<ClientCacheStatusPacket>
    {
        public const int PACKET_ID = 0x81;
        public override int Id => PACKET_ID;

        public bool SupportsCache = false;
        //public string FilteredMessage = string.Empty;
        public override void Clean()
        {
            SupportsCache = false;
            //FilteredMessage = string.Empty;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            SupportsCache = reader.ReadBool();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(SupportsCache);
        }
    }
}
