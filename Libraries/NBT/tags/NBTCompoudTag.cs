using ConMaster.Buffers;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ConMaster.Deepslate.NBT
{
    
    public sealed class NBTCompoudTag() : NBTTag(TagType.Compoud), IDictionary<string, NBTTag>
    {
        private readonly Dictionary<string, NBTTag> _dictionary = [];
        public int Count => _dictionary.Count;
        public NBTTag this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }
        public ICollection<string> Keys => _dictionary.Keys;
        public ICollection<NBTTag> Values => _dictionary.Values;
        public void Add(string key, NBTTag value) => _dictionary.Add(key, value);
        public void Clear() => _dictionary.Clear();
        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
        public bool Remove(string key) => _dictionary.Remove(key);
        public bool TryGetValue(string key,[MaybeNullWhen(false)] out NBTTag value) => _dictionary.TryGetValue(key, out value);
        public override NBTCompoudTag Clone()
        {
            NBTCompoudTag tag = [];
            foreach (var kvp in this) tag.Add(kvp.Key, kvp.Value.Clone());
            return tag;
        }

        #region Interface Implementation

        bool ICollection<KeyValuePair<string, NBTTag>>.Contains(KeyValuePair<string, NBTTag> item) => ((ICollection<KeyValuePair<string, NBTTag>>)_dictionary).Contains(item);
        void ICollection<KeyValuePair<string, NBTTag>>.CopyTo(KeyValuePair<string, NBTTag>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, NBTTag>>)_dictionary).CopyTo(array, arrayIndex);
        bool ICollection<KeyValuePair<string, NBTTag>>.Remove(KeyValuePair<string, NBTTag> item) => ((ICollection<KeyValuePair<string, NBTTag>>)_dictionary).Remove(item);
        void ICollection<KeyValuePair<string, NBTTag>>.Add(KeyValuePair<string, NBTTag> item) => ((ICollection<KeyValuePair<string, NBTTag>>)_dictionary).Add(item);
        public IEnumerator<KeyValuePair<string, NBTTag>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        bool ICollection<KeyValuePair<string, NBTTag>>.IsReadOnly => false;

        #endregion

        public override string ToString()
        {
            StringBuilder bBuilder = new StringBuilder("{\n");
            foreach (var kvp in this) bBuilder.Append($"  {kvp.Key}: {kvp.Value.ToString()?.Replace("\n", "\n  ") ?? ""},\n");
            bBuilder.Append('}');
            return bBuilder.ToString();
        }
        public override void Write(ConstantNBTWriter writer)
        {
            foreach(var t in this)
            {
                writer.WriteCompoudEntryRaw(t.Value.Type, t.Key.GetBytes());
                t.Value.Write(writer);
            }
            writer.WriteEndOfCompoud();
        }
    }
}
