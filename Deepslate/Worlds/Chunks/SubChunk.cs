using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Worlds.Chunks
{
    public class SubChunk
    {
        public const byte Version = 8;
        public SubChunkStorage[] Storages = [new SubChunkStorage()];
        public void SerializeTo(ProtocolMemoryWriter writer)
        {
            writer.Write(Version);
            writer.Write((byte)Storages.Length);
            //writer.Write(Index);
            foreach (SubChunkStorage storage in Storages) storage.SerializeTo(writer);
        }
        public bool IsEmpty => Storages[0].IsEmpty;
        public void SetBlock(byte x, byte y, byte z, int hash, byte index = 0) => Storages[index].SetBlock(x, y, z, hash);
        public void SetBlockRaw(byte x, byte y, byte z, ushort paletteIndex, byte index = 0) => Storages[index].SetBlockRaw(x, y, z, paletteIndex);
    }
}
