using ConMaster.Deepslate.Protocol.Types;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class UpdateAbilitiesPacket : BasePacket<UpdateAbilitiesPacket>
    {
        public const int PACKET_ID = 187;
        public override int Id => PACKET_ID;

        public PlayerAbilitiesData Data;
        public override void Clean()
        {
            Data = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            reader.Read(ref Data);
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(ref Data);
        }
    }
}
