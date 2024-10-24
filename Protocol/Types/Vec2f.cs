using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct Vec2f: INetworkType
    {
        public float X;
        public float Y;

        public void Read(ProtocolMemoryReader reader)
        {
            X = reader.ReadFloat();
            Y = reader.ReadFloat();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }
        public readonly override string ToString()
        {
            return $"<{X}, {Y}>";
        }
    }
}
