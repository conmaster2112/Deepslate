namespace ConMaster.Deepslate.NBT
{
    public sealed class NBTInt32ArrayTag(ReadOnlyMemory<int>? value = default) : NBTTag(TagType.Int32Array)
    {
        public ReadOnlyMemory<int> Value = value ?? ReadOnlyMemory<int>.Empty;
        public override NBTInt32ArrayTag Clone() => new(Value);
        public static implicit operator ReadOnlyMemory<int>(NBTInt32ArrayTag tag) => tag.Value;
        public static implicit operator ReadOnlySpan<int>(NBTInt32ArrayTag tag) => tag.Value.Span;
        public ReadOnlySpan<int> Span => Value.Span;
        public int Length => Value.Length;
        public override void Write(ConstantNBTWriter writer)
        {
            writer.WriteInt32Array(Value.Span);
        }
    }
}
