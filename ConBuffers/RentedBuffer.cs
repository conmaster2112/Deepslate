using System.Buffers;
using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public struct RentedBuffer: IDisposable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RentedBuffer Alloc(int size) => new(
                ArrayPool<byte>.Shared.Rent(size),
                size
            );
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RentedBuffer From(ReadOnlySpan<byte> buffer)
        {
            RentedBuffer rented = Alloc(buffer.Length);
            buffer.CopyTo(rented);
            return rented;
        }
        private byte[] _buffer;
        private int _length;
        public readonly int Length => _length;
        public readonly Span<byte> Span => _buffer.AsSpan(0, _length);
        public readonly Memory<byte> Memory => _buffer.AsMemory(0, _length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RentedBuffer(byte[] buffer, int size)
        {
            _buffer = buffer;
            _length = size;
        }
        public void Return()
        {
            if (_buffer == null) return;
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null!;
            _length = default;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<byte> buffer) => Span.CopyTo(buffer);
        public void Dispose() => Return();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Span<byte>(RentedBuffer buffer) => buffer.Span;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<byte>(RentedBuffer buffer) => buffer.Span;
        public static void SetLength(ref RentedBuffer buffer, int newLength)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(newLength, buffer._buffer.Length, nameof(newLength));
            buffer._length = newLength;
        }
    }
}
