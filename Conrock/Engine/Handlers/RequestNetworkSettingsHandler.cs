using ConMaster.Bedrock.Network;
using ConMaster.Bedrock.Packets;

namespace ConMaster.Bedrock.Engine.Handlers
{
    internal static class RequestNetworkSettingsHandler
    {
        public static ProtocolPacketHandler<RequestNetworkSettingsPacket> GetHandler()
        {
            return new(Handler, RequestNetworkSettingsPacket.PACKET_ID);
        }
        public static void Handler(Client client, RequestNetworkSettingsPacket packet)
        {
            if (client.Server.Protocol.ProtocolVersion > packet.ProtocolVersion)
            {
                client.Disconnect("Outdated client protocol version: " + packet.ProtocolVersion + " expected: " + client.Server.Protocol.ProtocolVersion);
                return;
            }
            if (client.Server.Protocol.ProtocolVersion < packet.ProtocolVersion)
            {
                client.Disconnect("We do not support this version of protocol yet, client: " + packet.ProtocolVersion + " expected: " + client.Server.Protocol.ProtocolVersion);
                return;
            }

            using NetworkSettingsPacket response = NetworkSettingsPacket.Create();
            client.SendPacket(response);
            client.Log.Info("Protocol Version: {0}",packet.ProtocolVersion);
            //has to be set after we send our NetworkSettings, until then there is no compression
            Client.SetHasCompression(client, true);
        }
    }
}
