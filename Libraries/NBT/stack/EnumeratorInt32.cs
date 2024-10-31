using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.NBT
{
    public ref struct EnumeratorInt32
    {
        private readonly ConstantNBTReader _reader;
        private int _next = -1;
        private int _index = 0;
        private int _length = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal EnumeratorInt32(ConstantNBTReader reader, int length)
        {
            _reader = reader;
            _length = length;
        }

        /// <summary>Advances the enumerator to the next element of the span.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (_index >= _length) return false;
            _next = _reader.ReadInt32();
            _index++;
            return true;
        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int Current => _next;
        public readonly int Length => _length;
        public readonly int Remaining => _length - _index;

        /// <summary>Gets an enumerator for this span.</summary>
        public readonly EnumeratorInt32 GetEnumerator() => this;
        public void Dispose()
        {
            for (; _index < _length; _index++) _reader.ReadInt32();
        }
    }
}
