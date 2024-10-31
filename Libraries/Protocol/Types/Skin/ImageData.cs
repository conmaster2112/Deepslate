using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.Skin
{
    public struct ImageData() : INetworkType
    {
        public static readonly ImageData Empty = new();
        public uint Width = 0;
        public uint Height = 0;
        public string Data64 = string.Empty;
        public void Read(ProtocolMemoryReader reader)
        {
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            Data64 = reader.ReadVarString();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(Width);
            writer.Write(Height);
            writer.WriteVarString(Data64);
        }
    }
}
