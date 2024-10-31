using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class SetPlayerGameModePacket : BasePacket<SetPlayerGameModePacket>
    {
        public const int PACKET_ID = 62;
        public override int Id => PACKET_ID;
        public GameMode GameMode;

        public override void Clean()
        {
            GameMode = default;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            GameMode = (GameMode)reader.ReadSignedVarInt();
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt((int)GameMode);
        }
    }
}
