using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class PacketViolationWarningPacket : BasePacket<PacketViolationWarningPacket>
    {
        public const int PACKET_ID = 156;
        public override int Id => 156;
        public int ViolationType = 0;
        public int ViolationSeverity = 0;
        public int ViolationPacketId = 0;
        public string ViolationContext = string.Empty;
        public override void Clean()
        {
            ViolationType = 0;
            ViolationSeverity = 0;
            ViolationPacketId = 0;
            ViolationContext = string.Empty;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            ViolationType = reader.ReadSignedVarInt();
            ViolationSeverity = reader.ReadSignedVarInt();
            ViolationPacketId = reader.ReadSignedVarInt();
            ViolationContext = reader.ReadVarString();
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
