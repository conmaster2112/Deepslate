using ConMaster.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConMaster.Deepslate.NBT
{
    public abstract class GeneralNBTDefinition(IAllocator allocator) : NBTDefiniton
    {
        public readonly IAllocator Allocator = allocator;
        public GeneralNBTDefinition() : this(new BasicAllocator()) { }

        public virtual void WriteArraySize(ConstantMemoryBufferWriter writer, int value) => WriteInt32Tag(writer, value);
        public virtual void WriteArraySize(BinaryStream writer, int value) => WriteInt32Tag(writer, value);
        public virtual int ReadArraySize(ConstantMemoryBufferReader reader) => ReadInt32Tag(reader);
        public virtual int ReadArraySize(BinaryStream reader) => ReadInt32Tag(reader);


        public override void WriteType(ConstantMemoryBufferWriter writer, TagType tag) => WriteByteTag(writer, (byte)tag);
        public override void WriteType(BinaryStream writer, TagType tag) => WriteByteTag(writer, (byte)tag);
        public override TagType ReadType(BinaryStream reader) => (TagType)ReadByteTag(reader);
        public override TagType ReadType(ConstantMemoryBufferReader reader) => (TagType)ReadByteTag(reader);


        public override void WriteByteTag(ConstantMemoryBufferWriter writer, byte value) => writer.Write(value);
        public override void WriteByteTag(BinaryStream writer, byte value) => writer.Write(value);
        public override byte ReadByteTag(BinaryStream reader) => reader.ReadUInt8();
        public override byte ReadByteTag(ConstantMemoryBufferReader reader) => reader.ReadUInt8();



        public override void WriteListTag<T>(BinaryStream writer, NBTListTag<T> tag)
        {
            WriteType(writer, tag.ListType);
            int length = tag.Count;
            WriteArraySize(writer, length);
            for (int i = 0; i < length; i++) WriteRawTag(writer, tag[i]);
        }
        public override void WriteListTag<T>(ConstantMemoryBufferWriter writer, NBTListTag<T> tag)
        {
            WriteType(writer, tag.ListType);
            int length = tag.Count;
            WriteArraySize(writer, length);
            for (int i = 0; i < length; i++) WriteRawTag(writer, tag[i]);
        }
        public override NBTListTag<NBTTag> ReadListTag(BinaryStream reader)
        {
            TagType type = ReadType(reader);
            NBTListTag<NBTTag> tag = new(type);
            int size = ReadArraySize(reader);
            for (int i = 0; i < size; i++) tag.Add(ReadRawTag(reader, type));
            return tag;
        }
        public override NBTListTag<NBTTag> ReadListTag(ConstantMemoryBufferReader reader)
        {
            TagType type = ReadType(reader);
            NBTListTag<NBTTag> tag = new(type);
            int size = ReadArraySize(reader);
            for (int i = 0; i < size; i++) tag.Add(ReadRawTag(reader, type));
            return tag;
        }



        public override void WriteByteArray(BinaryStream writer, ReadOnlySpan<byte> value)
        {
            WriteArraySize(writer, value.Length);
            writer.Write(value);
        }
        public override void WriteByteArray(ConstantMemoryBufferWriter writer, ReadOnlySpan<byte> value)
        {
            WriteArraySize(writer, value.Length);
            writer.Write(value);
        }
        public override ReadOnlyMemory<byte> ReadByteArray(BinaryStream reader)
        {
            int size = ReadArraySize(reader);
            Memory<byte> data = Allocator.AllocMemory<byte>(size);
            reader.ReadExactly(data.Span);
            return data;
        }
        public override ReadOnlyMemory<byte> ReadByteArray(ConstantMemoryBufferReader reader)
        {
            int size = ReadArraySize(reader);
            Memory<byte> data = Allocator.AllocMemory<byte>(size);
            reader.ReadSlice(size).CopyTo(data.Span);
            return data;
        }


        public override void WriteInt32Array(BinaryStream writer, ReadOnlySpan<int> value)
        {
            int length = value.Length;
            WriteArraySize(writer, length);
            for (int i = 0; i < length; i++) WriteInt32Tag(writer, value[i]);
        }
        public override void WriteInt32Array(ConstantMemoryBufferWriter writer, ReadOnlySpan<int> value)
        {
            int length = value.Length;
            WriteArraySize(writer, length);
            for (int i = 0; i < length; i++) WriteInt32Tag(writer, value[i]);
        }
        public override ReadOnlyMemory<int> ReadInt32Array(BinaryStream reader)
        {
            int size = ReadArraySize(reader);
            Memory<int> data = Allocator.AllocMemory<int>(size);
            Span<int> span = data.Span;
            for (int i = 0; i < size; i++) span[i] = ReadInt32Tag(reader);
            return data;
        }
        public override ReadOnlyMemory<int> ReadInt32Array(ConstantMemoryBufferReader reader)
        {
            int size = ReadArraySize(reader);
            Memory<int> data = Allocator.AllocMemory<int>(size);
            Span<int> span = data.Span;
            for (int i = 0; i < size; i++) span[i] = ReadInt32Tag(reader);
            return data;
        }


        public override void WriteInt64Array(BinaryStream writer, ReadOnlySpan<long> value)
        {
            int length = value.Length;
            WriteArraySize(writer, length);
            for (int i = 0; i < length; i++) WriteInt64Tag(writer, value[i]);
        }
        public override void WriteInt64Array(ConstantMemoryBufferWriter writer, ReadOnlySpan<long> value)
        {
            int length = value.Length;
            WriteArraySize(writer, length);
            for (int i = 0; i < length; i++) WriteInt64Tag(writer, value[i]);
        }
        public override ReadOnlyMemory<long> ReadInt64Array(BinaryStream reader)
        {
            int size = ReadArraySize(reader);
            Memory<long> data = Allocator.AllocMemory<long>(size);
            Span<long> span = data.Span;
            for (int i = 0; i < size; i++) span[i] = ReadInt64Tag(reader);
            return data;
        }
        public override ReadOnlyMemory<long> ReadInt64Array(ConstantMemoryBufferReader reader)
        {
            int size = ReadArraySize(reader);
            Memory<long> data = Allocator.AllocMemory<long>(size);
            Span<long> span = data.Span;
            for (int i = 0; i < size; i++) span[i] = ReadInt64Tag(reader);
            return data;
        }
    }
}
