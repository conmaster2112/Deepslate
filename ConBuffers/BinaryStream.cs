using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace ConMaster.Buffers
{
    public class BinaryStream : Stream
    {
        public new static readonly BinaryStream Null = new();
        protected Stream _stream;
        protected bool _leaveOpen;
        protected bool _isMemoryStream;
        private BinaryStream()
        {
            _stream = Stream.Null;
        }
        public BinaryStream(Stream stream) : this(stream, false) { }
        public BinaryStream(Stream stream, bool leaveOpen)
        {
            _stream = stream;
            _leaveOpen = leaveOpen;
            _isMemoryStream = (stream.GetType() == typeof(MemoryStream));
        }
        public Stream BaseStream
        {
            get
            {
                _stream.Flush();
                return _stream;
            }
        }
        public bool IsEndOfStream => Position >= Length;
        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
        public override int Read(Span<byte> buffer) => _stream.Read(buffer);

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

        public override void SetLength(long value) => _stream.SetLength(value);
        public override void Write(ReadOnlySpan<byte> buffer) => _stream.Write(buffer);
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
        public override void Close() => Dispose(true);
        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_leaveOpen) Flush();
                else _stream.Close();
            }
        }
        public new void Dispose() => Dispose(true);
        public virtual void Write(byte value) => _stream.WriteByte(value);
        public virtual void Write(sbyte value) => _stream.WriteByte((byte)value);

        public virtual Span<byte> LoadSpan(Span<byte> bytes)
        {
            _stream.ReadExactly(bytes);
            return bytes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(short value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(short)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(int value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(int)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(long value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(long)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(ushort value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(ushort)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(uint value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(uint)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(ulong value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(ulong)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(Half value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[2];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(float value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[2];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WriteBigEndian(double value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(double)];
            bytes.WriteBigEndian(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(short value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(short)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(int value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(int)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(long value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(long)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(ushort value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(ushort)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(uint value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(uint)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(ulong value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(ulong)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(Half value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[2];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(float value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(float)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Write(double value)
        {
            int offset = 0;
            Span<byte> bytes = stackalloc byte[sizeof(double)];
            bytes.Write(ref offset, value);
            _stream.Write(bytes);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual byte ReadUInt8()
        {
            int data = ReadByte();
            if (data == -1) throw new EndOfStreamException();
            return (byte)data;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual sbyte ReadInt8() => (sbyte)ReadUInt8();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual short ReadInt16BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(short)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadInt16BigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int ReadInt32BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(int)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadInt32BigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual long ReadInt64BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(long)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadInt64BigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ushort ReadUInt16BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(ushort)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadUInt16BigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual uint ReadUInt32BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(uint)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ulong ReadUInt64BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(ulong)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadUInt64BigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Half ReadHalfBigEndian()
        {
            Span<byte> buffer = stackalloc byte[2];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadHalfBigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual float ReadFloatBigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(float)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadSingleBigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double ReadFloat64BigEndian()
        {
            Span<byte> buffer = stackalloc byte[sizeof(double)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadDoubleBigEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual short ReadInt16()
        {
            Span<byte> buffer = stackalloc byte[sizeof(short)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadInt16LittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int ReadInt32()
        {
            Span<byte> buffer = stackalloc byte[sizeof(int)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadInt32LittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual long ReadInt64()
        {
            Span<byte> buffer = stackalloc byte[sizeof(long)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadInt64LittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ushort ReadUInt16()
        {
            Span<byte> buffer = stackalloc byte[sizeof(ushort)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual uint ReadUInt32()
        {
            Span<byte> buffer = stackalloc byte[sizeof(uint)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ulong ReadUInt64()
        {
            Span<byte> buffer = stackalloc byte[sizeof(ulong)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadUInt64LittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Half ReadHalf()
        {
            Span<byte> buffer = stackalloc byte[2];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadHalfLittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual float ReadFloat32()
        {
            Span<byte> buffer = stackalloc byte[sizeof(float)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadSingleLittleEndian(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double ReadFloat64()
        {
            Span<byte> buffer = stackalloc byte[sizeof(double)];
            _stream.ReadExactly(buffer);
            return BinaryPrimitives.ReadDoubleLittleEndian(buffer);
        }
    }
}
