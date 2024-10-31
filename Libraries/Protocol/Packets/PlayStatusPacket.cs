using ConMaster.Deepslate.Protocol.Enums;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class PlayStatusPacket : BasePacket<PlayStatusPacket>
    {
        public const int PACKET_ID = 2;
        public override int Id => PACKET_ID;
        public PlayStatus Status;
        public override void Clean()
        {
            Status = 0;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            Status = (PlayStatus)reader.ReadInt32BE();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteBE((int)Status);
            return;
        }
        public static PlayStatusPacket FromStatus(PlayStatus status)
        {
            var packet = Create();
            packet.Status = status;
            return packet;
        }
    }
}
