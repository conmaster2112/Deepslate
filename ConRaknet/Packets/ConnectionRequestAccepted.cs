using System.Buffers.Binary;
using System.Net;

namespace ConMaster.Raknet.Packets
{

    public struct ConnectionRequestAccepted
    {
        public static IPEndPoint ANY_ENDPOINT = new(IPAddress.Any, 19138);
        public static IPEndPoint LOOPBACK_ENDPOINT = new(IPAddress.Loopback, 0); 
        public IPEndPoint ClientAddress;
        public short SystemIndex;
        //public IPEndPoint[] InternalIds;
        public long RequestTime;
        public long Time;
        public const byte PackedId = 0x10;
        public int PACKET_SIZE => 19 + (ClientAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork?7:29) + 20 * 7;
        public ConnectionRequestAccepted Deserialize(ReadOnlySpan<byte> buffer)
        {
            int readed = Helper.ReadIpAddress(buffer.Slice(1), out ClientAddress);
            SystemIndex = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(1 + readed));
            for (int i = 0; i < 20; i++) readed += Helper.ReadIpAddress(buffer.Slice(3 + readed), out ClientAddress);
            RequestTime = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(3 + readed));
            Time = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(11 + readed));
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PackedId;
            int writen = Helper.WriteIpAdress(buffer.Slice(1), ClientAddress);
            BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(1 + writen), SystemIndex);


            //Loopback required
            //writen += Helper.WriteIpAdress(buffer.Slice(3 + writen), LOOPBACK_ENDPOINT);

            //Span<byte> internalAddress = stackalloc byte[7];
            //Helper.WriteIpAdress(internalAddress, ANY_ENDPOINT);
            for (int i = 0; i < 20; i++) writen += Helper.WriteIpAdress(buffer.Slice(3 + writen), ANY_ENDPOINT);
            //writen += 19 * internalAddress.Length;

            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(3 + writen), RequestTime);
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(11 + writen), Time);
            return buffer.Slice(0, 19 + writen);
        }
    }
}
