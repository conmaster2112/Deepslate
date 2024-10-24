using ConMaster.Deepslate.Network;
using System.Runtime.CompilerServices;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct Vec2ChunkPosition(int x, int z) : INetworkType
    {
        public int X = x;
        public int Z = z;
        public static readonly Vec2ChunkPosition Zero = new(0, 0);
        public readonly int LengthPower => X*X + Z*Z;
        public void Read(ProtocolMemoryReader reader)
        {
            X = reader.ReadSignedVarInt();
            Z = reader.ReadSignedVarInt();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(X);
            writer.WriteSignedVarInt(Z);
        }
        public override string ToString()
        {
            return $"<{X} {Z}>";
        }
        public readonly override int GetHashCode() => HashCode.Combine(X, Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vec2ChunkPosition p1, Vec2ChunkPosition p2) => p1.X == p2.X && p1.Z == p2.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vec2ChunkPosition p1, Vec2ChunkPosition p2) => p1.X != p2.X || p1.Z != p2.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2ChunkPosition operator +(Vec2ChunkPosition v1, Vec2ChunkPosition v2) => new(v1.X + v2.X, v1.Z + v2.Z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2ChunkPosition operator -(Vec2ChunkPosition v1, Vec2ChunkPosition v2) => new(v1.X - v2.X, v1.Z - v2.Z);

        public readonly override bool Equals(object? obj)
        {
            if (obj is Vec2ChunkPosition c) return c == this;
            return false;
        }
    }
}
