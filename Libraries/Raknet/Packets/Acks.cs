using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConMaster.Raknet
{
    public static class AckPacket
    {
        public const byte AckPackedId = 0xc0;
        public const byte NackPackedId = 0xa0;
        public static int GetPacketSize(ReadOnlySpan<AckRecord> records) => 3 + records.Length * AckRecord.RECORD_SIZE_CONST;
        public static ushort GetCountOfRecords(ReadOnlySpan<byte> buffer) => BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(1));
        public static void Deserialize(ReadOnlySpan<byte> buffer, Span<AckRecord> records)
        {
            int offset = 0;
            for(int i = 0; i < records.Length; i++) records[i] = records[i].Deserialize(buffer.Slice(3 + offset), ref offset);
        }
        public static AckRecordEnumerable GetAckEnumerator(ReadOnlySpan<byte> bytes) => new AckRecordEnumerable(bytes, GetCountOfRecords(bytes));
        public static Span<byte> Serialize(Span<byte> buffer, ReadOnlySpan<AckRecord> records, byte packetId = AckPackedId)
        {
            buffer[0] = packetId;
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(1), (ushort)records.Length);
            for (int i = 0; i < records.Length; i++) records[i].Serialize(buffer.Slice(3 + i * AckRecord.RECORD_SIZE_CONST));
            return buffer;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct AckRecord
    {
        public const int RECORD_SIZE_CONST = 4 + 3;
        //public bool IsRange;
        public int Low;
        public int High;
        public AckRecord Deserialize(ReadOnlySpan<byte> buffer, ref int offset)
        {
            bool IsNotRange = buffer[0] != 0;
            Low = Helper.ReadUInt24LE(buffer.Slice(1));
            offset += 4;
            if (!IsNotRange)
            {
                offset += 3;
                High = Helper.ReadUInt24LE(buffer.Slice(4));
            }
            else High = Low;
            return this;
        }

        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = 0; //IsNotRange false
            Helper.WriteUInt24LE(buffer.Slice(1), Low);
            Helper.WriteUInt24LE(buffer.Slice(4), High);
            return buffer.Slice(0,RECORD_SIZE_CONST);
        }
    }

    public ref struct AckRecordEnumerable(ReadOnlySpan<byte> buffer, int count)
    {
        public int Offset = 0;
        public int Index = -1;
        public int Count = count;
        internal AckRecord _current = default;
        public ReadOnlySpan<byte> Source = buffer;

        /// <summary>Advances the enumerator to the next element of the span.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            int index = Index + 1;
            if (index < Count)
            {
                _current.Deserialize(Source.Slice(3 + Offset), ref Offset);
                Index = index;
                return true;
            }

            return false;
        }
        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public readonly AckRecord Current => _current;
        public readonly AckRecordEnumerable GetEnumerator() => this;
    }
}
