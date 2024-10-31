using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTByteTag(byte value = default) : NBTTag(TagType.Byte)
    {
        public byte Value = value;
        public override NBTByteTag Clone() => new(Value);
        public static implicit operator byte(NBTByteTag tag) => tag.Value;
        public static implicit operator NBTByteTag(byte value) => new(value);
        public static implicit operator sbyte(NBTByteTag tag) => (sbyte)tag.Value;
        public static implicit operator NBTByteTag(sbyte value) => new((byte)value);
        public static implicit operator short(NBTByteTag tag) => tag.Value;
        public static implicit operator ushort(NBTByteTag tag) => tag.Value;
        public static implicit operator int(NBTByteTag tag) => tag.Value;
        public static implicit operator uint(NBTByteTag tag) => tag.Value;
        public static implicit operator long(NBTByteTag tag) => tag.Value;
        public static implicit operator ulong(NBTByteTag tag) => tag.Value;
        public static implicit operator float(NBTByteTag tag) => tag.Value;
        public static implicit operator double(NBTByteTag tag) => tag.Value;

        public static explicit operator NBTByteTag(short value) => new((byte)value);
        public static explicit operator NBTByteTag(ushort value) => new((byte)value);
        public static explicit operator NBTByteTag(int value) => new((byte)value);
        public static explicit operator NBTByteTag(uint value) => new((byte)value);
        public static explicit operator NBTByteTag(long value) => new((byte)value);
        public static explicit operator NBTByteTag(ulong value) => new((byte)value);
        public static explicit operator NBTByteTag(float value) => new((byte)value);
        public static explicit operator NBTByteTag(decimal value) => new((byte)value);
        public override string ToString()
        {
            return Value + "b";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
    }
    public struct NBTByteTagValue : INBTTag
    {
        public byte Value;
        public readonly TagType Type => TagType.Byte;
        public readonly void Write(ConstantNBTWriter writer) => writer.Write(Value);
        public static unsafe implicit operator byte(NBTByteTagValue tagValue) => *(byte*)&tagValue;
        public static unsafe implicit operator NBTByteTagValue(byte tagValue) => *(NBTByteTagValue*)&tagValue;
    }
}
