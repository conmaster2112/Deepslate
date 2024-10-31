using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, sbyte value) => writer[offset++] = (byte)value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, byte value) => writer[offset++] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, bool value) => writer[offset++] = (byte)(value?1:0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, short value)
        {
            BinaryPrimitives.WriteInt16BigEndian(writer.Slice(offset, sizeof(short)), value);
            offset += sizeof(short);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, int value)
        {
            BinaryPrimitives.WriteInt32BigEndian(writer.Slice(offset, sizeof(int)), value);
            offset += sizeof(int);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, long value)
        {
            BinaryPrimitives.WriteInt64BigEndian(writer.Slice(offset, sizeof(long)), value);
            offset += sizeof(long);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(writer.Slice(offset, sizeof(ushort)), value);
            offset += sizeof(ushort);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, uint value)
        {
            BinaryPrimitives.WriteUInt32BigEndian(writer.Slice(offset, sizeof(uint)), value);
            offset += sizeof(uint);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, ulong value)
        {
            BinaryPrimitives.WriteUInt64BigEndian(writer.Slice(offset, sizeof(ulong)), value);
            offset += sizeof(ulong);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, Half value)
        {
            BinaryPrimitives.WriteHalfBigEndian(writer.Slice(offset, 2), value);
            offset += 2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, float value)
        {
            BinaryPrimitives.WriteSingleBigEndian(writer.Slice(offset, sizeof(float)), value);
            offset += sizeof(float);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this Span<byte> writer, ref int offset, double value)
        {
            BinaryPrimitives.WriteDoubleBigEndian(writer.Slice(offset, sizeof(double)), value);
            offset += sizeof(double);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, short value)
        {
            BinaryPrimitives.WriteInt16LittleEndian(writer.Slice(offset, sizeof(short)), value);
            offset += sizeof(short);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(writer.Slice(offset, sizeof(int)), value);
            offset += sizeof(int);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, long value)
        {
            BinaryPrimitives.WriteInt64LittleEndian(writer.Slice(offset, sizeof(long)), value);
            offset += sizeof(long);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, ushort value)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(writer.Slice(offset, sizeof(ushort)), value);
            offset += sizeof(ushort);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, uint value)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(writer.Slice(offset, sizeof(uint)), value);
            offset += sizeof(uint);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, ulong value)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(writer.Slice(offset, sizeof(ulong)), value);
            offset += sizeof(ulong);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, Half value)
        {
            BinaryPrimitives.WriteHalfLittleEndian(writer.Slice(offset, 2), value);
            offset += 2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, float value)
        {
            BinaryPrimitives.WriteSingleLittleEndian(writer.Slice(offset, sizeof(float)), value);
            offset += sizeof(float);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this Span<byte> writer, ref int offset, double value)
        {
            BinaryPrimitives.WriteDoubleLittleEndian(writer.Slice(offset, sizeof(double)), value);
            offset += sizeof(double);
            Unsafe.ReadUnaligned<Int32>(ref writer[0]);
        }
    }
}