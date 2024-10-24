using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class NetworkSettingsPacket : BasePacket<NetworkSettingsPacket>
    {
        public const int PACKET_ID = 143;
        public override int Id => PACKET_ID;
        public ushort CompressionThreshold = 256;
        public ushort CompressionAlgorithm = 0;
        public bool ClientThrottleEnabled = false;
        public byte ClientThrottleThreshold = 0;
        public float ClientThrottleScalar = 0;
        public override void Clean() //Reset packet to default values
        {
            CompressionThreshold = 256;
            CompressionAlgorithm = 0;
            ClientThrottleEnabled = false;
            ClientThrottleThreshold = 0;
            ClientThrottleScalar = 0;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            CompressionThreshold = reader.ReadUInt16();
            CompressionAlgorithm = reader.ReadUInt16();
            ClientThrottleEnabled = reader.ReadBool();
            ClientThrottleThreshold = reader.ReadUInt8();
            ClientThrottleScalar = reader.ReadFloat();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(CompressionThreshold);
            writer.Write(CompressionAlgorithm);
            writer.Write(ClientThrottleEnabled);
            writer.Write(ClientThrottleThreshold);
            writer.Write(ClientThrottleScalar);
            return;
        }
    }
}
