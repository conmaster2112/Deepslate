using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTInt64Tag(long value = default) : NBTTag(TagType.Int64)
    {
        public long Value = value;
        public override NBTInt64Tag Clone() => new(Value);

        public static implicit operator NBTInt64Tag(byte value) => new(value);
        public static implicit operator NBTInt64Tag(sbyte value) => new(value);
        public static implicit operator long(NBTInt64Tag tag) => tag.Value;
        public static implicit operator float(NBTInt64Tag tag) => tag.Value;
        public static implicit operator double(NBTInt64Tag tag) => tag.Value;


        public static explicit operator int(NBTInt64Tag tag) => (int)tag.Value;
        public static explicit operator short(NBTInt64Tag tag) => (short)tag.Value;
        public static explicit operator ushort(NBTInt64Tag tag) => (ushort)tag.Value;
        public static explicit operator uint(NBTInt64Tag tag) => (uint)tag.Value;
        public static explicit operator ulong(NBTInt64Tag tag) => (ulong)tag.Value;

        public static explicit operator byte(NBTInt64Tag tag) => (byte)tag.Value;
        public static explicit operator sbyte(NBTInt64Tag tag) => (sbyte)tag.Value;

        public static explicit operator NBTInt64Tag(short value) => new((byte)value);
        public static explicit operator NBTInt64Tag(ushort value) => new((byte)value);
        public static explicit operator NBTInt64Tag(int value) => new((byte)value);
        public static explicit operator NBTInt64Tag(uint value) => new((byte)value);
        public static explicit operator NBTInt64Tag(long value) => new((byte)value);
        public static explicit operator NBTInt64Tag(ulong value) => new((byte)value);
        public static explicit operator NBTInt64Tag(float value) => new((byte)value);
        public static explicit operator NBTInt64Tag(decimal value) => new((byte)value);
        public override string ToString()
        {
            return Value + "l";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
    }
    public struct NBTLongTagValue : INBTTag
    {
        public long Value;
        public readonly TagType Type => TagType.Int64;
        public readonly void Write(ConstantNBTWriter writer) => writer.Write(Value);
        public static unsafe implicit operator long(NBTLongTagValue tagValue) => *(long*)&tagValue;
        public static unsafe implicit operator NBTLongTagValue(long tagValue) => *(NBTLongTagValue*)&tagValue;
    }
}
