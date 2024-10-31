using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol;
using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Packets;


namespace ConMaster.Deepslate.Service
{
    public partial class GameService
    {
        public DeepslateProtocol Protocol { get; init; }
        public GameService(DeepslateProtocol? protocol = null)
        {
            Protocol = protocol ?? new(748);
            Protocol.OnWarn += (s, d) => Console.WriteLine(d + " from " + s);
            Protocol.OnError += (s, d) => Console.WriteLine(d.GetException().Message,d.GetException().StackTrace);
            Protocol.PacketViolationWarning.OnRecieved += (s, c, p) =>
            {
                Console.WriteLine("Packet Violation Warning, Id: {0}, message: {1}",p.ViolationPacketId, p.ViolationContext);
            };


            Protocol.PlayerLogin.OnRecieved += PlayerLogin_OnRecieved;
            Protocol.ResourcePacksResponse.OnRecieved += ResourcePacksResponse_OnRecieved;
        }
    }
}
