using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.NBT
{
    public ref struct EnumeratorInt64
    {
        private readonly ConstantNBTReader _reader;
        private long _next = -1;
        private int _index = 0;
        private int _length = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal EnumeratorInt64(ConstantNBTReader reader, int length)
        {
            _reader = reader;
            _length = length;
        }

        /// <summary>Advances the enumerator to the next element of the span.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (_index >= _length) return false;
            _next = _reader.ReadInt64();
            _index++;
            return true;
        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Advances the enumerator to the next element of the span.</summary>
        public readonly long Current => _next;
        public readonly int Length => _length;
        public readonly int Remaining => _length - _index;

        /// <summary>Gets an enumerator for this span.</summary>
        /// <summary>Advances the enumerator to the next element of the span.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly EnumeratorInt64 GetEnumerator() => this;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            for (; _index < _length; _index++) _reader.ReadInt64();
        }
    }
}
