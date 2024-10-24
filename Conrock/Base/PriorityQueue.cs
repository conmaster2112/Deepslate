using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Bedrock.Base
{
    public class PriorityQueue<T>
    {
        private readonly object _l = new();
        public bool IsEmpty => Next == null;
        private RefValue? Next;
        public bool TryDequeue([MaybeNullWhen(false)] out T value)
        {
            if (Next == null)
            {
                value = default;
                return false;
            }
            lock (_l)
            {
                value = Next.Value;
                Next = Next.Next;
                return true;
            }
        }
        public void Enqueue(T value, int priority = int.MaxValue)
        {
            lock (_l)
            {
                ref RefValue? current = ref Next;
                while (current != null)
                {
                    if (current.Priority > priority)
                    {
                        RefValue c = new(priority, value) { Next = current };
                        current = c;
                        return;
                    }
                    current = ref current.Next;
                }
                current = new RefValue(priority, value);
            }
        }
        public void Clear()
        {
            lock (_l) { Next = null; }
        }
        private class RefValue(int priority, T value)
        {
            public int Priority = priority;
            public T Value = value;
            public RefValue? Next;
            public bool IsLast => Next == null;
        }
    }
}
