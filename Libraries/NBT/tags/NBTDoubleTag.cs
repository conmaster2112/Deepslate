using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTFloat64Tag(double value = default) : NBTTag(TagType.Float64)
    {
        public double Value = value;
        public override NBTFloat64Tag Clone() => new(Value);


        public static implicit operator NBTFloat64Tag(byte value) => new(value);
        public static implicit operator NBTFloat64Tag(sbyte value) => new(value);
        public static implicit operator double(NBTFloat64Tag tag) => tag.Value;


        public static explicit operator float(NBTFloat64Tag tag) => (float)tag.Value;
        public static explicit operator long(NBTFloat64Tag tag) => (long)tag.Value;
        public static explicit operator int(NBTFloat64Tag tag) => (int)tag.Value;
        public static explicit operator short(NBTFloat64Tag tag) => (short)tag.Value;
        public static explicit operator ushort(NBTFloat64Tag tag) => (ushort)tag.Value;
        public static explicit operator uint(NBTFloat64Tag tag) => (uint)tag.Value;
        public static explicit operator ulong(NBTFloat64Tag tag) => (ulong)tag.Value;

        public static explicit operator byte(NBTFloat64Tag tag) => (byte)tag.Value;
        public static explicit operator sbyte(NBTFloat64Tag tag) => (sbyte)tag.Value;

        public static explicit operator NBTFloat64Tag(short value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(ushort value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(int value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(uint value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(long value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(ulong value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(float value) => new((byte)value);
        public static explicit operator NBTFloat64Tag(decimal value) => new((byte)value);
        public override string ToString()
        {
            return Value + "d";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
        public struct NBTDoubleTagValue : INBTTag
        {
            public double Value;
            public readonly TagType Type => TagType.Float64;
            public readonly void Write(ConstantNBTWriter writer) => writer.Write(Value);
            public static unsafe implicit operator double(NBTDoubleTagValue tagValue) => *(double*)&tagValue;
            public static unsafe implicit operator NBTDoubleTagValue(double tagValue) => *(NBTDoubleTagValue*)&tagValue;
        }
    }
}
