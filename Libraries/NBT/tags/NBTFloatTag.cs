using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTFloat32Tag(float value = default) : NBTTag(TagType.Float32)
    {
        public float Value = value;
        public override NBTFloat32Tag Clone() => new(Value);

        public static implicit operator NBTFloat32Tag(byte value) => new(value);
        public static implicit operator NBTFloat32Tag(sbyte value) => new(value);
        public static implicit operator float(NBTFloat32Tag tag) => tag.Value;
        public static implicit operator double(NBTFloat32Tag tag) => tag.Value;


        public static explicit operator long(NBTFloat32Tag tag) => (long)tag.Value;
        public static explicit operator int(NBTFloat32Tag tag) => (int)tag.Value;
        public static explicit operator short(NBTFloat32Tag tag) => (short)tag.Value;
        public static explicit operator ushort(NBTFloat32Tag tag) => (ushort)tag.Value;
        public static explicit operator uint(NBTFloat32Tag tag) => (uint)tag.Value;
        public static explicit operator ulong(NBTFloat32Tag tag) => (ulong)tag.Value;

        public static explicit operator byte(NBTFloat32Tag tag) => (byte)tag.Value;
        public static explicit operator sbyte(NBTFloat32Tag tag) => (sbyte)tag.Value;

        public static explicit operator NBTFloat32Tag(short value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(ushort value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(int value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(uint value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(long value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(ulong value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(float value) => new((byte)value);
        public static explicit operator NBTFloat32Tag(decimal value) => new((byte)value);
        public override string ToString()
        {
            return Value + "f";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
    }
    public struct NBTFloatTagValue : INBTTag
    {
        public float Value;
        public readonly TagType Type => TagType.Float32;
        public readonly void Write(ConstantNBTWriter writer) => writer.Write(Value);
        public static unsafe implicit operator float(NBTFloatTagValue tagValue) => *(float*)&tagValue;
        public static unsafe implicit operator NBTFloatTagValue(float tagValue) => *(NBTFloatTagValue*)&tagValue;
    }
}
