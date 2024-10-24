using ConMaster.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Deepslate.Network.InternalPackets
{
    public struct DisconnectPacketInternal : IPacket, INetworkType
    {
        public const int PACKET_ID = 0x5;
        public readonly int Id => PACKET_ID;

        public int Reason;
        public bool SkipMessage;
        public string Message;
        //public string FilteredMessage = string.Empty;

        public void Read(ProtocolMemoryReader reader)
        {
            Reason = reader.ReadSignedVarInt();
            SkipMessage = reader.ReadBool();
            if (!SkipMessage)
            {
                Message = reader.ReadVarString();
                //FilteredMessage = reader.ReadVarString();
            }
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(Reason);
            writer.Write(SkipMessage);
            if (!SkipMessage)
            {
                ReadOnlySpan<byte> raw = Message.GetBytes();
                writer.WriteVarStringRaw(raw);
                writer.WriteVarStringRaw(raw);
                //Filtered
                // writer.WriteVarString(FilteredMessage);
            }
        }
    }
}
