using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTIntTag(int value = default) : NBTTag(TagType.Int32)
    {
        public int Value = value;
        public override NBTIntTag Clone() => new(Value);


        public static implicit operator NBTIntTag(byte value) => new(value);
        public static implicit operator NBTIntTag(sbyte value) => new(value);
        public static implicit operator int(NBTIntTag tag) => tag.Value;
        public static implicit operator long(NBTIntTag tag) => tag.Value;
        public static implicit operator float(NBTIntTag tag) => tag.Value;
        public static implicit operator double(NBTIntTag tag) => tag.Value;


        public static explicit operator short(NBTIntTag tag) => (short)tag.Value;
        public static explicit operator ushort(NBTIntTag tag) => (ushort)tag.Value;
        public static explicit operator uint(NBTIntTag tag) => (uint)tag.Value;
        public static explicit operator ulong(NBTIntTag tag) => (ulong)tag.Value;

        public static explicit operator byte(NBTIntTag tag) => (byte)tag.Value;
        public static explicit operator sbyte(NBTIntTag tag) => (sbyte)tag.Value;

        public static explicit operator NBTIntTag(short value) => new((byte)value);
        public static explicit operator NBTIntTag(ushort value) => new((byte)value);
        public static explicit operator NBTIntTag(int value) => new((byte)value);
        public static explicit operator NBTIntTag(uint value) => new((byte)value);
        public static explicit operator NBTIntTag(long value) => new((byte)value);
        public static explicit operator NBTIntTag(ulong value) => new((byte)value);
        public static explicit operator NBTIntTag(float value) => new((byte)value);
        public static explicit operator NBTIntTag(decimal value) => new((byte)value);
        public override string ToString()
        {
            return Value + "i";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
    }
    public struct NBTIntTagValue : INBTTag
    {
        public int Value;
        public readonly TagType Type => TagType.Int32;
        public readonly void Write(ConstantNBTWriter writer) => writer.Write(Value);
        public static unsafe implicit operator int(NBTIntTagValue tagValue) => *(int*)&tagValue;
        public static unsafe implicit operator NBTIntTagValue(int tagValue) => *(NBTIntTagValue*)&tagValue;
    }
}
