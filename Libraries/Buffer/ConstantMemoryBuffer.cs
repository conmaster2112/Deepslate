using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Buffers
{
    public class ConstantMemoryBuffer(Memory<byte> buffer, int offset = 0)
    {
        internal int _Offset = offset;
        internal Memory<byte> _Buffer = buffer;
        public ConstantMemoryBufferReader Reader => new(_Buffer.Span, ref _Offset);
        public ConstantMemoryBufferWriter Writer => new(_Buffer.Span, ref _Offset);
        public int Offset => _Offset;
        public int Length => _Buffer.Length;
        public Memory<byte> Memory => _Buffer;
        public Span<byte> Span => _Buffer.Span;
    }
}