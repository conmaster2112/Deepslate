using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Packets;

namespace ConMaster.Deepslate.Service
{
    public partial class GameService
    {

        protected async void ResourcePacksResponse_OnRecieved(BaseProtocol proto, Client client, ResourcePackClientResponsePacket packet)
        {
            // Return if no gamer for this client
            if (!_gamers.TryGetValue(client, out var gamer)) return;

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
                    client.SendPacket([stack]);
                    ((IDisposable)stack).Dispose();
                    return;
                case ResourcePackResponse.Completed:
                    StartGameSequence(gamer);
                    return;
                default:
                    break;
            }
            gamer.Disconnect(nameof(ResourcePacksResponse_OnRecieved) + " is not fully implemented: §a " + packet.Response);
        }
        protected ResourcePacksInfoPacket BuildResourcePackInfoPacket(Gamer gamer)
        {
            ResourcePacksInfoPacket packet = ResourcePacksInfoPacket.Create();
            //packet.CndUrls = [];
            packet.ResourcePacks = [];
            packet.MustAccept = false;
            packet.HasAddons = false;
            packet.HasAddons = false;
            return packet;
        }
    }
}
