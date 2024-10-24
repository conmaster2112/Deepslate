using System.Buffers.Binary;

namespace ConMaster.Raknet
{
    public struct IncompatibleProtocol
    {
        public byte ProtocolVersion;
        public ulong ServerGuid;
        public const byte PackedId = 0x19;
        public int PACKET_SIZE => 26;

        public IncompatibleProtocol Deserialize(ReadOnlySpan<byte> buffer)
        {
            ProtocolVersion = buffer[1];
            ServerGuid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(18));
            return this;
        }

        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PackedId;
            buffer[1] = ProtocolVersion;
            Helper.CopyMagicTo(buffer.Slice(2));
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(18), ServerGuid);
            return buffer.Slice(0,PACKET_SIZE);
        }
    }
}
