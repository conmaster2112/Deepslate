namespace ConMaster.Deepslate.NBT
{
    public enum TagType: byte
    {
        EndOfCompoud = 0,
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
    }
}
