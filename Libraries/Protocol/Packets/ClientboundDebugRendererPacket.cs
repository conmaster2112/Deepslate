using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class ClientboundDebugRendererPacket: BasePacket<ClientboundDebugRendererPacket>
    {
        public const int PACKET_ID = 164;
        public override int Id => PACKET_ID;
        public uint Type;
        public DebugMarkerOptions MarkerOptions;
        public override void Clean()
        {
            Type = 0;
            MarkerOptions = default;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            Type = reader.ReadUInt32();
            if (Type == 2) reader.Read(ref MarkerOptions);
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(Type);
            if(Type == 2) writer.Write(ref MarkerOptions);
        }
    }
    public struct DebugMarkerOptions: INetworkType
    {
        public string Text;
        public Vec3f Position;
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;
        public ulong DurationInMilliseconds;

        public void Read(ProtocolMemoryReader reader)
        {
            Text = reader.ReadVarString();
            reader.Read(ref Position);
            Red = reader.ReadFloat();
            Green = reader.ReadFloat();
            Blue = reader.ReadFloat();
            Alpha = reader.ReadFloat();
            DurationInMilliseconds = reader.ReadUInt64();
        }

        public void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Text);
            writer.Write(ref Position);
            writer.Write(Red);
            writer.Write(Green);
            writer.Write(Blue);
            writer.Write(Alpha);
            writer.Write(DurationInMilliseconds);
        }
    }
}
