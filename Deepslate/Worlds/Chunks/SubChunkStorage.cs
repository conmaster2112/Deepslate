using ConMaster.Deepslate.Network;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.Worlds.Chunks
{
    public class SubChunkStorage 
    {
        public const int MAX_SIZE = 16;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndex(byte x, byte y, byte z) => ((x & 0xf) << 8) | ((z & 0xf) << 4) | (y & 0xf);
        public List<int> Palette = [-604749536];// 507450469, -2108756090 //507450469]; //11_881// -1324488512
        //public ushort[] Pallete = new ushort[255];
        public ushort[] RawBlockIndexes = new ushort[MAX_SIZE * MAX_SIZE * MAX_SIZE];
        public void SerializeTo(ProtocolMemoryWriter writer)
        {
            int bitsPerBlock = Palette.Count > 0 ? 32 - BitOperations.LeadingZeroCount((uint)(Palette.Count - 1)) : 0;

            // Add padding to the bits per block if needed.
            switch (bitsPerBlock)
            {
                case 0:
                    bitsPerBlock = 1;
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    break;
                case 7:
                case 8:
                    bitsPerBlock = 8;
                    break;
                default:
                    bitsPerBlock = 16;
                    break;
            }

            // First bit should indicate if block is represented as hash or network id, 
            // but it doesn't works if this doesn't machets settings sended by StartGamePacket

            //Write number of bits per each word (int32)
            writer.Write((byte)(bitsPerBlock << 1 | 1));

            int blocksPerWord = 32 / bitsPerBlock; //Word is meaning fow int32
            int wordsPerStorage = RawBlockIndexes.Length / blocksPerWord;

            // Write the word to the stream.
            int position = 0;
            for (int w = 0; w < wordsPerStorage; w++)
            {
                uint word = 0;

                for (int block = 0; block < blocksPerWord; block++)
                    word |= (uint)RawBlockIndexes[position++] << (bitsPerBlock * block);

                writer.Write(word);
            }
            // Write the palette length.
            // And each runtime ID in the palette.
            writer.WriteSignedVarInt(Palette.Count);
            for (int i = 0; i < Palette.Count; i++) writer.WriteSignedVarInt(Palette[i]);
        }
        public SubChunkStorage()
        {

        }
        public ushort GetPaletteIndexPreFetch(int hash)
        {
            int index = Palette.IndexOf(hash);
            if (index == -1) {
                index = Palette.Count;
                Palette.Add(hash);
            }
            return (ushort)index;
        }
        public void SetBlock(byte x, byte y, byte z, int hash)=> RawBlockIndexes[GetIndex(x, y, z)] = GetPaletteIndexPreFetch(hash);
        public void SetBlockRaw(byte x, byte y, byte z, ushort index)=> RawBlockIndexes[GetIndex(x, y, z)] = index;
        public bool IsEmpty => Palette.Count <= 1;
    }
}
