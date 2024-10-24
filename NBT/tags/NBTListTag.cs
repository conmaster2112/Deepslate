using ConMaster.Buffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Deepslate.NBT
{
    public sealed class NBTListTag<T> : NBTTag, IList<T> where T : NBTTag
    {
        public NBTListTag(TagType colectionType, IEnumerable<T>? collection = default) : base(TagType.List)
        {
            ListType = colectionType;
            if (collection != null) foreach (var item in collection) Add(item);
        }
        private readonly List<T> _list = [];
        public TagType ListType { get; init; }
        public T this[int index] {
            get => _list[index];
            set {
                if (value.Type != ListType) throw new Exception("Trying to insert NBT_Tag of different type.");
                _list[index] = value;
            }
        }
        public int Count => _list.Count;
        public int Length => _list.Count;
        public void Add(T item)
        {
            if (item.Type != ListType) throw new Exception("Trying to insert NBT_Tag of different type.");
            _list.Add(item);
        }
        public void Clear() => _list.Clear();
        public bool Contains(T item) => _list.Contains(item);
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        public int IndexOf(T item) => _list.IndexOf(item);
        public void Insert(int index, T item) {
            if(item.Type != ListType) throw new Exception("Trying to insert NBT_Tag of different type.");
            _list.Insert(index, item);
        }
        public bool Remove(T item) => _list.Remove(item);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException("by design");
        bool ICollection<T>.IsReadOnly => false;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override NBTListTag<T> Clone()
        {
            NBTListTag<T> tag = new(ListType);
            foreach (var item in this) tag.Add((T)item.Clone());
            return tag;
        }

        public override string ToString()
        {
            StringBuilder bBuilder = new StringBuilder("[\n");
            foreach (var kvp in this) bBuilder.Append($"  {kvp.ToString()?.Replace("\n", "\n  ")??""},\n");
            bBuilder.Append(']');
            return bBuilder.ToString();
        }
        public override void Write(ConstantNBTWriter writer)
        {
            writer.Write(ListType);
            writer.WriteArraySize(Count);
            foreach(var n in this) n.Write(writer);
        }
    }
}
