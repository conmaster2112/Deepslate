using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Raknet
{
    public struct ConnectionRequest
    {
        public ulong ClientGuid;
        public long Time;
        public bool UseSecurity;
        public const byte PackedId = 0x09;
        public const byte PACKET_SIZE = 18;

        public ConnectionRequest Deserialize(ReadOnlySpan<byte> buffer)
        {
            ClientGuid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(1));
            Time = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(9));
            UseSecurity = buffer[17] != 0;
            return this;
        }

        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PackedId;
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(1), ClientGuid);
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(9), Time);
            buffer[17] = (byte)(UseSecurity ? 1 : 0);
            return buffer.Slice(0, PACKET_SIZE);
        }
    }
}
