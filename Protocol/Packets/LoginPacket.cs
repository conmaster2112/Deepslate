using ConMaster.Deepslate.Protocol.Types;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class LoginPacket : BasePacket<LoginPacket>
    {
        public const int PACKET_ID = 1;
        public override int Id => PACKET_ID;
        public int ClientNetworkVersion = 0;
        public ClientTokenData Data = default;
        public override void Clean() // Struct types doesn't need release
        {
            ClientNetworkVersion = 0;
            Data = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            ClientNetworkVersion = reader.ReadInt32BE();
            reader.Read(ref Data);
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteBE(ClientNetworkVersion);
            writer.Write(Data);
            return;
        }
    }
}
