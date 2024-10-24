using System.Runtime.CompilerServices;

namespace ConMaster.Bedrock.Base
{
    public struct BlockLocation(int x, int y, int z): INetworkType
    {
        public int X = x;
        public int Y = y;
        public int Z = z;
        public static readonly BlockLocation Zero = new(0, 0, 0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BlockLocation operator +(BlockLocation v1, BlockLocation v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BlockLocation operator -(BlockLocation v1, BlockLocation v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        public readonly override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }
    }
}
