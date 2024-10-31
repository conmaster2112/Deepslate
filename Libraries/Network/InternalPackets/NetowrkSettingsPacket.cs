namespace ConMaster.Deepslate.Network.InternalPackets
{
    public struct NetworkSettingsPacketInternal() : IPacket, INetworkType
    {
        public const int PACKET_ID = 143;
        public readonly int Id => PACKET_ID;
        public ushort CompressionThreshold = 1;
        public ushort CompressionAlgorithm = 0;
        public bool ClientThrottleEnabled = false;
        public byte ClientThrottleThreshold = 0;
        public float ClientThrottleScalar = 0;
        public void Read(ProtocolMemoryReader reader)
        {
            CompressionThreshold = reader.ReadUInt16();
            CompressionAlgorithm = reader.ReadUInt16();
            ClientThrottleEnabled = reader.ReadBool();
            ClientThrottleThreshold = reader.ReadUInt8();
            ClientThrottleScalar = reader.ReadFloat();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
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
