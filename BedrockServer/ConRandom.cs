using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesing
{

    public class CustomRandom(uint seed)
    {
        const uint BIT_NOISE1 = 0x68E31DA4; // Bunch of random bits
        const uint BIT_NOISE2 = 0xB5297A4D; // Bunch of random bits
        const uint BIT_NOISE3 = 0x1B56C4E9; // Bunch of random bits
        public volatile uint Index = uint.MaxValue;
        public readonly uint Seed = seed;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Next()
        {
            uint int32 = (Index++ * BIT_NOISE1 + Seed);
            int32 ^= (int32 >> 8);
            int32 += BIT_NOISE2;
            int32 ^= (int32 << 8);
            int32 *= BIT_NOISE3;
            int32 ^= (int32 >> 8);
            return (int)(int32 & 0x7fff_ffff);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double NextDouble()
        {
            return Next() * 4.656612875245797e-10;
        }
    }
}
