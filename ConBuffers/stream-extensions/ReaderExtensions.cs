using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadInt8(this ConstantMemoryBufferReader reader) => reader.Span.ReadInt8(ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadUInt8(this ConstantMemoryBufferReader reader) => reader.Span[reader.Offset++];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(this ConstantMemoryBufferReader reader) => reader.Span[reader.Offset++] != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16BigEndian(this ConstantMemoryBufferReader reader) => ReadInt16BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32BigEndian(this ConstantMemoryBufferReader reader) => ReadInt32BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64BigEndian(this ConstantMemoryBufferReader reader) => ReadInt64BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16BigEndian(this ConstantMemoryBufferReader reader) => ReadUInt16BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32BigEndian(this ConstantMemoryBufferReader reader) => ReadUInt32BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64BigEndian(this ConstantMemoryBufferReader reader) => ReadUInt64BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadFloat16BigEndian(this ConstantMemoryBufferReader reader) => ReadFloat16BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat32BigEndian(this ConstantMemoryBufferReader reader) => ReadFloat32BigEndian(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat64BigEndian(this ConstantMemoryBufferReader reader) => ReadFloat64BigEndian(reader.Span, ref reader.Offset);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16(this ConstantMemoryBufferReader reader) => ReadInt16(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32(this ConstantMemoryBufferReader reader) => ReadInt32(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64(this ConstantMemoryBufferReader reader) => ReadInt64(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16(this ConstantMemoryBufferReader reader) => ReadUInt16(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(this ConstantMemoryBufferReader reader) => ReadUInt32(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64(this ConstantMemoryBufferReader reader) => ReadUInt64(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ReadFloat16(this ConstantMemoryBufferReader reader) => ReadFloat16(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat32(this ConstantMemoryBufferReader reader) => ReadFloat32(reader.Span, ref reader.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat64(this ConstantMemoryBufferReader reader) => ReadFloat64(reader.Span, ref reader.Offset);
    }
}
