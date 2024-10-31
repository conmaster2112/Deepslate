using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Buffers
{
    public readonly ref struct ConstantMemoryBufferReader(ReadOnlySpan<byte> data, ref int offset)
    {
        public readonly ref int Offset = ref offset;
        public bool IsEndOfStream => Offset >= Length;
        public readonly ReadOnlySpan<byte> Span = data;
        public readonly int Length => Span.Length;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<byte> ReadSlice(int length)
        {
            int offset = Offset;
            Offset = offset + length;
            return Span.Slice(offset, length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void SetOffset(int offset) => Offset = offset;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<byte> PeekSlice(int length) => Span.Slice(Offset, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<byte> PeekFull() => Span.Slice(Offset);
    }
}
