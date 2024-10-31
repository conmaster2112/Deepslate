using ConMaster.Deepslate.Protocol.Enums;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class ResourcePackClientResponsePacket : BasePacket<ResourcePackClientResponsePacket>
    {
        public const int PACKET_ID = 0x8;
        public override int Id => PACKET_ID;
        public ResourcePackResponse Response = 0;
        public string[] Packs = [];
        public override void Clean()
        {
            Response = 0;
            Packs = [];
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            Response = (ResourcePackResponse)reader.ReadUInt8();
            Packs = new string[reader.ReadUInt16()];
            for (int i = 0; i < Packs.Length; i++) Packs[i] = reader.ReadVarString();
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write((byte)Response);
            ushort length = (ushort)(Packs?.Length ?? 0);
            writer.Write(length);
            for (int i = 0; i < length; i++) writer.WriteVarString(Packs?[i]);
        }
    }
}
