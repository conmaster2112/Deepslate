using ConMaster.Deepslate.Entities;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Packets;

namespace ConMaster.Deepslate.Service
{
    public partial class Gamer(Game game, Client client, Authentication.ClientChainExtraData data)
    {
        private PlayStatus _status = 0;

        public bool IsXboxSigned => Xuid != string.Empty;
        //We want fast access without null check as the player is always present once the gamer play's
        public Player Player { get; protected set; } = null!;
        public Game Game { get; private set; } = game;
        public string Name { get; private set; } = data.DisplayName;
        public string Xuid { get; private set; } = data.XUID;
        public Client Client { get; private set; } = client;
        public PlayStatus PlayStatus { 
            get => _status;
            set
            {
                _status = value;
                using PlayStatusPacket packet = PlayStatusPacket.FromStatus(value);
                Client.SendPacket([packet]);
            }
        }
        public void Disconnect(string message, DisconnectReason reason = DisconnectReason.NoReason)
        {
            Client.Disconnect((int)reason, message);
        }
    }
}
