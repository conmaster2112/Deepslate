using System.Runtime.CompilerServices;
namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BigEndian(this ReadOnlySpan<byte> buffer)
        {
            int value = 0;
            value |= buffer[2];
            value |= buffer[1] << 8;
            value |= buffer[0] << 16;
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LittleEndian(this ReadOnlySpan<byte> buffer)
        {
            int value = 0;
            value |= buffer[0];
            value |= buffer[1] << 8;
            value |= buffer[2] << 16;
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BigEndian(this Span<byte> reader) => ReadUInt24BigEndian(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LittleEndian(this Span<byte> reader) => ReadUInt24LittleEndian(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24LittleEndian(this Span<byte> buffer, int value)
        {
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24BigEndian(this Span<byte> buffer, int value)
        {
            buffer[2] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[0] = (byte)(value >> 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            int v = ReadUInt24BigEndian(reader.Slice(offset, 3));
            offset += 3;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LittleEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            int v = ReadUInt24LittleEndian(reader.Slice(offset, 3));
            offset += 3;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BigEndian(this Span<byte> reader, ref int offset) => ReadUInt24BigEndian(reader, ref offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LittleEndian(this Span<byte> reader, ref int offset) => ReadUInt24LittleEndian(reader, ref offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24LittleEndian(this Span<byte> writer, ref int offset, int value)
        {
            WriteUInt24LittleEndian(writer.Slice(offset, 3), value);
            offset += 3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24BigEndian(this Span<byte> writer, ref int offset, int value)
        {
            WriteUInt24BigEndian(writer.Slice(offset, 3), value);
            offset += 3;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BigEndian(this ConstantMemoryBufferReader reader) => reader.Span.ReadUInt24BigEndian(ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LittleEndian(this ConstantMemoryBufferReader reader) => reader.Span.ReadUInt24LittleEndian(ref reader.Offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24LittleEndian(this ConstantMemoryBufferWriter writer, int value) => writer.Span.WriteUInt24LittleEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24BigEndian(this ConstantMemoryBufferWriter writer, int value) => writer.Span.WriteUInt24BigEndian(ref writer.Offset, value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BigEndian(this BinaryStream reader) => reader.LoadSpan(stackalloc byte[3]).ReadUInt24BigEndian();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LittleEndian(this BinaryStream reader) => reader.LoadSpan(stackalloc byte[3]).ReadUInt24LittleEndian();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24LittleEndian(this BinaryStream writer, int value) {
            Span<byte> span = stackalloc byte[3];
            span.WriteUInt24LittleEndian(value);
            writer.Write(span);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24BigEndian(this BinaryStream writer, int value)
        {
            Span<byte> span = stackalloc byte[3];
            span.WriteUInt24BigEndian(value);
            writer.Write(span);
        }
    }
}
