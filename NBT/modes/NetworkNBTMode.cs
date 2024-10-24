using ConMaster.Buffers;
using System;
namespace ConMaster.Deepslate.NBT
{
    internal class NetworkNBTMode : NBTMode
    {
        public override int ReadArraySize(ConstantMemoryBufferReader reader) => (int)reader.ReadUVarInt32();
        public override int ReadStringSize(ConstantMemoryBufferReader reader) => (int)reader.ReadUVarInt32();
        public override byte ReadByte(ConstantMemoryBufferReader reader) => reader.ReadUInt8();
        public override float ReadFloat32(ConstantMemoryBufferReader reader) => reader.ReadFloat32();
        public override double ReadFloat64(ConstantMemoryBufferReader reader) => reader.ReadFloat64();
        public override int ReadInt32(ConstantMemoryBufferReader reader) => reader.ReadVarInt32();
        public override short ReadInt16(ConstantMemoryBufferReader reader) => reader.ReadInt16();
        public override long ReadInt64(ConstantMemoryBufferReader reader) => reader.ReadVarInt64();


        public override void WriteArraySize(ConstantMemoryBufferWriter writer, int size) => writer.WriteUVarInt32((uint)size);
        public override void WriteStringSize(ConstantMemoryBufferWriter writer, int size) => writer.WriteUVarInt32((uint)size);
        public override void WriteByte(ConstantMemoryBufferWriter writer, byte value) => writer.Write(value);
        public override void WriteInt16(ConstantMemoryBufferWriter writer, short value) => writer.Write(value);
        public override void WriteFloat32(ConstantMemoryBufferWriter writer, float value) => writer.Write(value);
        public override void WriteFloat64(ConstantMemoryBufferWriter writer, double value) => writer.Write(value);
        public override void WriteInt32(ConstantMemoryBufferWriter writer, int value) => writer.WriteVarInt32(value);
        public override void WriteInt64(ConstantMemoryBufferWriter writer, long value) => writer.WriteVarInt64(value);
    }
}
