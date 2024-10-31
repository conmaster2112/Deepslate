using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Raknet.Packets
{
    public struct ConnectedPing
    {
        public long Time;
        public const byte PacketId = 0x0;
        public const byte PACKET_SIZE = 9;
        public readonly int PacketSize => PACKET_SIZE;
        public static ReadOnlySpan<byte> WriteTo(Span<byte> data, long time)
        {
            data[0] = PacketId;
            BinaryPrimitives.WriteInt64BigEndian(data.Slice(1), time);
            return data.Slice(0, PACKET_SIZE);
        }
        public ConnectedPing Deserialize(ReadOnlySpan<byte> buffer)
        {
            Time = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(1));
            return this;
        }
        public readonly Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PacketId;
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(1), Time);
            return buffer.Slice(0, PACKET_SIZE);
        }
    }
    public struct ConnectedPong
    {
        public long PingTime;
        public long PongTime;

        public const byte PacketId = 0x03;
        public ConnectedPong Deserialize(ReadOnlySpan<byte> buffer)
        {
            PingTime = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(1));
            PongTime = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(9));
            return this;
        }

        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PacketId;
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(1), PingTime);
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(9), PongTime);
            return buffer.Slice(0, PACKET_SIZE);
        }
        public int PACKET_SIZE => 17;
    }
}
