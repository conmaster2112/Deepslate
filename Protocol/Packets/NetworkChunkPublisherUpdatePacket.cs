using ConMaster.Deepslate.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class NetworkChunkPublisherUpdatePacket: BasePacket<NetworkChunkPublisherUpdatePacket>
    {
        public const int PACKET_ID = 121;

        public override int Id => PACKET_ID;

        public  Vec3i  ViewPosition;
        public int NewRadiusOfView;
        public IReadOnlyCollection<Vec2ChunkPosition> ChunkPositions = [];

        public override void Clean()
        {
            ViewPosition = default;
            NewRadiusOfView = default;
            ChunkPositions = [];
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            throw new NotImplementedException();/*
            reader.Read(ref  ViewPosition);
            NewRadiusOfView = (int)reader.ReadUnsignedVarInt();
            ChunkPositions = reader.ReadVarArray<ChunkPosition>();*/
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(ref ViewPosition);
            writer.WriteUnsignedVarInt((uint)NewRadiusOfView);
            //writer.Write(ChunkPositions.Count);
            writer.WriteArray32<Vec2ChunkPosition>(ChunkPositions);
        }
    }
}
