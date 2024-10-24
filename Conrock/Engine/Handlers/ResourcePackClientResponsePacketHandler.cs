using ConMaster.Bedrock.Enums;
using ConMaster.Bedrock.Network;
using ConMaster.Bedrock.Packets;

namespace ConMaster.Bedrock.Engine.Handlers
{
    internal class ResourcePackClientResponsePacketHandler
    {
        public static ProtocolPacketHandler<ResourcePackClientResponsePacket> GetHandler()
        {
            return new(Handler, ResourcePackClientResponsePacket.PACKET_ID);
        }
        public static void Handler(Client client, ResourcePackClientResponsePacket packet)
        {
            switch (packet.Response)
            {
                case ResourcePackResponse.None:
                    break;
                case ResourcePackResponse.Refused:
                    break;
                case ResourcePackResponse.SendPacks:
                    break;
                case ResourcePackResponse.HaveAllPacks:
                    ResourcePackStackPacket stack = ResourcePackStackPacket.Create();
                    client.SendPacket(stack);
                    ((IPoolable<ResourcePackStackPacket>)stack).Return();
                    return;
                case ResourcePackResponse.Completed:
                    GeneralHandlers.JoinProccess(client);
                    return;
                default:
                    break;
            }
            client.Disconnect(nameof(ResourcePackClientResponsePacketHandler) + " is not implemented: §a " + packet.Response);
        }
    }
}
