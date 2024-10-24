using ConMaster.Buffers;
using System;
namespace ConMaster.Deepslate.NBT
{
    internal class JavaNBTMode : NBTMode
    {
        public override int ReadArraySize(ConstantMemoryBufferReader reader) => reader.ReadInt32BigEndian();
        public override int ReadStringSize(ConstantMemoryBufferReader reader) => reader.ReadInt32BigEndian();
        public override byte ReadByte(ConstantMemoryBufferReader reader) => reader.ReadUInt8();
        public override float ReadFloat32(ConstantMemoryBufferReader reader) => reader.ReadFloat32BigEndian();
        public override double ReadFloat64(ConstantMemoryBufferReader reader) => reader.ReadFloat64BigEndian();
        public override int ReadInt32(ConstantMemoryBufferReader reader) => reader.ReadInt32BigEndian();
        public override short ReadInt16(ConstantMemoryBufferReader reader) => reader.ReadInt16BigEndian();
        public override long ReadInt64(ConstantMemoryBufferReader reader) => reader.ReadInt64BigEndian();


        public override void WriteArraySize(ConstantMemoryBufferWriter writer, int size) => writer.WriteBigEndian(size);
        public override void WriteStringSize(ConstantMemoryBufferWriter writer, int size) => writer.WriteBigEndian(size);
        public override void WriteByte(ConstantMemoryBufferWriter writer, byte value) => writer.Write(value);
        public override void WriteInt16(ConstantMemoryBufferWriter writer, short value) => writer.WriteBigEndian(value);
        public override void WriteFloat32(ConstantMemoryBufferWriter writer, float value) => writer.WriteBigEndian(value);
        public override void WriteFloat64(ConstantMemoryBufferWriter writer, double value) => writer.WriteBigEndian(value);
        public override void WriteInt32(ConstantMemoryBufferWriter writer, int value) => writer.WriteBigEndian(value);
        public override void WriteInt64(ConstantMemoryBufferWriter writer, long value) => writer.WriteBigEndian(value);
    }
}
