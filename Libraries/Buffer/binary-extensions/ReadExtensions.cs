using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadInt8(this ReadOnlySpan<byte> reader, ref int offset) => (sbyte)reader[offset++];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadUInt8(this ReadOnlySpan<byte> reader, ref int offset) => reader[offset++];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(this ReadOnlySpan<byte> reader, ref int offset) => reader[offset++] != 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt16BigEndian(reader.Slice(offset, sizeof(short)));
            offset += sizeof(short);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt32BigEndian(reader.Slice(offset, sizeof(int)));
            offset += sizeof(int);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt64BigEndian(reader.Slice(offset, sizeof(long)));
            offset += sizeof(long);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt16BigEndian(reader.Slice(offset, sizeof(ushort)));
            offset += sizeof(ushort);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt32BigEndian(reader.Slice(offset, sizeof(uint)));
            offset += sizeof(uint);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt64BigEndian(reader.Slice(offset, sizeof(ulong)));
            offset += sizeof(ulong);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadFloat16BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadHalfBigEndian(reader.Slice(offset, 2));
            offset += 2;
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat32BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadSingleBigEndian(reader.Slice(offset, sizeof(float)));
            offset += sizeof(float);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat64BigEndian(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadDoubleBigEndian(reader.Slice(offset, sizeof(double)));
            offset += sizeof(double);
            return temp;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt16LittleEndian(reader.Slice(offset, sizeof(short)));
            offset += sizeof(short);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt32LittleEndian(reader.Slice(offset, sizeof(int)));
            offset += sizeof(int);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt64LittleEndian(reader.Slice(offset, sizeof(long)));
            offset += sizeof(long);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt16LittleEndian(reader.Slice(offset, sizeof(ushort)));
            offset += sizeof(ushort);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt32LittleEndian(reader.Slice(offset, sizeof(uint)));
            offset += sizeof(uint);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt64LittleEndian(reader.Slice(offset, sizeof(ulong)));
            offset += sizeof(ulong);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadFloat16(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadHalfLittleEndian(reader.Slice(offset, 2));
            offset += 2;
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat32(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadSingleLittleEndian(reader.Slice(offset, sizeof(float)));
            offset += sizeof(float);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat64(this ReadOnlySpan<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadDoubleLittleEndian(reader.Slice(offset, sizeof(double)));
            offset += sizeof(double);
            return temp;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadInt8(this Span<byte> reader, ref int offset) => (sbyte)reader[offset++];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadUInt8(this Span<byte> reader, ref int offset) => reader[offset++];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(this Span<byte> reader, ref int offset) => reader[offset++] != 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt16BigEndian(reader.Slice(offset, sizeof(short)));
            offset += sizeof(short);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt32BigEndian(reader.Slice(offset, sizeof(int)));
            offset += sizeof(int);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt64BigEndian(reader.Slice(offset, sizeof(long)));
            offset += sizeof(long);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt16BigEndian(reader.Slice(offset, sizeof(ushort)));
            offset += sizeof(ushort);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt32BigEndian(reader.Slice(offset, sizeof(uint)));
            offset += sizeof(uint);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt64BigEndian(reader.Slice(offset, sizeof(ulong)));
            offset += sizeof(ulong);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadFloat16BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadHalfBigEndian(reader.Slice(offset, 2));
            offset += 2;
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat32BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadSingleBigEndian(reader.Slice(offset, sizeof(float)));
            offset += sizeof(float);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat64BigEndian(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadDoubleBigEndian(reader.Slice(offset, sizeof(double)));
            offset += sizeof(double);
            return temp;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt16LittleEndian(reader.Slice(offset, sizeof(short)));
            offset += sizeof(short);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt32LittleEndian(reader.Slice(offset, sizeof(int)));
            offset += sizeof(int);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadInt64LittleEndian(reader.Slice(offset, sizeof(long)));
            offset += sizeof(long);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt16LittleEndian(reader.Slice(offset, sizeof(ushort)));
            offset += sizeof(ushort);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt32LittleEndian(reader.Slice(offset, sizeof(uint)));
            offset += sizeof(uint);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadUInt64LittleEndian(reader.Slice(offset, sizeof(ulong)));
            offset += sizeof(ulong);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadFloat16(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadHalfLittleEndian(reader.Slice(offset, 2));
            offset += 2;
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat32(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadSingleLittleEndian(reader.Slice(offset, sizeof(float)));
            offset += sizeof(float);
            return temp;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat64(this Span<byte> reader, ref int offset)
        {
            var temp = BinaryPrimitives.ReadDoubleLittleEndian(reader.Slice(offset, sizeof(double)));
            offset += sizeof(double);
            return temp;
        }
    }
}
