using System.Buffers.Binary;

namespace ConMaster.Raknet.Packets
{
    public struct OpenConnectionRequest1
    {
        public byte ProtocolVersion;
        public short MTU;
        public const byte PacketId = 0x05;
        public int PACKET_SIZE => 0xffff;

        public OpenConnectionRequest1 Deserialize(ReadOnlySpan<byte> buffer)
        {
            ProtocolVersion = buffer[17];
            MTU = (short)(buffer.Length - 18);
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PacketId;
            Helper.CopyMagicTo(buffer.Slice(1));
            buffer[17] = ProtocolVersion;
            return buffer;
        }
    }
    public struct OpenConnectionReply1
    {
        public ulong ServerGuid;
        public bool UseSecurity;
        public int Cookie;
        public short MTU;

        public const byte PacketId = 0x06;
        public int PACKET_SIZE => 32;
        public OpenConnectionReply1 Deserialize(ReadOnlySpan<byte> buffer)
        {
            //0
            //1
            ServerGuid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(17)); //17
            UseSecurity = buffer[25] > 0; //2
            Cookie = BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(26));
            MTU = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(30));
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PacketId; 
            Helper.CopyMagicTo(buffer.Slice(1));
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(17), ServerGuid);
            buffer[25] = (byte)(UseSecurity ? 1 : 0);
            BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(26), Cookie);
            BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(30), MTU);
            return buffer.Slice(0, PACKET_SIZE);
        }
    }
}
