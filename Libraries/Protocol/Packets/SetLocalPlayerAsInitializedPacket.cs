using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class SetLocalPlayerAsInitializedPacket : BasePacket<SetLocalPlayerAsInitializedPacket>
    {
        public const int PACKET_ID = 113;
        public override int Id => PACKET_ID;

        public ulong ActorRuntimeId;
        public override void Clean()
        {
            ActorRuntimeId = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            ActorRuntimeId = reader.ReadUnsignedVarLong();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarLong(ActorRuntimeId);
        }
    }
}
