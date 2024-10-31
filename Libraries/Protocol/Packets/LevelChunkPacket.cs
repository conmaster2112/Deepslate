using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Buffers;
using ConMaster.Deepslate.Network;


namespace ConMaster.Deepslate.Protocol.Packets
{
    public class LevelChunkPacket: BasePacket<LevelChunkPacket>
    {
        public const int PacketId = 58;

        public override int Id => PacketId;
        public Vec2ChunkPosition ChunkPosition;
        public int NetworkDimensionId;
        public int SubChunksCount = 0;
        //public bool CacheEnabled;
        public Memory<byte> Chunk;
        public override void Clean()
        {
            ChunkPosition = default;
            NetworkDimensionId = default;
            //CacheEnabled = default;
            Chunk = null;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            throw new NotImplementedException();
            /*
            X = reader.ReadSignedVarInt();
            Z = reader.ReadSignedVarInt();
            NetworkDimensionId = reader.ReadSignedVarInt();
            Chunk.SubChunks.Length;*/
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(ref ChunkPosition);
            writer.WriteSignedVarInt(NetworkDimensionId);
            writer.WriteUnsignedVarInt((uint)SubChunksCount);
            writer.Write(false); //Cache Enabled
            writer.WriteUnsignedVarInt((uint)Chunk.Length);
            writer.Writer.Write(Chunk.Span);
        }
    }
}
