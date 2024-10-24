using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTInt16Tag(short value = default) : NBTTag(TagType.Int16)
    {
        public short Value = value;
        public override NBTInt16Tag Clone() => new(Value);

        public static implicit operator NBTInt16Tag(byte value) => new(value);
        public static implicit operator NBTInt16Tag(sbyte value) => new(value);
        public static implicit operator short(NBTInt16Tag tag) => tag.Value;
        public static implicit operator int(NBTInt16Tag tag) => tag.Value;
        public static implicit operator long(NBTInt16Tag tag) => tag.Value;
        public static implicit operator float(NBTInt16Tag tag) => tag.Value;
        public static implicit operator double(NBTInt16Tag tag) => tag.Value;


        public static explicit operator ushort(NBTInt16Tag tag) => (ushort)tag.Value;
        public static explicit operator uint(NBTInt16Tag tag) => (uint)tag.Value;
        public static explicit operator ulong(NBTInt16Tag tag) => (ulong)tag.Value;

        public static explicit operator byte(NBTInt16Tag tag) => (byte)tag.Value;
        public static explicit operator sbyte(NBTInt16Tag tag) => (sbyte)tag.Value;

        public static explicit operator NBTInt16Tag(short value) => new((byte)value);
        public static explicit operator NBTInt16Tag(ushort value) => new((byte)value);
        public static explicit operator NBTInt16Tag(int value) => new((byte)value);
        public static explicit operator NBTInt16Tag(uint value) => new((byte)value);
        public static explicit operator NBTInt16Tag(long value) => new((byte)value);
        public static explicit operator NBTInt16Tag(ulong value) => new((byte)value);
        public static explicit operator NBTInt16Tag(float value) => new((byte)value);
        public static explicit operator NBTInt16Tag(decimal value) => new((byte)value);
        public override string ToString()
        {
            return Value + "s";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
    }
    public struct NBTShortTagValue: INBTTag
    {
        public short Value;
        public readonly TagType Type => TagType.Int16;
        public readonly void Write(ConstantNBTWriter writer) => writer.Write(Value);
        public static unsafe implicit operator short(NBTShortTagValue tagValue) => *(short*)&tagValue;
        public static unsafe implicit operator NBTShortTagValue(short tagValue) => *(NBTShortTagValue*)&tagValue;
    }
}
