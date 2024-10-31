using ConMaster.Buffers;
using System.Runtime.InteropServices;

namespace ConMaster.Deepslate.NBT
{
    public sealed class NBTStringTag(string? data) : NBTTag(TagType.String)
    {
        public string Value = data??string.Empty;
        public override NBTStringTag Clone() => new(Value);



        public static implicit operator string(NBTStringTag tag)=>tag.Value;
        public static implicit operator NBTStringTag(string value)=>new(value);
        public static implicit operator ReadOnlySpan<char>(NBTStringTag tag)=>tag.Value;

        public static NBTStringTag operator +(NBTStringTag v1, NBTStringTag v2) => new(v1.Value + v2.Value);
        public override string ToString()
        {
            return $"\"{Value.Replace("\"","\\\"").Replace("\n","\\n")}\"";
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.WriteString(Value);
        }
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct NBTStringTagValue : INBTTag
    {
        [FieldOffset(0)]
        public string Value;
        public readonly TagType Type => TagType.String;
        public readonly void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value);
        }
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        public static unsafe implicit operator string(NBTStringTagValue tagValue) => *(string*)&tagValue;
        public static unsafe implicit operator NBTStringTagValue(string tagValue) => *(NBTStringTagValue*)&tagValue;
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
    }
}
