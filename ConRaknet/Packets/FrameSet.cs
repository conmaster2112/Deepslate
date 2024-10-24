using System.Buffers.Binary;

namespace ConMaster.Raknet.Packets
{
    public static class FrameSetHeader
    {
        public const byte PackedId = 0x80;
        public const byte HEADER_SIZE = 4;
        public static int ReadSequenceIndex(ReadOnlySpan<byte> buffer)=>Helper.ReadUInt24LE(buffer.Slice(1));
        public static Span<byte> SetHeader(Span<byte> buffer, int sequenceIndex)
        {
            buffer[0] = PackedId;
            Helper.WriteUInt24LE(buffer.Slice(1), sequenceIndex);
            return buffer.Slice(4);
        }
        public static FrameInfo ReadFrameInfo(ReadOnlySpan<byte> buffer, out int bytesReaded)
        {
            FrameInfo frameInfo = new FrameInfo();
            bytesReaded = frameInfo.Deserialize(buffer);
            return frameInfo;
        }
        public static int WriteFrame(Span<byte> buffer, FrameInfo frameInfo, ReadOnlySpan<byte> data) {
            frameInfo.BodyLength = data.Length;
            int writen = frameInfo.Serialize(buffer);
            data.CopyTo(buffer.Slice(writen));
            return writen + data.Length;
        }
    }
    public struct FrameInfo
    {
        public static readonly FrameInfo ReliableOrdered = new() { Reliability = FrameReliability.ReliableOrdered };
        public static readonly FrameInfo None = new() { Reliability = FrameReliability.Unreliable };
        public const int MAX_SIZE = 23; // 3 + 3 + 3 + 4 + 10
        public static bool GetIsOrdered(FrameReliability Reliability) =>
            (Reliability == FrameReliability.UnreliableSequenced) ||
            (Reliability == FrameReliability.ReliableOrdered) ||
            (Reliability == FrameReliability.ReliableSequenced) ||
            (Reliability == FrameReliability.ReliableOrderedWithAckReceipt);
        public static bool GetIsReliable(FrameReliability Reliability) => 
            (Reliability == FrameReliability.Reliable) || 
            (Reliability == FrameReliability.ReliableOrdered) || 
            (Reliability == FrameReliability.ReliableSequenced);
        public static bool GetIsOrderedExclusive(FrameReliability Reliability) =>
            (Reliability == FrameReliability.ReliableOrdered) ||
            (Reliability == FrameReliability.ReliableOrderedWithAckReceipt);
        public static bool GetIsSequenced(FrameReliability Reliability) => 
            (Reliability == FrameReliability.UnreliableSequenced) || 
            (Reliability == FrameReliability.ReliableSequenced);

        public byte RawFlag;
        public FrameReliability Reliability;
        public bool IsFragmented
        {
            readonly get => (RawFlag & 0x10) == 0x10;
            set
            {
                if (value) RawFlag |= 0x10;
                else RawFlag = (byte)(RawFlag & (~0x10));
            }
        }

        public int ReliableFrameIndex;
        public int SequencedFrameIndex;

        public int OrderFrameIndex;
        public byte OrderChannel;
        public int FragmentSize;
        public short FragmentCompoudId;
        public int FragmentIndex;
        public int BodyLength;
        public int Deserialize(ReadOnlySpan<byte> buffer)
        {
            RawFlag = buffer[0];
            byte rl = (byte)(RawFlag >> 5);
            //if (rl >= 5) rl -= 5;
            Reliability = (FrameReliability)rl;
            BodyLength = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1)) >> 3;
            int offset = 3;
            if (GetIsReliable(Reliability))
            {
                ReliableFrameIndex = Helper.ReadUInt24LE(buffer.Slice(offset));
                offset += 3;
            }
            if (GetIsSequenced(Reliability))
            {
                SequencedFrameIndex = Helper.ReadUInt24LE(buffer.Slice(offset));
                offset += 3;
            }
            if (GetIsOrdered(Reliability))
            {
                OrderFrameIndex = Helper.ReadUInt24LE(buffer.Slice(offset));
                offset += 3;
                OrderChannel = buffer[offset++];
            }
            if (IsFragmented)
            {
                FragmentSize = BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(offset));
                offset += 4;
                FragmentCompoudId = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(offset));
                offset += 2;
                FragmentIndex = BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(offset));
                offset += 4;
            }
            return offset;
        }

        public readonly int FRAME_INFO_SIZE
        {
            get
            {
                int length = 3;
                if (GetIsReliable(Reliability)) length += 3;
                if (GetIsSequenced(Reliability)) length += 3;
                if (GetIsOrdered(Reliability)) length += 4;
                if (IsFragmented) length += 10;
                return length;
            }
        }

        public readonly int Serialize(Span<byte> buffer)
        {
            buffer[0] = (byte)(IsFragmented?(0x4 | ((int)Reliability << 5)):((int)Reliability << 5)); // ((byte)(RawFlag & ((byte)Reliability << 5)));
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1), (ushort)(BodyLength << 3));

            int offset = 3;
            if (GetIsReliable(Reliability))
            {
                Helper.WriteUInt24LE(buffer.Slice(offset), ReliableFrameIndex);
                offset += 3;

            }
            if (GetIsSequenced(Reliability))
            {
                Helper.WriteUInt24LE(buffer.Slice(offset), SequencedFrameIndex);
                offset += 3;
            }
            if (GetIsOrdered(Reliability))
            {
                Helper.WriteUInt24LE(buffer.Slice(offset), OrderFrameIndex);
                offset += 3;
                buffer[offset++] = OrderChannel;
            }
            if (IsFragmented)
            {
                BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(offset), FragmentSize);
                offset += 4;
                BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(offset), FragmentCompoudId);
                offset += 2;
                BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(offset), FragmentIndex);
                offset += 4;
            }
            return offset;
        }
    }
}
