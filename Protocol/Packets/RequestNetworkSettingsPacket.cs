using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class RequestNetworkSettingsPacket : BasePacket<RequestNetworkSettingsPacket>
    {
        public const int PACKET_ID = 193;
        public override int Id => PACKET_ID;
        public int ProtocolVersion = 0;
        public override void Clean() // Struct types doesn't need release
        {
            ProtocolVersion = 0;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            ProtocolVersion = reader.ReadInt32BE();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteBE(ProtocolVersion);
        }
    }
}
