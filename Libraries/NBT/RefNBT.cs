namespace ConMaster.Deepslate.NBT
{
    public enum TagTokenType : byte
    {
        Byte = 1,
        Int16 = 2,
        Int32 = 3,
        Int64 = 4,
        Float32 = 5,
        Float64 = 6,
        ByteArray = 7,
        String = 8,
        List = 9,
        Compoud = 10,
        Int32Array = 11,
        Int64Array = 12,

        TypeToken = 32,
        ArrayStart = 33,
        ArrayElement = 34,
        CompoudEntry = 35,
        EndOfCompoud = 0,

    }
    public ref struct NbtDocument
    {
        public TagTokenType TokenType;
        public ReadOnlySpan<byte> Source;
        public NbtElement Root
        {
            get
            {
                return new()
                {
                    Index = 0,
                    Root = this,
                    TagType = TokenType
                };
            }
        }
        public static NbtDocument ReadCompoudKey(ReadOnlySpan<byte> buffer)
        {
            return new()
            {
                Source = buffer,
                TokenType = TagTokenType.CompoudEntry,
            };
        }
    }
    public ref struct NbtElement
    {
        public TagTokenType TagType;
        public NbtDocument Root;
        public int Index;
    }
}
