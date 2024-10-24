using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Buffers;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class CommandRequestPacket : BasePacket<CommandRequestPacket>
    {
        public const int PACKET_ID = 77;
        public override int Id => PACKET_ID;

        public string? Command = default;
        public CommandOriginData CommandOrigin = default;
        public bool IsInternalSource = default;
        public int Version = default;
        public override void Clean()
        {
            Command = default;
            CommandOrigin = default;
            IsInternalSource = default;
            Version = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            Command = reader.ReadVarString();
            reader.Read(ref CommandOrigin);
            IsInternalSource = reader.ReadBool();
            Version = reader.ReadSignedVarInt();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Command);
            writer.Write(ref CommandOrigin);
            writer.Write(IsInternalSource);
            writer.WriteSignedVarInt(Version);
        }
    }
}
