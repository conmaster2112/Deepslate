using ConMaster.Deepslate.Protocol.Types;
using System.Reflection.PortableExecutable;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class PlayerActionPacket : BasePacket<PlayerActionPacket>
    {
        public const int PACKET_ID = 36;
        public override int Id => PACKET_ID;
        public ulong EntityRuntimeId;
        public int ActionId;
        public  Vec3i  BlockPosition;
        public  Vec3i  ResultPosition;
        public int Face;
        public override void Clean()
        {
            EntityRuntimeId = default;
            ActionId = default;
            BlockPosition = default;
            ResultPosition = default;
            Face = default;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            EntityRuntimeId = reader.ReadUnsignedVarLong();
            ActionId = reader.ReadSignedVarInt();
            reader.Read(ref  BlockPosition);
            reader.Read(ref ResultPosition);
            Face = reader.ReadSignedVarInt();
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarLong(EntityRuntimeId);
            writer.WriteSignedVarInt(ActionId);
            writer.Write(ref  BlockPosition);
            writer.Write(ref ResultPosition);
            writer.WriteSignedVarInt(Face);
        }
    }
}
