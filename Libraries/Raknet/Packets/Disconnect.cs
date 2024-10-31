namespace ConMaster.Raknet
{
    public struct DisconnectPacket
    {
        public const byte PackedId = 0x15;
        public readonly int PACKET_SIZE => 1;
        private static readonly byte[] PACKET_DATA_PRIVATE = [PackedId];
        public static ReadOnlySpan<byte> PACKET_DATA => PACKET_DATA_PRIVATE; 

        public readonly DisconnectPacket Deserialize(ReadOnlySpan<byte> buffer)
        {
            return this;
        }

        public readonly Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PackedId;
            return buffer;
        }
    }
}
