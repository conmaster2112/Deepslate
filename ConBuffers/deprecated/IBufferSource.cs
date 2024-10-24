using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Buffers
{
    [Obsolete]
    public interface IBufferSource
    {
        public Span<byte> SourceSpan { get; }
        public int Offset { get; set; }
    }
    [Obsolete]
    public interface IBufferReadOnlySource
    {
        public ReadOnlySpan<byte> SourceSpan { get; }
        public int Offset { get; set; }
    }
    [Obsolete]
    public interface IBufferSerializer
    {
        public void Skip(int count);
        public bool IsEndOfBuffer { get; }
        public int Length { get; }
        public int Position { get; }
    }
    [Obsolete]
    public interface IBufferWriter: IBufferSerializer
    {
        public Span<byte> GetSlice(int length);
        public Span<byte> GetSlice();
        public Span<byte> GetProccessedBytesSpan();
        public Memory<byte> GetProccessedBytesMemory();
        public int WriteUInt8(byte value);
        public int WriteInt8(sbyte value);
        public int WriteBoolean(bool value);

        public int WriteUInt16LittleEndian(ushort value);
        public int WriteInt16LittleEndian(short value);
        public int WriteUInt32LittleEndian(uint value);
        public int WriteInt32LittleEndian(int value);
        public int WriteUInt64LittleEndian(ulong value);
        public int WriteInt64LittleEndian(long value);
        public int WriteFloatLittleEndian(float value);
        public int WriteFloat64LittleEndian(double value);
        public int WriteUInt24LittleEndian(int value);

        public int WriteUInt16BigEndian(ushort value);
        public int WriteInt16BigEndian(short value);
        public int WriteUInt32BigEndian(uint value);
        public int WriteInt32BigEndian(int value);
        public int WriteUInt64BigEndian(ulong value);
        public int WriteInt64BigEndian(long value);
        public int WriteFloatBigEndian(float value);
        public int WriteFloat64BigEndian(double value);
        public int WriteUInt24BigEndian(int value);


        public int WriteBytes(byte[] value, int start, int length);
        public int WriteBytes(ReadOnlySpan<byte> value);
    }
    [Obsolete]
    public interface IBufferReader: IBufferSerializer
    {
        public ReadOnlySpan<byte> GetSlice(int length);
        public ReadOnlySpan<byte> GetSlice();
        public ReadOnlySpan<byte> GetProccessedBytesSpan();
        public ReadOnlyMemory<byte> GetProccessedBytesMemory();
        public byte ReadUInt8();
        public sbyte ReadInt8();
        public bool ReadBoolean();
        public ushort ReadUInt16BigEndian();
        public short ReadInt16BigEndian();
        public uint ReadUInt32BigEndian();
        public int ReadInt32BigEndian();
        public ulong ReadUInt64BigEndian();
        public long ReadInt64BigEndian();
        public float ReadFloatBigEndian();
        public double ReadFloat64BigEndian();
        public int ReadUInt24BigEndian();

        public ushort ReadUInt16LittleEndian();
        public short ReadInt16LittleEndian();
        public uint ReadUInt32LittleEndian();
        public int ReadInt32LittleEndian();
        public ulong ReadUInt64LittleEndian();
        public long ReadInt64LittleEndian();
        public float ReadFloatLittleEndian();
        public double ReadFloat64LittleEndian();
        public int ReadUInt24LittleEndian();
        public ReadOnlySpan<byte> ReadBytes(int length);
        public ReadOnlyMemory<byte> ReadMemory(int length);
    }
}
