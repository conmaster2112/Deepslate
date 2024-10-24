namespace ConMaster.Deepslate.NBT
{
    public interface IAllocator
    {
        public Memory<T> AllocMemory<T>(int size) where T : struct;
        public void ReleaseAll();
        public Memory<T> AllocMemoryWith<T>(ReadOnlySpan<T> bytes) where T : struct
        {
            Memory<T> memory = AllocMemory<T>(bytes.Length);
            bytes.CopyTo(memory.Span);
            return memory;
        }
        public Memory<byte> RentBufferUnsafe(int size, out uint id);
        public bool Return(uint id);
    }
}
