using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Buffers
{
    public readonly ref struct ConstantMemoryBufferWriter(Span<byte> data, ref int offset)
    {
        public readonly ref int Offset = ref offset;
        public readonly Span<byte> Span = data;
        public bool IsEndOfStream => Offset >= Length;
        public readonly int Length => Span.Length;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Span<byte> ReadSlice(int length)
        {
            int offset = Offset;
            Offset = offset + length;
            return Span.Slice(offset, length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(ReadOnlySpan<byte> slice)
        {
            slice.CopyTo(Span.Slice(Offset,slice.Length));
            Offset += slice.Length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void SetOffset(int offset) => Offset = offset;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Span<byte> PeekSlice(int length) => Span.Slice(Offset, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Span<byte> PeekFull() => Span.Slice(Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<byte> GetWritenBytes() => Span.Slice(0, Offset);
    }
}
