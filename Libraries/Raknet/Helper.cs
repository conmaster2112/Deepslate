using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConMaster.Raknet
{
    public static class Helper
    {
        public static readonly byte[] MAGIC = [0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78];
        public const int MAGIC_LENGTH = 16; [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt24LE(Span<byte> buffer, int value)
        {
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            return 3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteUInt24BE(Span<byte> buffer, int value)
        {
            buffer[2] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[0] = (byte)(value >> 16);
            return 3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24LE(ReadOnlySpan<byte> buffer)
        {
            int value = 0;
            value |= buffer[0];
            value |= buffer[1] << 8;
            value |= buffer[2] << 16;
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUInt24BE(ReadOnlySpan<byte> buffer)
        {
            int value = 0;
            value |= buffer[2];
            value |= buffer[1] << 8;
            value |= buffer[0] << 16;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMagicTo(Span<byte> buffer)=>MAGIC.AsSpan().CopyTo(buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString16(Span<byte> buffer, ReadOnlySpan<char> value, Encoding? data = null)
        {
            int writen = (data??Encoding.UTF8).GetBytes(value, buffer.Slice(2));
            BinaryPrimitives.WriteUInt16BigEndian(buffer, (ushort)writen);
            return writen + 2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString16(ReadOnlySpan<byte> buffer, Encoding? data = null)
        {
            return (data ?? Encoding.UTF8).GetString(buffer.Slice(2, BinaryPrimitives.ReadUInt16BigEndian(buffer)));
        }
        public static int ReadIpAddress(ReadOnlySpan<byte> buffer, out IPEndPoint address)
        {
            switch(buffer[0])
            {
                case 4:
                    address = new IPEndPoint(new IPAddress(buffer.Slice(1,4)), BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(5)));
                    return 7;
                case 6:
                    // +2 AdressFamily
                    ushort port = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(3));
                    // +4 Flow info

                    uint scopeId = BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(25));
                    Console.WriteLine(scopeId + "");
                    IPAddress ipAddress = new(buffer.Slice(9, 16), scopeId);
                    address = new IPEndPoint(ipAddress, port);
                    return 1 + 2 + 2 + 4 + 16 + 4;
                default:
                    throw new NotImplementedException("Usupported IP protocol: " + buffer[0]);
                    //_ = reader.ReadUInt16(useLittleEndian); //Adress Family but it always should be InterNetworkV6 
                    //ushort port = reader.ReadUInt16(useLittleEndian);
                    //Span<byte> flowData = ReadBuffer(4);
                    
                    //IPAddress iPAddress = new(ReadBuffer(16), reader.ReadUInt32(useLittleEndian));
                    //return new IPEndPoint(iPAddress, port);
            }
        }
        public static int WriteIpAdress(Span<byte> buffer, IPEndPoint address)
        {
            byte ipVersion = (byte)(address.Address.AddressFamily == AddressFamily.InterNetwork ? 4 : 6);
            buffer[0] = ipVersion;
            switch (ipVersion)
            {
                case 4:
                    address.Address.GetAddressBytes().AsSpan().CopyTo(buffer.Slice(1));
                    BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(5), (ushort)address.Port);
                    break;
                case 6:
                    throw new NotImplementedException("IPv6 address type is not supported at the moment.");
                    //We dont have enought information about required data such as FlowInfo

                    /*WriteUInt16((ushort)AddressFamily.InterNetworkV6, false); //Adress Family but it always should be InterNetworkV6 
                    WriteUInt16((ushort)address.Port, false);
                    Seek(4); //Flow Info idk what is it
                    //IPAddress iPAddress = new(ReadBuffer(16), ReadUInt32(false));
                    //return new IPEndPoint(iPAddress, port);
                    break;*/
            }
            return 7;
        }
    }
}
