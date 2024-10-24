using ConMaster.Bedrock.Network;
using ConMaster.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Bedrock.Base
{

    public struct Vec3(float x, float y, float z) : INetworkType
    {
        public float X = x;
        public float Y = y;
        public float Z = z;
        public readonly float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        public static readonly Vec3 Zero = new(0, 0, 0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator +(Vec3 v1, Vec3 v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 v1, Vec3 v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator +(Vec3 v1, float n) => new(v1.X + n, v1.Y + n, v1.Z + n);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 v1, float n) => new(v1.X - n, v1.Y - n, v1.Z - n);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 v1) => new(-v1.X, -v1.Y, -v1.Z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator *(Vec3 v1, float s) => new(v1.X * s, v1.Y * s, v1.Z * s);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator /(Vec3 v1, float s) => new(v1.X / s, v1.Y / s, v1.Z / s);
        /// <summary>
        /// Vector Normalization
        /// </summary>
        /// <param name="v1">Vector parameter</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator ~(Vec3 v1) => v1 * (1f / v1.Length);

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public void Read(ProtocolMemoryReader reader)
        {
            X = reader.ReadFloat();
            Y = reader.ReadFloat();
            Z = reader.ReadFloat();
        }
        public readonly override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }
    }
}
