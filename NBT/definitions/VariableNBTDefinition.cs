using ConMaster.Buffers;
using System.Text;

namespace ConMaster.Deepslate.NBT
{
    internal class VariableNBTDefinition : GeneralNBTDefinition
    {
        public override double ReadFloat64Tag(BinaryStream reader) => reader.ReadFloat64();
        public override double ReadFloat64Tag(ConstantMemoryBufferReader reader) => reader.ReadFloat64();

        public override float ReadFloat32Tag(BinaryStream reader) => reader.ReadFloat32();
        public override float ReadFloat32Tag(ConstantMemoryBufferReader reader) => reader.ReadFloat32();

        public override int ReadInt32Tag(BinaryStream reader) => reader.ReadVarInt32();
        public override int ReadInt32Tag(ConstantMemoryBufferReader reader) => reader.ReadVarInt32();

        public override long ReadInt64Tag(BinaryStream reader) => reader.ReadVarInt64();
        public override long ReadInt64Tag(ConstantMemoryBufferReader reader) => reader.ReadVarInt64();

        public override short ReadInt16Tag(BinaryStream reader) => reader.ReadInt16();
        public override short ReadInt16Tag(ConstantMemoryBufferReader reader) => reader.ReadInt16();

        public override string ReadStringTag(BinaryStream reader)
        {
            int length = (int)reader.ReadUVarInt32();
            uint id = uint.MaxValue;
            try
            {
                Span<byte> bytes = length < 255 ? stackalloc byte[length] : Allocator.RentBufferUnsafe(length, out id).Span;
                reader.ReadExactly(bytes);
                return Encoding.UTF8.GetString(bytes);
            }
            finally
            {
                if (id != uint.MaxValue) Allocator.Return(id);
            }
        }
        public override string ReadStringTag(ConstantMemoryBufferReader reader)
        {
            int length = (int)reader.ReadUVarInt32();
            return Encoding.UTF8.GetString(reader.ReadSlice(length));
        }


        public override void WriteFloat64Tag(ConstantMemoryBufferWriter writer, double value) => writer.Write(value);
        public override void WriteFloat64Tag(BinaryStream writer, double value) => writer.Write(value);

        public override void WriteFloat32Tag(ConstantMemoryBufferWriter writer, float value) => writer.Write(value);
        public override void WriteFloat32Tag(BinaryStream writer, float value) => writer.Write(value);

        public override void WriteInt32Tag(ConstantMemoryBufferWriter writer, int value) => writer.WriteVarInt32(value);
        public override void WriteInt32Tag(BinaryStream writer, int value) => writer.WriteVarInt32(value);

        public override void WriteInt64Tag(ConstantMemoryBufferWriter writer, long value) => writer.WriteVarInt64(value);
        public override void WriteInt64Tag(BinaryStream writer, long value) => writer.WriteVarInt64(value);

        public override void WriteInt16Tag(ConstantMemoryBufferWriter writer, short value) => writer.Write(value);
        public override void WriteInt16Tag(BinaryStream writer, short value) => writer.Write(value);

        public override void WriteStringTag(ConstantMemoryBufferWriter writer, ReadOnlySpan<char> value)
        {
            int maxLength = Encoding.UTF8.GetMaxByteCount(value.Length);
            Span<byte> bytes = writer.PeekFull().Slice(5);
            if (maxLength > bytes.Length) throw new IndexOutOfRangeException();
            int length = Encoding.UTF8.GetBytes(value, bytes);
            writer.WriteUVarInt32((uint)length);
            writer.Write(bytes.Slice(0, length));
        }
        public override void WriteStringTag(BinaryStream writer, ReadOnlySpan<char> value)
        {
            int maxLength = Encoding.UTF8.GetMaxByteCount(value.Length);
            uint id = uint.MaxValue;
            try
            {
                Span<byte> bytes = maxLength < 255 ? stackalloc byte[maxLength] : Allocator.RentBufferUnsafe(maxLength, out id).Span;
                int length = Encoding.UTF8.GetBytes(value, bytes);

                writer.WriteUVarInt32((uint)length);
                //Write(writer, (short)length);
                writer.Write(bytes.Slice(0, length));
            }
            finally
            {
                if (id != uint.MaxValue) Allocator.Return(id);
            }
        }
    }
}
