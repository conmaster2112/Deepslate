using System.Buffers;
using System.Collections.Concurrent;

namespace ConMaster.Deepslate.NBT
{
    class BasicAllocator : IAllocator
    {
        private volatile uint Id = 0;
        private ConcurrentDictionary<uint, byte[]> referencies = new();
        public Memory<T> AllocMemory<T>(int size) where T : struct
        {
            return GC.AllocateUninitializedArray<T>(size);
        }
        public void ReleaseAll()
        {
            lock (referencies)
            {
                foreach (var t in referencies.Values) ArrayPool<byte>.Shared.Return(t);
                referencies.Clear();
            }
        }

        public Memory<byte> RentBufferUnsafe(int size, out uint id)
        {
            lock (this) id = Id++;
            byte[] reff = ArrayPool<byte>.Shared.Rent(size);
            lock(referencies) referencies[id] = reff;
            return reff.AsMemory(0, size);
        }

        public bool Return(uint id)
        {
            lock (referencies) {
                if (!referencies.TryRemove(id, out byte[]? reff)) return false;
                ArrayPool<byte>.Shared.Return(reff);
                return true;
            }
        }
    }
}
