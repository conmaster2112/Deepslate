using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, sbyte value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, byte value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, bool value) => writer.Span.Write(ref writer.Offset, (byte)(value?1:0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, short value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, int value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, long value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, ushort value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, uint value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, ulong value) => writer.Span.WriteBigEndian(ref writer.Offset, value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, Half value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, float value) => writer.Span.WriteBigEndian(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian(this ConstantMemoryBufferWriter writer, double value) => writer.Span.WriteBigEndian(ref writer.Offset, value);



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, short value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, int value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, long value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, ushort value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, uint value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, ulong value) => writer.Span.Write(ref writer.Offset, value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, Half value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, float value) => writer.Span.Write(ref writer.Offset, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ConstantMemoryBufferWriter writer, double value) => writer.Span.Write(ref writer.Offset, value);
    }
}
