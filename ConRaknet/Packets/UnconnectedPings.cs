using System.Buffers.Binary;
using System.Text;

namespace ConMaster.Raknet
{
    public struct UnconnectedPing
    {
        public long Time;
        public ulong Guid;
        public const byte PacketId = 0x01;
        public int PACKET_SIZE => 33;
        public UnconnectedPing Deserialize(ReadOnlySpan<byte> buffer)
        {
            Time = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(1));
            Guid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(25));
            return this;
        }

        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PacketId;
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(1), Time);
            Helper.CopyMagicTo(buffer.Slice(9));
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(25), Guid);
            return buffer.Slice(0, PACKET_SIZE);
        }
    }
    public struct UnconnectedPong
    {
        public long Time;
        public ulong Guid;
        public string MOTD;

        public const byte PacketId = 0x1c;
        public UnconnectedPong Deserialize(ReadOnlySpan<byte> buffer)
        {
            Time = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(1));
            Guid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(9));
            //MAGIC
            MOTD = Helper.ReadString16(buffer.Slice(33), Encoding.UTF8);
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PacketId;
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(1), Time);
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(9), Guid);
            Helper.CopyMagicTo(buffer.Slice(17));
            int data = Helper.WriteString16(buffer.Slice(33), MOTD, Encoding.UTF8);
            return buffer.Slice(0, 33 + data);
        }
        public int PACKET_SIZE => 35 + Encoding.UTF8.GetByteCount(MOTD);
    }
}
