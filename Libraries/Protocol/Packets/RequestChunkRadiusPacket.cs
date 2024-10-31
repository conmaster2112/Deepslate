using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class RequestChunkRadiusPacket: BasePacket<RequestChunkRadiusPacket>
    {
        public const int PACKET_ID = 69;

        public override int Id => PACKET_ID;
        public int ChunkRadius;
        public byte MaxChunkRadius;

        public override void Clean()
        {
            ChunkRadius = default;
            MaxChunkRadius = default;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            ChunkRadius = reader.ReadSignedVarInt();
            MaxChunkRadius = reader.ReadUInt8();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(ChunkRadius);
            writer.Write(MaxChunkRadius);
        }
    }
    public class UpdateChunkRadiusPacket : BasePacket<UpdateChunkRadiusPacket>
    {
        public const int PACKET_ID = 70;

        public override int Id => PACKET_ID;
        public int ChunkRadius;

        public override void Clean()
        {
            ChunkRadius = default;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            ChunkRadius = reader.ReadSignedVarInt();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(ChunkRadius);
        }
    }
}
