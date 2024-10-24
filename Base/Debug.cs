using System.Text;

namespace ConMaster
{
#if DEBUG
    public static class Debug
    {
        public static string ToString(ReadOnlySpan<byte> bytes, byte separator = (byte)' ')
        {
            int length = bytes.Length;
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            ReadOnlySpan<byte> characters = "0123456789ABCDEF"u8;

            // (int.MaxValue / 3) == 715,827,882 Bytes == 699 MB
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, int.MaxValue / 3);

            Span<byte> dst = stackalloc byte[length * 3 - 1];
            var src = bytes;
            int i = 0;
            int j = 0;
            byte b = src[i++];
            dst[j++] = characters[(b >> 4) & 0xf];
            dst[j++] = characters[b & 0xf];
            while (i < src.Length)
            {
                b = src[i++];
                dst[j++] = separator;
                dst[j++] = characters[(b >> 4) & 0xf];
                dst[j++] = characters[b & 0xf]; ;
            }

            return Encoding.ASCII.GetString(dst);
        }
    }
#endif
}
