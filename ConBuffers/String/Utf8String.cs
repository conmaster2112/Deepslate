using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConMaster.Buffers.String
{
    public readonly struct Utf8String
      : IEquatable<Utf8String>,
        IEnumerable,
        IEnumerable<byte>,
        IEquatable<Utf8String?>,
        ICloneable
    {
        private readonly byte[] _b;
        private byte[] _init { init => _b = value; }
        public int Length => _b?.Length ?? 0;
        public bool IsEmpty => Length == 0;
        public byte this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _b[index];
        }
        public ReadOnlySpan<byte> Span => _b.AsSpan();
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null) return false;

            if (obj is not Utf8String str) return false;

            if (Length != str.Length) return false;

            return _b.AsSpan().SequenceEqual(str._b);
        }
        public override string ToString() => Encoding.UTF8.GetString(_b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => InternalStringHelper.ComputeHash32(_b);
        public bool Equals(Utf8String other) => _b.AsSpan().SequenceEqual(other._b);
        public bool Equals(ReadOnlySpan<byte> other) => _b.AsSpan().SequenceEqual(other);

        public IEnumerator GetEnumerator() => _b.GetEnumerator();
        IEnumerator<byte> IEnumerable<byte>.GetEnumerator() => _b.AsEnumerable().GetEnumerator();

        public object Clone()
        {
            byte[] array = GC.AllocateUninitializedArray<byte>(_b.Length);
            _b.AsSpan().CopyTo(array);
            return new Utf8String()
            {
                _init = array
            };
        }
        public bool Equals(Utf8String? other) => Equals(other!.Value);
        public static bool operator ==(Utf8String v1, Utf8String v2) => v1.Equals(v2);
        public static bool operator !=(Utf8String v1, Utf8String v2) => !v1.Equals(v2);
        public static bool operator ==(Utf8String v1, ReadOnlySpan<byte> v2) => v2.SequenceEqual(v1._b);
        public static bool operator !=(Utf8String v1, ReadOnlySpan<byte> v2) => !v2.SequenceEqual(v1._b);
        public static implicit operator ReadOnlySpan<byte>(Utf8String v) => v._b;
        //public static explicit operator Utf8String(ReadOnlySpan<byte> v) => From(v);
        public static implicit operator Utf8String(ReadOnlySpan<byte> v) => From(v);

        public static Utf8String From(string text)
        {
            return new()
            {
                _init = text.GetBytes()
            };
        }
        public static Utf8String From(ReadOnlySpan<byte> utf8text)
        {
            byte[] bytes = GC.AllocateUninitializedArray<byte>(utf8text.Length);
            utf8text.CopyTo(bytes);
            return new()
            {
                _init = bytes
            };
        }
    }
}