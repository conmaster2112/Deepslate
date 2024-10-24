using ConMaster.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConMaster.Deepslate.NBT
{
    public readonly ref struct ConstantNBTWriter(ConstantMemoryBufferWriter reader, NBTMode? mode = default)
    {
        public readonly NBTMode Mode = mode ?? NBTMode.Default;
        public readonly ConstantMemoryBufferWriter Writer = reader;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteArraySize(int size) => Mode.WriteArraySize(Writer, size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteType(TagType type) => Mode.WriteByte(Writer, (byte)type);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteByte(byte value) => Mode.WriteByte(Writer, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt16(short value) => Mode.WriteInt16(Writer, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt32(int value) => Mode.WriteInt32(Writer, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt64(long value) => Mode.WriteInt64(Writer, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteFloat32(float value) => Mode.WriteFloat32(Writer, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteFloat64(double value) => Mode.WriteFloat64(Writer, value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteString(ReadOnlySpan<byte> bytes)
        {
            Mode.WriteStringSize(Writer, bytes.Length);
            Writer.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteString(string bytes)
        {
            byte[] data = Encoding.UTF8.GetBytes(bytes);
            Mode.WriteStringSize(Writer, data.Length);
            Writer.Write(data);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteByteArray(ReadOnlySpan<byte> bytes)
        {
            Mode.WriteStringSize(Writer, bytes.Length);
            Writer.Write(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt32Array(EnumeratorInt32 enumerator)
        {
            Mode.WriteArraySize(Writer, enumerator.Remaining);
            if (enumerator.Remaining > 0) foreach (int i in enumerator) Mode.WriteInt32(Writer, i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt64Array(EnumeratorInt64 enumerator)
        {
            Mode.WriteArraySize(Writer, enumerator.Remaining);
            if (enumerator.Remaining > 0) foreach (long i in enumerator) Mode.WriteInt64(Writer, i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt32Array(IReadOnlyCollection<int> enumerator)
        {
            Mode.WriteArraySize(Writer, enumerator.Count);
            if (enumerator.Count > 0) foreach (int i in enumerator) Mode.WriteInt32(Writer, i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt64Array(IReadOnlyCollection<long> enumerator)
        {
            Mode.WriteArraySize(Writer, enumerator.Count);
            if (enumerator.Count > 0) foreach (long i in enumerator) Mode.WriteInt64(Writer, i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt32Array(ReadOnlySpan<int> enumerator)
        {
            Mode.WriteArraySize(Writer, enumerator.Length);
            if (enumerator.Length > 0) foreach (int i in enumerator) Mode.WriteInt32(Writer, i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteInt64Array(ReadOnlySpan<long> enumerator)
        {
            Mode.WriteArraySize(Writer, enumerator.Length);
            if (enumerator.Length > 0) foreach (long i in enumerator) Mode.WriteInt64(Writer, i);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntryRaw(TagType type, ReadOnlySpan<byte> key)
        {
            WriteType(type);
            if (type == TagType.EndOfCompoud) return;
            WriteString(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, byte value)
        {
            WriteType(TagType.Byte);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, short value)
        {
            WriteType(TagType.Int16);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, int value)
        {
            WriteType(TagType.Int32);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, long value)
        {
            WriteType(TagType.Int64);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, float value)
        {
            WriteType(TagType.Float32);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, double value)
        {
            WriteType(TagType.Float64);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, ReadOnlySpan<byte> value)
        {
            WriteType(TagType.String);
            WriteString(key);
            WriteString(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, string value)
        {
            WriteType(TagType.String);
            WriteString(key);
            Write(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteCompoudEntry(ReadOnlySpan<byte> key, INBTTag value)
        {
            WriteType(value.Type);
            WriteString(key);
            Write(value);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void WriteEndOfCompoud() => WriteType(TagType.EndOfCompoud);



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(TagType type) => WriteType(type);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(byte value) => WriteByte(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(string value) => WriteString(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(short value) => WriteInt16(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(int value) => WriteInt32(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(long value) => WriteInt64(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(float value) => WriteFloat32(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(double value) => WriteFloat64(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(ReadOnlySpan<byte> value) => WriteByteArray(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(ReadOnlySpan<int> value) => WriteInt32Array(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(IReadOnlyCollection<int> value) => WriteInt32Array(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(ReadOnlySpan<long> value) => WriteInt64Array(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(IReadOnlyCollection<long> value) => WriteInt64Array(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Write(INBTTag tag) => tag.Write(this);
    }
}
