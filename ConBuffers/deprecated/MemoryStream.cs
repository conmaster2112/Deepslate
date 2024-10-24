using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace ConMaster.Buffers.deprecated
{
    [Obsolete]
    public class MemoryBufferStream : Stream
    {
        public byte this[int index]
        {
            get => _buffer[index];
            set => _buffer[index] = value;
        }
        public const int MAX_MEMORY_SIZE = int.MaxValue;
        protected readonly byte[] _buffer;
        protected long _offset;
        protected long _length;
        public MemoryBufferStream(byte[] buffer, long offset = 0, long length = default)
        {
            if (length == default) length = buffer.Length;
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length, nameof(length));
            if (length > buffer.Length) throw new ArgumentOutOfRangeException(nameof(length), "Specified Length must be less than length of buffer");
            _buffer = buffer;
            _offset = offset;
            _length = length;
        }
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length { get => _length; }
        public bool EndOfStream => _offset >= Length;

        public override long Position
        {
            get => _offset;
            set
            {
                if (value > Length) throw new ArgumentOutOfRangeException(nameof(value), value, "Can not set position in out of buffer memory range");
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "Position must be positive or zero only.");
                _offset = value;
            }
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ValidateBufferArguments(buffer, offset, count);
            if (EndOfStream) return 0;
            if (offset + count > Length) count = (int)Length - offset;
            if (count > 8) Buffer.BlockCopy(_buffer, (int)_offset, buffer, offset, count);
            else
            {
                int byteCount = count;
                while (--byteCount >= 0) buffer[offset + byteCount] = _buffer[_offset + byteCount];
            }
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin = SeekOrigin.Current)
        {
            if (offset > MAX_MEMORY_SIZE) throw new ArgumentOutOfRangeException(nameof(offset), "Maximum memory size ");
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0) throw new IOException("Seek before begin");
                    Position = offset;
                    return _offset;
                case SeekOrigin.Current:
                    if (Position + offset < 0) throw new IOException("Seek before begin");
                    Position += offset;
                    return Position;
                case SeekOrigin.End:
                    if (Length + offset < 0) throw new IOException("Seek before begin");
                    Position = Length + offset;
                    return Position;
            }
            return Position;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> GetSlice(int offset, int? count = null) => count == null ? InternalGetSlice(offset) : InternalGetSlice(offset, count ?? 0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Span<byte> InternalGetSlice(int offset, int count) => _buffer.AsSpan(offset, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Span<byte> InternalGetSlice(int offset) => _buffer.AsSpan(offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Span<byte> InternalGetRange(int size) => _buffer.AsSpan((int)_offset, size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetLength(long value)
        {
            if (value > _buffer.Length) throw new ArgumentOutOfRangeException(nameof(value));
            _length = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Write(byte[] buffer, int offset, int count) => Write(buffer.AsSpan(offset, count));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Write(ReadOnlySpan<byte> buffer) => buffer.CopyTo(_buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteBuffer(ReadOnlySpan<byte> buffer)
        {
            buffer.CopyTo(_buffer.AsSpan((int)_offset));
            _offset += buffer.Length;
            return buffer.Length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> ReadBufferRaw(int count) => _buffer.AsSpan((int)_offset, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> ReadBuffer(int count)
        {
            Span<byte> o = _buffer.AsSpan((int)_offset, count);
            _offset += o.Length;
            return o;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> ReadMemory(int count)
        {
            Memory<byte> o = _buffer.AsMemory((int)_offset, count);
            _offset += o.Length;
            return o;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBuffer() => _buffer;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public Memory<byte> GetMemory() => _buffer.AsMemory((int)_offset, (int)_length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetWritten() => _buffer.AsMemory(0, (int)_offset);
    }
}
