using System.Buffers.Binary;
using System.Net;

namespace ConMaster.Raknet.Packets
{

    public struct NewIncomingConnection
    {
        public IPEndPoint ServerAddress;
        //public IPEndPoint[] InternalIds;
        public long IncomingTime;
        public long ServerTime;
        public const byte PackedId = 0x13;
        public int PACKET_SIZE => 17 + (ServerAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? 7 : 29) + 20 * 7;
        public NewIncomingConnection Deserialize(ReadOnlySpan<byte> buffer)
        {
            int readed = Helper.ReadIpAddress(buffer.Slice(1), out ServerAddress);
            //SystemIndex = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(1 + readed));

            // Skip 9 random bytes, i really dont have idea what it is

            for (int i = 0; i < 20; i++)
            {
                readed += Helper.ReadIpAddress(buffer.Slice(1 + readed), out _);
            }
            IncomingTime = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(1 + readed));
            ServerTime = BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(9 + readed));
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            buffer[0] = PackedId;
            int writen = Helper.WriteIpAdress(buffer.Slice(1), ServerAddress);

            //Loopback required
            //writen += Helper.WriteIpAdress(buffer.Slice(3 + writen), LOOPBACK_ENDPOINT);

            //Span<byte> internalAddress = stackalloc byte[7];
            //Helper.WriteIpAdress(internalAddress, ANY_ENDPOINT);
            for (int i = 0; i < 20; i++) writen += Helper.WriteIpAdress(buffer.Slice(1 + writen), ConnectionRequestAccepted.ANY_ENDPOINT);
            //writen += 19 * internalAddress.Length;

            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(1 + writen), IncomingTime);
            BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(9 + writen), ServerTime);
            return buffer.Slice(0, 17 + writen);
        }
    }
}
