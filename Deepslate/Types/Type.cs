using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.Types
{
    public abstract class Types<T> where T : Type<T>
    {
        private static readonly Dictionary<string, T> _Types = [];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<T> GetAll() => _Types.Values;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? Get(string type)
        {
            _Types.TryGetValue(type, out T? value);
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddType(T type) => _Types[type.Id] = type;
    }
    public abstract class Type<T> where T : Type<T>
    {
        public Type(string id)
        {
            Id = id;
            Types<T>.AddType((T)this);
        }
        public string Id { get; init; }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
