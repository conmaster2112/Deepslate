namespace ConMaster.Deepslate.NBT
{
    public sealed class NBTInt64ArrayTag(ReadOnlyMemory<long>? value = default) : NBTTag(TagType.Int64Array)
    {
        public ReadOnlyMemory<long> Value = value ?? ReadOnlyMemory<long>.Empty;
        public override NBTInt64ArrayTag Clone() => new(Value);
        public static implicit operator ReadOnlyMemory<long>(NBTInt64ArrayTag tag) => tag.Value;
        public static implicit operator ReadOnlySpan<long>(NBTInt64ArrayTag tag) => tag.Value.Span;
        public ReadOnlySpan<long> Span => Value.Span;
        public int Length => Value.Length;
        public override void Write(ConstantNBTWriter writer)
        {
            writer.WriteInt64Array(Value.Span);
        }
    }
}
