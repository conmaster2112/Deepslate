using ConMaster.Buffers;

namespace ConMaster.Deepslate.NBT
{
    public abstract class NBTTag: ICloneable, INBTTag
    {
        public TagType Type { get; init; }

        internal NBTTag(TagType type) {
            Type = type;
        }
        public abstract NBTTag Clone();
        object ICloneable.Clone()=>Clone();
        public abstract void Write(ConstantNBTWriter writer);
    }
    public interface INBTTag
    {
        public TagType Type { get; }
        public void Write(ConstantNBTWriter writer);
    }
    public interface INBTCompoudTag : INBTTag
    {
        TagType INBTTag.Type => TagType.Compoud;
        public IEnumerable<KeyValuePair<string, INBTTag>> GetCompoudEntries();
        public INBTTag? GetProperty(string key);
        void INBTTag.Write(ConstantNBTWriter writer)
        {
            foreach(KeyValuePair<string, INBTTag> kv in GetCompoudEntries())
            {
                writer.WriteCompoudEntryRaw(kv.Value.Type, kv.Key.GetBytes());
                kv.Value.Write(writer);
            }
        }
    }
}
