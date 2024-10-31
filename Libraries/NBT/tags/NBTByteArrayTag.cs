using System.Collections;

namespace ConMaster.Deepslate.NBT
{
    public sealed class NBTByteArrayTag(ReadOnlyMemory<byte>? value = default) : NBTTag(TagType.ByteArray)
    {
        public ReadOnlyMemory<byte> Value = value ?? ReadOnlyMemory<byte>.Empty;
        public override NBTByteArrayTag Clone() => new(Value);



        public static implicit operator ReadOnlyMemory<byte>(NBTByteArrayTag tag) => tag.Value;
        public static implicit operator ReadOnlySpan<byte>(NBTByteArrayTag tag) => tag.Value.Span;
        public ReadOnlySpan<byte> Span => Value.Span;
        public int Length => Value.Length;
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(Value.Span);
        }
    }
}
