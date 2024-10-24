using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Buffers;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class DisconnectPacket : BasePacket<DisconnectPacket>
    {
        public const int PACKET_ID = 0x5;
        public override int Id => PACKET_ID;

        public DisconnectReason Reason = 0;
        public bool SkipMessage = false;
        public string Message = string.Empty;
        //public string FilteredMessage = string.Empty;
        public override void Clean()
        {
            Reason = 0;
            SkipMessage = false;
            Message = string.Empty;
            //FilteredMessage = string.Empty;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            Reason = NetworkEnums.ReadDisconnectReason(reader);
            SkipMessage = reader.ReadBool();
            if (!SkipMessage)
            {
                Message = reader.ReadVarString();
                //FilteredMessage = reader.ReadVarString();
            }
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            NetworkEnums.Write(writer, Reason);
            writer.Write(SkipMessage);
            if (!SkipMessage)
            {
                ReadOnlySpan<byte> raw = Message.GetBytes();
                writer.WriteVarStringRaw(raw);
                writer.WriteVarStringRaw(raw); //Filtered
                // writer.WriteVarString(FilteredMessage);
            }
        }
    }
}
