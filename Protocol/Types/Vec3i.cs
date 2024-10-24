using ConMaster.Deepslate.Network;
using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct Vec3i(int x, int y, int z): INetworkType
    {
        public int X = x;
        public int Y = y;
        public int Z = z;
        public static readonly Vec3i Zero = new(0, 0, 0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3i operator +(Vec3i v1, Vec3i v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3i operator -(Vec3i v1, Vec3i v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public void Read(ProtocolMemoryReader reader)
        {
            X = reader.ReadSignedVarInt();
            Y = (int)reader.ReadUnsignedVarInt();
            Z = reader.ReadSignedVarInt();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(X);
            writer.WriteUnsignedVarInt((uint)Y);
            writer.WriteSignedVarInt(Z);
        }
        public readonly override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }
    }
}
