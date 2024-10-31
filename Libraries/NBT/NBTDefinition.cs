using ConMaster.Buffers;
using ConMaster.Deepslate.NBT;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.NBT
{
    public abstract class NBTDefiniton
    {
        public static readonly NBTDefiniton BedrockNBT = new BedrockNBTDefinition();
        public static readonly NBTDefiniton NetworkNBT = new VariableNBTDefinition();
        public abstract void WriteType(ConstantMemoryBufferWriter writer, TagType tag);
        public abstract void WriteByteTag(ConstantMemoryBufferWriter writer, byte value);
        public abstract void WriteInt16Tag(ConstantMemoryBufferWriter writer, short value);
        public abstract void WriteInt32Tag(ConstantMemoryBufferWriter writer, int value);
        public abstract void WriteInt64Tag(ConstantMemoryBufferWriter writer, long value);
        public abstract void WriteFloat32Tag(ConstantMemoryBufferWriter writer, float value);
        public abstract void WriteFloat64Tag(ConstantMemoryBufferWriter writer, double value);
        public abstract void WriteStringTag(ConstantMemoryBufferWriter writer, ReadOnlySpan<char> value);
        public virtual void WriteCompoudTag(ConstantMemoryBufferWriter writer, IDictionary<string, NBTTag> compoud)
        {
            foreach (var item in compoud)
            {
                NBTTag tag = item.Value;
                WriteType(writer, tag.Type);
                WriteStringTag(writer, item.Key);
                WriteRawTag(writer, tag);
            }
            WriteType(writer, TagType.EndOfCompoud);
        }
        public abstract void WriteListTag<T>(ConstantMemoryBufferWriter writer, NBTListTag<T> tag) where T : NBTTag;
        public abstract void WriteByteArray(ConstantMemoryBufferWriter writer, ReadOnlySpan<byte> value);
        public abstract void WriteInt32Array(ConstantMemoryBufferWriter writer, ReadOnlySpan<int> value);
        public abstract void WriteInt64Array(ConstantMemoryBufferWriter writer, ReadOnlySpan<long> value);

        public abstract void WriteType(BinaryStream writer, TagType tag);
        public abstract void WriteByteTag(BinaryStream writer, byte value);
        public abstract void WriteInt16Tag(BinaryStream writer, short value);
        public abstract void WriteInt32Tag(BinaryStream writer, int value);
        public abstract void WriteInt64Tag(BinaryStream writer, long value);
        public abstract void WriteFloat32Tag(BinaryStream writer, float value);
        public abstract void WriteFloat64Tag(BinaryStream writer, double value);
        public abstract void WriteStringTag(BinaryStream writer, ReadOnlySpan<char> value);
        public virtual void WriteCompoudTag(BinaryStream writer, IDictionary<string, NBTTag> compoud)
        {
            foreach (var item in compoud)
            {
                NBTTag tag = item.Value;
                WriteType(writer, tag.Type);
                WriteStringTag(writer, item.Key);
                WriteRawTag(writer, tag);
            }
            WriteType(writer, TagType.EndOfCompoud);
        }
        public abstract void WriteListTag<T>(BinaryStream writer, NBTListTag<T> tag) where T : NBTTag;
        public abstract void WriteByteArray(BinaryStream writer, ReadOnlySpan<byte> value);
        public abstract void WriteInt32Array(BinaryStream writer, ReadOnlySpan<int> value);
        public abstract void WriteInt64Array(BinaryStream writer, ReadOnlySpan<long> value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteRawTag(BinaryStream writer, NBTTag tag)
        {
            switch (tag.Type)
            {
                case TagType.Byte:
                    WriteByteTag(writer, (NBTByteTag)tag);
                    break;
                case TagType.Int16:
                    WriteInt16Tag(writer, (NBTInt16Tag)tag);
                    break;
                case TagType.Int32:
                    WriteInt32Tag(writer, (NBTIntTag)tag);
                    break;
                case TagType.Int64:
                    WriteInt64Tag(writer, (NBTInt64Tag)tag);
                    break;
                case TagType.Float32:
                    WriteFloat32Tag(writer, (NBTFloat32Tag)tag);
                    break;
                case TagType.Float64:
                    WriteFloat64Tag(writer, (NBTFloat64Tag)tag);
                    break;
                case TagType.String:
                    WriteStringTag(writer, (NBTStringTag)tag);
                    break;
                case TagType.List:
                    WriteListTag(writer, (NBTListTag<NBTTag>)tag);
                    break;
                case TagType.Compoud:
                    WriteCompoudTag(writer, (NBTCompoudTag)tag);
                    break;
                case TagType.ByteArray:
                    WriteByteArray(writer, (NBTByteArrayTag)tag);
                    break;
                case TagType.Int32Array:
                    WriteInt32Array(writer, (NBTInt32ArrayTag)tag);
                    break;
                case TagType.Int64Array:
                    WriteInt64Array(writer, (NBTInt64ArrayTag)tag);
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteRawTag(ConstantMemoryBufferWriter writer, NBTTag tag)
        {
            switch (tag.Type)
            {
                case TagType.Byte:
                    WriteByteTag(writer, (NBTByteTag)tag);
                    break;
                case TagType.Int16:
                    WriteInt16Tag(writer, (NBTInt16Tag)tag);
                    break;
                case TagType.Int32:
                    WriteInt32Tag(writer, (NBTIntTag)tag);
                    break;
                case TagType.Int64:
                    WriteInt64Tag(writer, (NBTInt64Tag)tag);
                    break;
                case TagType.Float32:
                    WriteFloat32Tag(writer, (NBTFloat32Tag)tag);
                    break;
                case TagType.Float64:
                    WriteFloat64Tag(writer, (NBTFloat64Tag)tag);
                    break;
                case TagType.String:
                    WriteStringTag(writer, (NBTStringTag)tag);
                    break;
                case TagType.List:
                    WriteListTag(writer, (NBTListTag<NBTTag>)tag);
                    break;
                case TagType.Compoud:
                    WriteCompoudTag(writer, (NBTCompoudTag)tag);
                    break;
                case TagType.ByteArray:
                    WriteByteArray(writer, (NBTByteArrayTag)tag);
                    break;
                case TagType.Int32Array:
                    WriteInt32Array(writer, (NBTInt32ArrayTag)tag);
                    break;
                case TagType.Int64Array:
                    WriteInt64Array(writer, (NBTInt64ArrayTag)tag);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteTag(ConstantMemoryBufferWriter writer, NBTTag tag)
        {
            WriteType(writer, tag.Type);
            WriteRawTag(writer, tag);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteTag(BinaryStream writer, NBTTag tag)
        {
            WriteType(writer, tag.Type);
            WriteRawTag(writer, tag);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteRootTag(ConstantMemoryBufferWriter writer, NBTTag tag, ReadOnlySpan<char> rootTag = default)
        {
            WriteType(writer, tag.Type);
            WriteStringTag(writer, rootTag);
            WriteRawTag(writer, tag);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteRootTag(BinaryStream writer, NBTTag tag, ReadOnlySpan<char> rootTag = default)
        {
            WriteType(writer, tag.Type);
            WriteStringTag(writer, rootTag);
            WriteRawTag(writer, tag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NBTTag ReadRawTag(BinaryStream reader, TagType tag)
        {
            return tag switch
            {
                TagType.Byte => new NBTByteTag(ReadByteTag(reader)),
                TagType.Int16 => new NBTInt16Tag(ReadInt16Tag(reader)),
                TagType.Int32 => new NBTIntTag(ReadInt32Tag(reader)),
                TagType.Int64 => new NBTInt64Tag(ReadInt64Tag(reader)),
                TagType.Float32 => new NBTFloat32Tag(ReadFloat32Tag(reader)),
                TagType.Float64 => new NBTFloat64Tag(ReadFloat64Tag(reader)),
                TagType.ByteArray => new NBTByteArrayTag(ReadByteArray(reader)),
                TagType.String => new NBTStringTag(ReadStringTag(reader)),
                TagType.List => ReadListTag(reader),
                TagType.Compoud => ReadCompoudTag(reader),
                TagType.Int32Array => new NBTInt32ArrayTag(ReadInt32Array(reader)),
                TagType.Int64Array => new NBTInt64ArrayTag(ReadInt64Array(reader)),
                _ => throw new InvalidEnumArgumentException("Invalid NBT Tag type used", (int)tag, typeof(TagType)),
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NBTTag ReadRawTag(ConstantMemoryBufferReader reader, TagType tag)
        {
            return tag switch
            {
                TagType.Byte => new NBTByteTag(ReadByteTag(reader)),
                TagType.Int16 => new NBTInt16Tag(ReadInt16Tag(reader)),
                TagType.Int32 => new NBTIntTag(ReadInt32Tag(reader)),
                TagType.Int64 => new NBTInt64Tag(ReadInt64Tag(reader)),
                TagType.Float32 => new NBTFloat32Tag(ReadFloat32Tag(reader)),
                TagType.Float64 => new NBTFloat64Tag(ReadFloat64Tag(reader)),
                TagType.ByteArray => new NBTByteArrayTag(ReadByteArray(reader)),
                TagType.String => new NBTStringTag(ReadStringTag(reader)),
                TagType.List => ReadListTag(reader),
                TagType.Compoud => ReadCompoudTag(reader),
                TagType.Int32Array => new NBTInt32ArrayTag(ReadInt32Array(reader)),
                TagType.Int64Array => new NBTInt64ArrayTag(ReadInt64Array(reader)),
                _ => throw new InvalidEnumArgumentException("Invalid NBT Tag type used", (int)tag, typeof(TagType)),
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NBTTag ReadTag(BinaryStream reader)
        {
            TagType type = ReadType(reader);
            return ReadRawTag(reader, type);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NBTTag ReadTag(ConstantMemoryBufferReader reader)
        {
            TagType type = ReadType(reader);
            return ReadRawTag(reader, type);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NBTTag ReadRootTag(BinaryStream reader, out string rootName)
        {
            TagType type = ReadType(reader);
            rootName = ReadStringTag(reader);
            return ReadRawTag(reader, type);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NBTTag ReadRootTag(ConstantMemoryBufferReader reader, out string rootName)
        {
            TagType type = ReadType(reader);
            rootName = ReadStringTag(reader);
            return ReadRawTag(reader, type);
        }

        public abstract TagType ReadType(BinaryStream reader);
        public abstract byte ReadByteTag(BinaryStream reader);
        public abstract short ReadInt16Tag(BinaryStream reader);
        public abstract int ReadInt32Tag(BinaryStream reader);
        public abstract long ReadInt64Tag(BinaryStream reader);
        public abstract float ReadFloat32Tag(BinaryStream reader);
        public abstract double ReadFloat64Tag(BinaryStream reader);
        public abstract string ReadStringTag(BinaryStream reader);
        public virtual NBTCompoudTag ReadCompoudTag(BinaryStream reader)
        {
            NBTCompoudTag tag = [];
            TagType type = ReadType(reader);
            while (type != TagType.EndOfCompoud)
            {
                string tagName = ReadStringTag(reader);
                tag[tagName] = ReadRawTag(reader, type);
                type = ReadType(reader);
            }
            return tag;
        }
        public abstract NBTListTag<NBTTag> ReadListTag(BinaryStream reader);
        public abstract ReadOnlyMemory<byte> ReadByteArray(BinaryStream reader);
        public abstract ReadOnlyMemory<int> ReadInt32Array(BinaryStream reader);
        public abstract ReadOnlyMemory<long> ReadInt64Array(BinaryStream reader);

        public abstract TagType ReadType(ConstantMemoryBufferReader reader);
        public abstract byte ReadByteTag(ConstantMemoryBufferReader reader);
        public abstract short ReadInt16Tag(ConstantMemoryBufferReader reader);
        public abstract int ReadInt32Tag(ConstantMemoryBufferReader reader);
        public abstract long ReadInt64Tag(ConstantMemoryBufferReader reader);
        public abstract float ReadFloat32Tag(ConstantMemoryBufferReader reader);
        public abstract double ReadFloat64Tag(ConstantMemoryBufferReader reader);
        public abstract string ReadStringTag(ConstantMemoryBufferReader reader);
        public virtual NBTCompoudTag ReadCompoudTag(ConstantMemoryBufferReader reader)
        {
            NBTCompoudTag tag = [];
            TagType type = ReadType(reader);
            while (type != TagType.EndOfCompoud)
            {
                tag[ReadStringTag(reader)] = ReadRawTag(reader, type);
                type = ReadType(reader);
            }
            return tag;
        }
        public abstract NBTListTag<NBTTag> ReadListTag(ConstantMemoryBufferReader reader);
        public abstract ReadOnlyMemory<byte> ReadByteArray(ConstantMemoryBufferReader reader);
        public abstract ReadOnlyMemory<int> ReadInt32Array(ConstantMemoryBufferReader reader);
        public abstract ReadOnlyMemory<long> ReadInt64Array(ConstantMemoryBufferReader reader);
    }
}
