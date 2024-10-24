using ConMaster.Buffers;
using System.Text;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct ClientTokenData : INetworkType
    {
        //Json Payload => {chain:["JWTString"]}
        public byte[] IdentityDataUtf8;
        //Raw Jwt payload
        public byte[] ClientDataUtf8;

        public void Read(ProtocolMemoryReader reader)
        {
            reader.ReadVarLength(); //PaloadSize in general
            IdentityDataUtf8 = reader.ReadSlice(reader.ReadInt32()).ToArray();
            ClientDataUtf8 = reader.ReadSlice(reader.ReadInt32()).ToArray();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarLength((IdentityDataUtf8.Length + ClientDataUtf8.Length + 8));
            writer.WriteString32Raw(IdentityDataUtf8);
            writer.WriteString32Raw(ClientDataUtf8);
        }
    }
}
