using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        #region ZigZag Encoding
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ZigZagEncode(int value) => (uint)((value << 1) ^ (value >> 31));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ZigZagDecode(uint value) => (int)((value >> 1) ^ -(value & 1));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ZigZagEncode(long value) => (ulong)((value << 1) ^ (value >> 31));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ZigZagDecode(ulong value) => (long)(value >> 1) ^ -((long)value & 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ZigZag(this int value) => ZigZagEncode(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ZigZag(this uint value) => ZigZagDecode(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ZigZag(this long value) => ZigZagEncode(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ZigZag(this ulong value) => ZigZagDecode(value);
        #endregion

        #region Span Extensions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUVarInt32(this ReadOnlySpan<byte> buffer, ref int offset)
        {
            uint value = 0;
            for(int i = 0, shift = 0; i < 5; i++, shift+=7)
            {
                byte data = buffer[offset++];
                value |= (uint)(data & 0b0111_1111) << shift;
                if ((data & 0b1000_0000) == 0) return value;
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadVarInt32(this ReadOnlySpan<byte> buffer, ref int offset) => ReadUVarInt32(buffer, ref offset).ZigZag();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUVarInt32(this Span<byte> buffer, ref int offset, uint value)
        {
            for (int i = 0, shift = 0; i < 5; i++, shift += 7)
            {
                byte canContinue = (value > 0b0111_1111)?(byte)0b1000_0000:(byte)0;
                buffer[offset++] = (byte)((value & 0b0111_1111) | canContinue);
                if (canContinue == 0) return;
                value >>= 7;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt32(this Span<byte> writer, ref int offset, int value) => WriteUVarInt32(writer, ref offset, value.ZigZag());


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUVarInt64(this ReadOnlySpan<byte> buffer, ref int offset)
        {
            ulong value = 0;
            for (int i = 0, shift = 0; i < 10; i++, shift += 7)
            {
                byte data = buffer[offset++];
                value = (ulong)(data & 0b0111_1111) << shift;
                if ((data & 0b1000_0000) == 0) return value;
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadVarInt64(this ReadOnlySpan<byte> buffer, ref int offset) => ReadUVarInt64(buffer, ref offset).ZigZag();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUVarInt64(this Span<byte> buffer, ref int offset, ulong value)
        {
            for (int i = 0, shift = 0; i < 10; i++, shift += 7)
            {
                byte canContinue = (value > 0b0111_1111) ? (byte)0b1000_0000 : (byte)0;
                buffer[offset++] = (byte)((value & 0b0111_1111) | canContinue);
                if (canContinue == 0) return;
                value >>= 7;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt64(this Span<byte> buffer, ref int offset, long value) => WriteUVarInt64(buffer, ref offset, value.ZigZag());

        #endregion

        #region ConstantStreamBuffer Extensions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUVarInt32(this ConstantMemoryBufferReader reader) => reader.Span.ReadUVarInt32(ref reader.Offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadVarInt32(this ConstantMemoryBufferReader reader) => reader.Span.ReadVarInt32(ref reader.Offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUVarInt32(this ConstantMemoryBufferWriter writer, uint value) => writer.Span.WriteUVarInt32(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt32(this ConstantMemoryBufferWriter writer, int value) => writer.Span.WriteVarInt32(ref writer.Offset, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUVarInt64(this ConstantMemoryBufferReader reader) => reader.Span.ReadUVarInt64(ref reader.Offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadVarInt64(this ConstantMemoryBufferReader reader) => reader.Span.ReadVarInt64(ref reader.Offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUVarInt64(this ConstantMemoryBufferWriter writer, ulong value) => writer.Span.WriteUVarInt64(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt64(this ConstantMemoryBufferWriter writer, long value) => writer.Span.WriteVarInt64(ref writer.Offset, value);
        #endregion

        #region BinaryStream Extensions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUVarInt32(this BinaryStream stream)
        {

            uint value = 0;
            for (int i = 0, shift = 0; i < 5; i++, shift += 7)
            {
                byte data = stream.ReadUInt8();
                value |= (uint)(data & 0b0111_1111) << shift;
                if ((data & 0b1000_0000) == 0) return value;
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadVarInt32(this BinaryStream stream) => ReadUVarInt32(stream).ZigZag();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUVarInt32(this BinaryStream stream, uint value)
        {
            Span<byte> span = stackalloc byte[5];
            int r = 0;
            span.WriteUVarInt32(ref r, value);
            stream.Write(span.Slice(0, r));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt32(this BinaryStream stream, int value)
        {
            Span<byte> span = stackalloc byte[5];
            int r = 0;
            span.WriteVarInt32(ref r, value);
            stream.Write(span.Slice(0, r));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUVarInt64(this BinaryStream stream)
        {
            ulong value = 0;
            for (int i = 0, shift = 0; i < 10; i++, shift += 7)
            {
                byte data = stream.ReadUInt8();
                value = (ulong)(data & 0b0111_1111) << shift;
                if ((data & 0b1000_0000) == 0) return value;
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadVarInt64(this BinaryStream stream) => ReadUVarInt64(stream).ZigZag();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUVarInt64(this BinaryStream stream, ulong value)
        {
            Span<byte> span = stackalloc byte[10];
            int r = 0;
            span.WriteUVarInt64(ref r, value);
            stream.Write(span.Slice(0, r));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt64(this BinaryStream stream, long value)
        {
            Span<byte> span = stackalloc byte[10];
            int r = 0;
            span.WriteVarInt64(ref r, value);
            stream.Write(span.Slice(0, r));
        }
        #endregion
    }
}
