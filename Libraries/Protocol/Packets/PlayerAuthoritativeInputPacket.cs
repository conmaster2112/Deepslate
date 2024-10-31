using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Types;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class PlayerAuthoritativeInputPacket: BasePacket<PlayerAuthoritativeInputPacket>
    {

        public const int PACKET_ID = 144;
        public override int Id => PACKET_ID;
        public Vec2f Rotation;
        public Vec3f Location;
        public Vec2f MoveRotation;
        public float HeadRotation;
        public ulong InputData;
        public uint InputMode;
        public uint PlayerMode;
        public uint NewInteractionModel;

        //VR Gaze idk

        public ulong CurrentTick;
        public Vec3f Delta;

        public Vec2f AnalogMoveVector;

        public override void Clean()
        {
            Rotation = default;
            Location = default;
            MoveRotation = default;
            HeadRotation = default;
            InputData = default;
            InputMode = default;
            PlayerMode = default;
            NewInteractionModel = default;
            CurrentTick = default;
            Delta = default;
            AnalogMoveVector = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            reader.Read(ref Rotation);
            reader.Read(ref Location);
            reader.Read(ref MoveRotation);
            HeadRotation = reader.ReadFloat();
            InputData = reader.ReadUnsignedVarLong();
            InputMode = reader.ReadUnsignedVarInt();
            PlayerMode = reader.ReadUnsignedVarInt();
            NewInteractionModel = reader.ReadUnsignedVarInt();


            CurrentTick = reader.ReadUnsignedVarLong();
            reader.Read(ref Delta);
            reader.Read(ref AnalogMoveVector);
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
