using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Buffers
{
    [Obsolete]
    public class BufferSource : IBufferReader, IBufferWriter
    {
        public static BufferSource UnsafeAlloc(int length) => GC.AllocateUninitializedArray<byte>(length);
        public static implicit operator Memory<byte>(BufferSource bufferSource) => bufferSource.AsMemory();
        public static implicit operator Span<byte>(BufferSource bufferSource) => bufferSource.AsSpan();
        public static implicit operator ReadOnlyMemory<byte>(BufferSource bufferSource) => bufferSource.AsMemory();
        public static implicit operator ReadOnlySpan<byte>(BufferSource bufferSource) => bufferSource.AsSpan();
        public static implicit operator BufferSource(byte[] buffer)=>new(buffer);

        public BufferSource(byte[] buffer, int length)
        {
            Buffer = buffer;
            _length = length;
        }
        public BufferSource(byte[] buffer) : this(buffer, buffer.Length) { }
        public byte[] Buffer;
        internal int _position = 0;
        internal int _length;

        public int Position
        {
            get => _position;
            set
            {
                if(value < 0) throw new ArgumentOutOfRangeException(nameof(value),"Position can not be negative.");
                if(value > _length) throw new ArgumentOutOfRangeException(nameof(value), "Position can not be higher than length of buffer.");
                _position = value;
            }
        }
        public  int Length => _length;
        public bool IsEndOfBuffer => _position >= _length;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Span<byte> AsSpan() => new(Buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Span<byte> AsSpan(int start) => new(Buffer, start, _length - start);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Span<byte> AsSpan(int start, int length) => new(Buffer, start, length);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Memory<byte> AsMemory() => new(Buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> AsMemory(int start) => new(Buffer, start, _length - start);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Memory<byte> AsMemory(int start, int length) => new(Buffer, start, length);




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt8(byte value)
        {
            RangeCheck();
            Buffer[_position++] = value;
            return 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt8(sbyte value)
        {
            RangeCheck();
            Buffer[_position++] = unchecked((byte)value);
            return 1;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt16LittleEndian(ushort value)
        {
            RangeCheck(sizeof(ushort));
            BinaryPrimitives.WriteUInt16LittleEndian(GetSlice(sizeof(ushort)), value);
            _position += sizeof(ushort);
            return sizeof(ushort);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt16BigEndian(ushort value)
        {
            RangeCheck(sizeof(ushort));
            BinaryPrimitives.WriteUInt16BigEndian(GetSlice(sizeof(ushort)), value);
            _position += sizeof(ushort);
            return sizeof(ushort);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt16LittleEndian(short value)
        {
            RangeCheck(sizeof(short));
            BinaryPrimitives.WriteInt16LittleEndian(GetSlice(sizeof(short)), value);
            _position += sizeof(short);
            return sizeof(short);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt16BigEndian(short value)
        {
            RangeCheck(sizeof(short));
            BinaryPrimitives.WriteInt16BigEndian(GetSlice(sizeof(short)), value);
            _position += sizeof(short);
            return sizeof(short);
        }






        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt32LittleEndian(uint value)
        {
            RangeCheck(sizeof(uint));
            BinaryPrimitives.WriteUInt32LittleEndian(GetSlice(sizeof(uint)), value);
            _position += sizeof(uint);
            return sizeof(uint);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt32BigEndian(uint value)
        {
            RangeCheck(sizeof(uint));
            BinaryPrimitives.WriteUInt32BigEndian(GetSlice(sizeof(uint)), value);
            _position += sizeof(uint);
            return sizeof(uint);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt32LittleEndian(int value)
        {
            RangeCheck(sizeof(int));
            BinaryPrimitives.WriteInt32LittleEndian(GetSlice(sizeof(int)), value);
            _position += sizeof(int);
            return sizeof(int);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt32BigEndian(int value)
        {
            RangeCheck(sizeof(int));
            BinaryPrimitives.WriteInt32BigEndian(GetSlice(sizeof(int)), value);
            //BinaryPrimitives.WriteInt32LittleEndian(GetSlice(sizeof(int)), value);
            _position += sizeof(int);
            return sizeof(int);
        }








        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt64LittleEndian(ulong value)
        {
            RangeCheck(sizeof(ulong));
            BinaryPrimitives.WriteUInt64LittleEndian(GetSlice(sizeof(ulong)), value);
            _position += sizeof(ulong);
            return sizeof(ulong);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt64BigEndian(ulong value)
        {
            RangeCheck(sizeof(ulong));
            BinaryPrimitives.WriteUInt64BigEndian(GetSlice(sizeof(ulong)), value);
            _position += sizeof(ulong);
            return sizeof(ulong);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt64LittleEndian(long value)
        {
            RangeCheck(sizeof(long));
            BinaryPrimitives.WriteInt64LittleEndian(GetSlice(sizeof(long)), value);
            
            _position += sizeof(long);
            return sizeof(long);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteInt64BigEndian(long value)
        {
            RangeCheck(sizeof(long));
            BinaryPrimitives.WriteInt64BigEndian(GetSlice(sizeof(long)), value);
            _position += sizeof(long);
            return sizeof(long);
        }






        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteFloatLittleEndian(float value)
        {
            RangeCheck(sizeof(float));
            BinaryPrimitives.WriteSingleLittleEndian(GetSlice(sizeof(float)), value);
            _position += sizeof(float);
            return sizeof(float);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteFloatBigEndian(float value)
        {
            RangeCheck(sizeof(float));
            BinaryPrimitives.WriteSingleBigEndian(GetSlice(sizeof(float)), value);
            _position += sizeof(float);
            return sizeof(float);
        }





        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteFloat64LittleEndian(double value)
        {
            RangeCheck(sizeof(double));
            BinaryPrimitives.WriteDoubleLittleEndian(GetSlice(sizeof(double)), value);
            _position += sizeof(double);
            return sizeof(double);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteFloat64BigEndian(double value)
        {
            RangeCheck(sizeof(double));
            BinaryPrimitives.WriteDoubleBigEndian(GetSlice(sizeof(double)), value);
            _position += sizeof(double);
            return sizeof(double);
        }




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt24LittleEndian(int value)
        {
            RangeCheck(3);
            Buffer[_position + 0] = (byte)value;
            Buffer[_position + 1] = (byte)(value >> 8);
            Buffer[_position + 2] = (byte)(value >> 16);
            _position += 3;
            return 3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteUInt24BigEndian(int value)
        {
            RangeCheck(3);
            Buffer[_position + 2] = (byte)value;
            Buffer[_position + 1] = (byte)(value >> 8);
            Buffer[_position + 0] = (byte)(value >> 16);
            _position += 3;
            return 3;
        }




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteBoolean(bool value)
        {
            RangeCheck();
            Buffer[_position++] = (byte)(value ? 1 : 0);
            return 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteBytes(byte[] value, int start, int length) => WriteBytes(value.AsSpan(start, length));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteBytes(ReadOnlySpan<byte> value)
        {
            int length = value.Length;
            RangeCheck(length);
            value.CopyTo(GetSlice(length));
            _position += length;
            return length;
        }





        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadUInt8()
        {
            RangeCheck();
            return Buffer[_position++];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadInt8()
        {
            RangeCheck();
            return unchecked((sbyte)Buffer[_position++]);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16LittleEndian()
        {
            RangeCheck(sizeof(ushort));
            ushort value = BinaryPrimitives.ReadUInt16LittleEndian(GetSlice(sizeof(ushort)));
            _position += sizeof(ushort);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16BigEndian()
        {
            RangeCheck(sizeof(ushort));
            ushort value = BinaryPrimitives.ReadUInt16BigEndian(GetSlice(sizeof(ushort)));
            _position += sizeof(ushort);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16LittleEndian()
        {
            
            RangeCheck(sizeof(short));
            short value = BinaryPrimitives.ReadInt16LittleEndian(GetSlice(sizeof(short)));
            _position += sizeof(short);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16BigEndian()
        {

            RangeCheck(sizeof(short));
            short value = BinaryPrimitives.ReadInt16BigEndian(GetSlice(sizeof(short)));
            _position += sizeof(short);
            return value;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32LittleEndian()
        {
            RangeCheck(sizeof(uint));
            uint value = BinaryPrimitives.ReadUInt32LittleEndian(GetSlice(sizeof(uint)));
            _position += sizeof(uint);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32BigEndian()
        {
            RangeCheck(sizeof(uint));
            uint value = BinaryPrimitives.ReadUInt32BigEndian(GetSlice(sizeof(uint)));
            _position += sizeof(uint);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32LittleEndian()
        {
            RangeCheck(sizeof(int));
            int value = BinaryPrimitives.ReadInt32LittleEndian(GetSlice(sizeof(int)));
            _position += sizeof(int);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32BigEndian()
        {
            RangeCheck(sizeof(int));
            int value = BinaryPrimitives.ReadInt32BigEndian(GetSlice(sizeof(int)));
            _position += sizeof(int);
            return value;
        }






        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64LittleEndian()
        {
            RangeCheck(sizeof(ulong));
            ulong value = BinaryPrimitives.ReadUInt64LittleEndian(GetSlice(sizeof(ulong)));
            _position += sizeof(ulong);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64BigEndian()
        {
            RangeCheck(sizeof(ulong));
            ulong value = BinaryPrimitives.ReadUInt64BigEndian(GetSlice(sizeof(ulong)));
            _position += sizeof(ulong);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64LittleEndian()
        {
            RangeCheck(sizeof(long));
            long value = BinaryPrimitives.ReadInt64LittleEndian(GetSlice(sizeof(long)));
            _position += sizeof(long);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64BigEndian()
        {
            RangeCheck(sizeof(long));
            long value = BinaryPrimitives.ReadInt64BigEndian(GetSlice(sizeof(long)));
            _position += sizeof(long);
            return value;
        }




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloatLittleEndian()
        {
            RangeCheck(sizeof(float));
            float value = BinaryPrimitives.ReadSingleLittleEndian(GetSlice(sizeof(float)));
            _position += sizeof(float);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloatBigEndian()
        {
            RangeCheck(sizeof(float));
            float value = BinaryPrimitives.ReadSingleBigEndian(GetSlice(sizeof(float)));
            _position += sizeof(float);
            return value;
        }




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadFloat64LittleEndian()
        {
            RangeCheck(sizeof(double));
            double value = BinaryPrimitives.ReadDoubleLittleEndian(GetSlice(sizeof(double)));
            _position += sizeof(double);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadFloat64BigEndian()
        {
            RangeCheck(sizeof(double));
            double value = BinaryPrimitives.ReadDoubleBigEndian(GetSlice(sizeof(double)));
            _position += sizeof(double);
            return value;
        }




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadUInt24LittleEndian()
        {
            RangeCheck(3);
            int value = 0;
            value |= Buffer[_position + 0];
            value |= Buffer[_position + 1] << 8;
            value |= Buffer[_position + 2] << 16;
            _position += 3;
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadUInt24BigEndian()
        {
            RangeCheck(3);
            int value = 0;
            value |= Buffer[_position + 2];
            value |= Buffer[_position + 1] << 8;
            value |= Buffer[_position + 0] << 16;
            _position += 3;
            return value;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBoolean()
        {
            RangeCheck();
            return Buffer[_position++] != 0;
        }




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> ReadBytes(int length)
        {
            RangeCheck(length);
            Span<byte> data = GetSlice(length);
            _position += length;
            return data;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlySpan<byte> IBufferReader.ReadBytes(int length) => ReadBytes(length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> ReadMemory(int length)
        {
            RangeCheck(length);
            Memory<byte> data = Buffer.AsMemory(_position, length);
            _position += length;
            return data;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyMemory<byte> IBufferReader.ReadMemory(int length) => ReadMemory(length);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(int count) => _position += count;



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Span<byte> GetSlice(int length) => new(Buffer, _position, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Span<byte> GetSlice() => new(Buffer, _position, _length - _position);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlySpan<byte> IBufferReader.GetSlice(int length) => new(Buffer, _position, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlySpan<byte> IBufferReader.GetSlice() => new(Buffer, _position,_length - _position);



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetProccessedBytesSpan() => AsSpan(0, Position);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetProccessedBytesMemory() => AsMemory(0, Position);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlySpan<byte> IBufferReader.GetProccessedBytesSpan() => AsSpan(0, Position);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyMemory<byte> IBufferReader.GetProccessedBytesMemory() => AsMemory(0, Position);





        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RangeCheck(int size = 1) { if (_position + size > Length) throw new EndOfStreamException(); }
    }
}
