
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Deepslate.Worlds.Chunks
{
    public class Chunk
    {
        public Vec2ChunkPosition Position { get; protected init; }
        public readonly int Y_OFFSET = 64; //Positive bc its faster for addition
        public Chunk(Vec2ChunkPosition pos)
        {
            Position = pos;
            SubChunks = new SubChunk[24];
            for (int i = 0; i < SubChunks.Length; i++)
            {
                SubChunks[i] = new SubChunk();
            }
        }
        public SubChunk[] SubChunks;
        public void SetBlock(byte x, int y, byte z, int hash, byte index = 0)
        {
            y += Y_OFFSET;
            SubChunks[y >> 4].SetBlock(x, (byte)(y & 0xf), z, hash, index);
        }

        public int GetSubChunksCount()
        {
            int count = SubChunks.Length;
            for (int i = SubChunks.Length - 1; i >= 0; i--, count--)
            {
                if (!SubChunks[i].IsEmpty) break;
            }
            return count;
        }
        public void SerializeTo(ProtocolMemoryWriter writer)
        {

            int count = GetSubChunksCount();
            for (int i = 0; i<= count; i++)
                SubChunks[i].SerializeTo(writer);


            //Biomes?
            for (int index = 0; index < 24; index++)
            {
                writer.Write((byte)0);
                writer.WriteUnsignedVarInt(2);
            }

            //? border block
            writer.Write((byte)0);
        }
        public override int GetHashCode() => Position.GetHashCode();
    }
}
