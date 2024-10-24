using ConMaster.Buffers;
using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.NBT
{
    public readonly ref struct ConstantNBTReader(ConstantMemoryBufferReader reader, NBTMode? mode = default)
    {
        public readonly NBTMode Mode = mode??NBTMode.Default;
        public readonly ConstantMemoryBufferReader Reader = reader;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TagType ReadType() => (TagType)Mode.ReadByte(Reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly byte ReadByte() => Mode.ReadByte(Reader);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly short ReadInt16() => Mode.ReadInt16(Reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int ReadInt32() => Mode.ReadInt32(Reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long ReadInt64() => Mode.ReadInt64(Reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float ReadFloat32() => Mode.ReadFloat32(Reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double ReadFloat64() => Mode.ReadFloat64(Reader);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<byte> ReadRawString()
        {
            int length = Mode.ReadStringSize(Reader);
            return Reader.ReadSlice(length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<byte> ReadByteArray()
        {
            int length = Mode.ReadArraySize(Reader);
            return Reader.ReadSlice(length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly EnumeratorInt32 ReadInt32Array() => new(this, Mode.ReadArraySize(Reader));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly EnumeratorInt64 ReadInt64Array() => new(this, Mode.ReadArraySize(Reader));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool ReadCompoudEntry(out TagType type, out ReadOnlySpan<byte> key)
        {
            type = ReadType();
            if (type == TagType.EndOfCompoud)
            {
                key = default;
                return false;
            }
            key = ReadRawString();
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int ReadListHeader(out TagType listType)
        {
            listType = ReadType();
            return Mode.ReadArraySize(Reader);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void SkipTag(TagType tag) => Mode.Skip(tag, Reader);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(ConstantNBTWriter writer, bool isRoot = false)
        {
            TagType type;
            if (isRoot)
            {
                ReadCompoudEntry(out type, out ReadOnlySpan<byte> key);
                writer.WriteCompoudEntryRaw(type, key);
            }
            else
            {
                type = ReadType();
                writer.WriteType(type);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public readonly void CopyTo(ConstantNBTWriter writer, TagType type)
        {
            switch (type)
            {
                case TagType.Byte: writer.WriteByte(ReadByte()); break;
                case TagType.Int16: writer.WriteInt16(ReadInt16()); break;
                case TagType.Int32: writer.WriteInt32(ReadInt32()); break;
                case TagType.Int64: writer.WriteInt64(ReadInt64()); break;
                case TagType.Float32: writer.WriteFloat32(ReadFloat32()); break;
                case TagType.Float64: writer.WriteFloat64(ReadFloat64()); break;
                case TagType.ByteArray: writer.WriteByteArray(ReadByteArray()); break;
                case TagType.String: writer.WriteString(ReadRawString()); break;
                case TagType.List:
                    type = ReadType();
                    writer.WriteType(type);
                    int size = Mode.ReadArraySize(Reader);
                    writer.Mode.WriteArraySize(writer.Writer, size);
                    for (int i = 0; i < size; i++) CopyTo(writer, type);
                    break;
                case TagType.Compoud:
                    while (ReadCompoudEntry(out type, out ReadOnlySpan<byte> key))
                    {
                        writer.WriteCompoudEntryRaw(type, key);
                        CopyTo(writer, type);
                    }
                    writer.WriteEndOfCompoud();
                    break;
                case TagType.Int32Array:
                    writer.WriteInt32Array(ReadInt32Array());
                    break;
                case TagType.Int64Array:
                    writer.WriteInt64Array(ReadInt64Array());
                    break;
            }
        }

    }
}
