using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConMaster.Buffers.deprecated
{
    [Obsolete]
    public class BinaryStream(byte[] buffer, int offset = 0, int length = default) : MemoryBufferStream(buffer, offset, length)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt8(byte value, bool useLittleEndian = true)
        {
            CanWriteCheck();
            _buffer[_offset++] = value;
            return 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt8(sbyte value, bool useLittleEndian = true)
        {
            CanWriteCheck();
            _buffer[_offset++] = unchecked((byte)value);
            return 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt16(ushort value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(ushort));
            if (useLittleEndian) BinaryPrimitives.WriteUInt16LittleEndian(InternalGetRange(sizeof(ushort)), value);
            else BinaryPrimitives.WriteUInt16BigEndian(InternalGetRange(sizeof(ushort)), value);
            _offset += sizeof(ushort);
            return sizeof(ushort);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt16(short value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(short));
            if (useLittleEndian) BinaryPrimitives.WriteInt16LittleEndian(InternalGetRange(sizeof(short)), value);
            else BinaryPrimitives.WriteInt16BigEndian(InternalGetRange(sizeof(short)), value);
            _offset += sizeof(short);
            return sizeof(short);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt32(uint value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(uint));
            if (useLittleEndian) BinaryPrimitives.WriteUInt32LittleEndian(InternalGetRange(sizeof(uint)), value);
            else BinaryPrimitives.WriteUInt32BigEndian(InternalGetRange(sizeof(uint)), value);
            _offset += sizeof(uint);
            return sizeof(uint);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt32(int value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(int));
            if (useLittleEndian) BinaryPrimitives.WriteInt32LittleEndian(InternalGetRange(sizeof(int)), value);
            else BinaryPrimitives.WriteInt32BigEndian(InternalGetRange(sizeof(int)), value);
            _offset += sizeof(int);
            return sizeof(int);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt64(ulong value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(ulong));
            if (useLittleEndian) BinaryPrimitives.WriteUInt64LittleEndian(InternalGetRange(sizeof(ulong)), value);
            else BinaryPrimitives.WriteUInt64BigEndian(InternalGetRange(sizeof(ulong)), value);
            _offset += sizeof(ulong);
            return sizeof(ulong);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt64(long value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(long));
            if (useLittleEndian) BinaryPrimitives.WriteInt64LittleEndian(InternalGetRange(sizeof(long)), value);
            else BinaryPrimitives.WriteInt64BigEndian(InternalGetRange(sizeof(long)), value);
            _offset += sizeof(long);
            return sizeof(long);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteFloat(float value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(float));
            if (useLittleEndian) BinaryPrimitives.WriteSingleLittleEndian(InternalGetRange(sizeof(float)), value);
            else BinaryPrimitives.WriteSingleBigEndian(InternalGetRange(sizeof(float)), value);
            _offset += sizeof(float);
            return sizeof(float);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteDoable(double value, bool useLittleEndian = true)
        {
            CanWriteCheck(sizeof(double));
            if (useLittleEndian) BinaryPrimitives.WriteDoubleLittleEndian(InternalGetRange(sizeof(double)), value);
            else BinaryPrimitives.WriteDoubleBigEndian(InternalGetRange(sizeof(double)), value);
            _offset += sizeof(double);
            return sizeof(double);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt24(int value, bool useLittleEndian = true)
        {
            CanWriteCheck(3);
            if (useLittleEndian)
            {
                _buffer[_offset + 0] = (byte)value;
                _buffer[_offset + 1] = (byte)(value >> 8);
                _buffer[_offset + 2] = (byte)(value >> 16);
            }
            else
            {
                _buffer[_offset + 2] = (byte)value;
                _buffer[_offset + 1] = (byte)(value >> 8);
                _buffer[_offset + 0] = (byte)(value >> 16);
            }
            _offset += 3;
            return 3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteBoolean(bool value, bool useLittleEndian = true)
        {
            CanWriteCheck();
            _buffer[_offset++] = (byte)(value ? 1 : 0);
            return 1;
        }
        public int WriteStruct<T>(T value, int sizeOf) where T : struct
        {
            CanWriteCheck(sizeOf);
            if (MemoryMarshal.TryWrite(InternalGetRange(sizeOf), value))
            {
                _offset += sizeOf;
                return sizeOf;
            }
            return 0;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadUInt8(bool useLittleEndian = true)
        {
            RangeCheck();
            return _buffer[_offset++];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadInt8(bool useLittleEndian = true)
        {
            RangeCheck();
            return unchecked((sbyte)_buffer[_offset++]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16(bool useLittleEndian = true)
        {
            ushort value;
            RangeCheck(sizeof(ushort));
            if (useLittleEndian) value = BinaryPrimitives.ReadUInt16LittleEndian(InternalGetRange(sizeof(ushort)));
            else value = BinaryPrimitives.ReadUInt16BigEndian(InternalGetRange(sizeof(ushort)));
            _offset += sizeof(ushort);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16(bool useLittleEndian = true)
        {
            short value;
            RangeCheck(sizeof(short));
            if (useLittleEndian) value = BinaryPrimitives.ReadInt16LittleEndian(InternalGetRange(sizeof(short)));
            else value = BinaryPrimitives.ReadInt16BigEndian(InternalGetRange(sizeof(short)));
            _offset += sizeof(short);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32(bool useLittleEndian = true)
        {
            uint value;
            RangeCheck(sizeof(uint));
            if (useLittleEndian) value = BinaryPrimitives.ReadUInt32LittleEndian(InternalGetRange(sizeof(uint)));
            else value = BinaryPrimitives.ReadUInt32BigEndian(InternalGetRange(sizeof(uint)));
            _offset += sizeof(uint);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32(bool useLittleEndian = true)
        {
            int value;
            RangeCheck(sizeof(int));
            if (useLittleEndian) value = BinaryPrimitives.ReadInt32LittleEndian(InternalGetRange(sizeof(int)));
            else value = BinaryPrimitives.ReadInt32BigEndian(InternalGetRange(sizeof(int)));
            _offset += sizeof(int);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64(bool useLittleEndian = true)
        {
            ulong value;
            RangeCheck(sizeof(ulong));
            if (useLittleEndian) value = BinaryPrimitives.ReadUInt64LittleEndian(InternalGetRange(sizeof(ulong)));
            else value = BinaryPrimitives.ReadUInt64BigEndian(InternalGetRange(sizeof(ulong)));
            _offset += sizeof(ulong);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64(bool useLittleEndian = true)
        {
            long value;
            RangeCheck(sizeof(long));
            if (useLittleEndian) value = BinaryPrimitives.ReadInt64LittleEndian(InternalGetRange(sizeof(long)));
            else value = BinaryPrimitives.ReadInt64BigEndian(InternalGetRange(sizeof(long)));
            _offset += sizeof(long);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(bool useLittleEndian = true)
        {
            float value;
            RangeCheck(sizeof(float));
            if (useLittleEndian) value = BinaryPrimitives.ReadSingleLittleEndian(InternalGetRange(sizeof(float)));
            else value = BinaryPrimitives.ReadSingleBigEndian(InternalGetRange(sizeof(float)));
            _offset += sizeof(float);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDoable(bool useLittleEndian = true)
        {
            double value;
            RangeCheck(sizeof(double));
            if (useLittleEndian) value = BinaryPrimitives.ReadDoubleLittleEndian(InternalGetRange(sizeof(double)));
            else value = BinaryPrimitives.ReadDoubleBigEndian(InternalGetRange(sizeof(double)));
            _offset += sizeof(double);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadUInt24(bool useLittleEndian = true)
        {
            RangeCheck(3);
            int value = 0;
            if (useLittleEndian)
            {
                value |= _buffer[_offset + 0];
                value |= _buffer[_offset + 1] << 8;
                value |= _buffer[_offset + 2] << 16;
            }
            else
            {
                value |= _buffer[_offset + 2];
                value |= _buffer[_offset + 1] << 8;
                value |= _buffer[_offset + 0] << 16;
            }
            _offset += 3;
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBoolean(bool useLittleEndian = true)
        {
            RangeCheck();
            return _buffer[_offset++] > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CanWriteCheck(int size = 1) { if (_offset + size > Length) throw new EndOfStreamException(); }
        protected void RangeCheck(int size = 1) { if (_offset + size > Length) throw new EndOfStreamException(); }
    }
}
