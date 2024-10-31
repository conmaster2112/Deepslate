using System.Buffers.Binary;
using System.Net;

namespace ConMaster.Raknet.Packets
{
    public struct OpenConnectionRequest2
    {
        public IPEndPoint? ServerAdress;
        //public int Cookie;
        //public bool ClientSupportsSecurity;
        public short MTU;
        public ulong ClientGuid;
        public const byte PacketId = 0x07;
        public int PACKET_SIZE => 27 + (ServerAdress?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 ? 29:7);

        public OpenConnectionRequest2 Deserialize(ReadOnlySpan<byte> buffer)
        {
            int size = Helper.ReadIpAddress(buffer.Slice(17), out ServerAdress);
            MTU = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(17 + size));
            ClientGuid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(19 + size));
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            if (ServerAdress == null) throw new NullReferenceException("Server address not specified");
            buffer[0] = PacketId;
            Helper.CopyMagicTo(buffer.Slice(1));
            int size = Helper.WriteIpAdress(buffer.Slice(17), ServerAdress);
            BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(17 + size), MTU);
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(19 + size), ClientGuid);
            return buffer;
        }
    }
    public struct OpenConnectionReply2
    {
        public ulong ServerGuid;
        public IPEndPoint? ClientAdress;
        public short MTU;
        public bool Encrypted;

        public const byte PacketId = 0x08;
        public int PACKET_SIZE => 28 + (ClientAdress?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 ? 29 : 7);
        public OpenConnectionReply2 Deserialize(ReadOnlySpan<byte> buffer)
        {
            ServerGuid = BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(17));
            int size = Helper.ReadIpAddress(buffer.Slice(25), out ClientAdress);
            MTU = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(25 + size));
            Encrypted = buffer[27 + size] > 0;
            return this;
        }
        public Span<byte> Serialize(Span<byte> buffer)
        {
            if (ClientAdress == null) throw new NullReferenceException("Adress can't be null.");
            buffer[0] = PacketId;
            Helper.CopyMagicTo(buffer.Slice(1));
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(17), ServerGuid);
            int size = Helper.WriteIpAdress(buffer.Slice(25), ClientAdress);
            BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(25 + size), MTU);
            buffer[27 + size] = (byte)(Encrypted ? 1 : 0);
            return buffer;
        }
    }
}
