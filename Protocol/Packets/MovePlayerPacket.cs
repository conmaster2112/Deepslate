using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Protocol.Enums;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class MovePlayerPacket : BasePacket<MovePlayerPacket>
    {
        public const int PACKET_ID = 19;
        public override int Id => PACKET_ID;

        public ulong ActorRuntimeId;
        public Vec3f Position;
        public Vec2f Rotation;
        public float YHeadRotation;
        public MoveMode PositionMode;
        public bool IsOnGround;
        public ulong RidingActorRuntimeId;
        public int TeleportCause;
        public int SourceActorType;
        public ulong Tick;
        public override void Clean()
        {
            ActorRuntimeId = default;
            Position = default;
            Rotation = default;
            YHeadRotation = default;
            PositionMode = default;
            IsOnGround = default;
            RidingActorRuntimeId = default;
            TeleportCause = default;
            SourceActorType = default;
            Tick = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            ActorRuntimeId = reader.ReadUnsignedVarLong();
            reader.Read(ref Position);
            reader.Read(ref Rotation);
            YHeadRotation = reader.ReadFloat();
            PositionMode = NetworkEnums.ReadMoveMode(reader);
            IsOnGround = reader.ReadBool();
            RidingActorRuntimeId = reader.ReadUnsignedVarLong();
            if(PositionMode == MoveMode.Teleport)
            {
                TeleportCause = reader.ReadInt32();
                SourceActorType = reader.ReadInt32();
            }
            Tick = reader.ReadUnsignedVarLong();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarLong(ActorRuntimeId);
            writer.Write(Position);
            writer.Write(Rotation); 
            writer.Write(YHeadRotation);
            NetworkEnums.Write(writer ,PositionMode);
            writer.Write(IsOnGround);
            writer.WriteUnsignedVarLong(RidingActorRuntimeId);
            if(PositionMode == MoveMode.Teleport)
            {
                writer.Write(TeleportCause);
                writer.Write(SourceActorType);
            }
            writer.WriteUnsignedVarLong(Tick);
        }
    }
}
