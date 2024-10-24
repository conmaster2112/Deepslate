namespace ConMaster
{
    public interface IPoolable<T> : IDisposable where T : class, IPoolable<T>, new()
    {
        public static T Rent() => ObjectPool<T>.Shared.Rent();
        public void Clean();
        public void Return() => ObjectPool<T>.Shared.Return((T)this);
        void IDisposable.Dispose() => Return();
    }
}
