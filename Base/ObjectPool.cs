using System.Collections.Concurrent;

namespace ConMaster
{
    public class ObjectPool<T>() where T : class, IPoolable<T>, new()
    {
        public static ObjectPool<T> Shared { get; private set; } = new();
        protected readonly ConcurrentStack<T> _Stack = new();
        public T Rent()
        {
            if (_Stack.TryPop(out T? result)) return result;
            return new();
        }
        public void Return(T value)
        {
            value.Clean();
            _Stack.Push(value);
        }
    }
}
