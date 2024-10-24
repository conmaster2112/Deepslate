using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConMaster.Compression
{
    public abstract class InternalCompressor
    {
        internal InternalCompressor(int _windowBits) {
            this._windowBits = _windowBits;
        }
        internal int _memoryLevel = ZLibInterop.Deflate_DefaultMemLevel;
        internal CompressionLevel _compressionLevel;
        internal System.IO.Compression.CompressionLevel _public_Level;
        internal int _windowBits;
        /// <summary>
        /// Compression Level
        /// </summary>
        public System.IO.Compression.CompressionLevel CompressionLevel { 
            get=>_public_Level;
            set
            {
                _public_Level = value;
                switch (value)
                {
                    case System.IO.Compression.CompressionLevel.Optimal:
                        _compressionLevel = Compression.CompressionLevel.DefaultCompression;
                        break;
                    case System.IO.Compression.CompressionLevel.Fastest:
                        _compressionLevel = Compression.CompressionLevel.BestSpeed;
                        break;
                    case System.IO.Compression.CompressionLevel.NoCompression:
                        _compressionLevel = Compression.CompressionLevel.NoCompression;
                        break;
                    case System.IO.Compression.CompressionLevel.SmallestSize:
                        _compressionLevel = Compression.CompressionLevel.BestCompression;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Memory Level 
        /// [1..9]
        /// 1 - Lowest memory usage, slow - low compression ratio
        /// 9 - Hight memory usage, fast - hight compression ratio
        /// </summary>
        public byte MemoryLevel { get => (byte)(_memoryLevel&0xff); set => _memoryLevel = value > 9 ? 9 : value; }

        /// <summary>
        /// Compression Algorithm
        /// </summary>
        /// <param name="source">Source buffer to operate compression on</param>
        /// <param name="destination">Target buffer to write compression results to</param>
        public abstract int Compress(ReadOnlySpan<byte> source, Span<byte> destination);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int byteWritten) => byteWritten = Compress(source, destination);
        public abstract int Decompress(ReadOnlySpan<byte> source, Span<byte> destination);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int byteWritten) => byteWritten = Decompress(source, destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Crc32(ReadOnlySpan<byte> source, uint token)
        {
            return ZLibInterop.CRC_32(token, ref MemoryMarshal.GetReference(source), source.Length);
        }

    }
    public class DeflateCompressor: InternalCompressor
    {
        public DeflateCompressor() : base(ZLibInterop.Deflate_DefaultWindowBits) { }
        internal DeflateCompressor(int windowBits) : base(windowBits) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Compress(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            return InternalDeflate(source, destination);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Decompress(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            return InternalInflate(source, destination);
        }
        internal unsafe int InternalDeflate(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            ZStream s = new(source, destination);
            if (
                    ZLibInterop.DeflateInit2_(
                        ref s, _compressionLevel, CompressionMethod.Deflated,
                        _windowBits, _memoryLevel, CompressionStrategy.DefaultStrategy
                    ) != ZLibErrorCode.Ok
                ) throw new Exception("Initialization Failed");

            if (ZLibInterop.Deflate(ref s, ZLibFlushCode.NoFlush) != ZLibErrorCode.Ok) throw new Exception("Unexpected error: " + Marshal.PtrToStringUTF8(s.msg));
            if ((int)ZLibInterop.Deflate(ref s, ZLibFlushCode.Finish) < 0) throw new Exception("Unexpected error: " + Marshal.PtrToStringUTF8(s.msg));
            if (ZLibInterop.DeflateEnd(ref s) != ZLibErrorCode.Ok) throw new Exception("Unexpected ending: " + Marshal.PtrToStringUTF8(s.msg));
            return destination.Length - (int)s.availOut;
        }
        internal unsafe int InternalInflate(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            ZStream s = new(source, destination);
            if (ZLibInterop.InflateInit2_(ref s, _windowBits) != ZLibErrorCode.Ok) 
                throw new Exception("Initialization Failed");

            ZLibErrorCode err = ZLibInterop.Inflate(ref s, ZLibFlushCode.NoFlush);
            switch (err)
            {
                case ZLibErrorCode.Ok:
                case ZLibErrorCode.StreamEnd:
                    break;
                case ZLibErrorCode.BufError:
                    throw new Exception("Output buffer is not large enought");
                default: throw new Exception("Unexpected error: " + Marshal.PtrToStringUTF8(s.msg));
            }
            if (ZLibInterop.InflateEnd(ref s) != ZLibErrorCode.Ok) throw new Exception("Unexpected ending: " + Marshal.PtrToStringUTF8(s.msg));
            return destination.Length - (int)s.availOut;
        }
    }
    public class ZLibCompressor(): DeflateCompressor(ZLibInterop.ZLib_DefaultWindowBits) { }
    public class GZipCompressor(): DeflateCompressor(ZLibInterop.GZip_DefaultWindowBits) { }
}
