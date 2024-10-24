using ConMaster.Bedrock.Engine.Handlers;
using ConMaster.Bedrock.Level;
using ConMaster.Bedrock.Network;
using ConMaster.Raknet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Bedrock.Engine
{
    public class BedrockRunner
    {
        private ServerMotdInfo _MOTD = new()
        {
            Name = "Bedrock Runner Server",
            LevelName = "Playing survival",
            MaxPlayerCount = 100,
            GameVersion = "1.22.0"
        };
        public string Title { get => _MOTD.Name; set { _MOTD.Name = value; Network.UpdateMOTD(_MOTD); } }
        public string GameVersion { get => _MOTD.GameVersion; set { _MOTD.GameVersion = value; Network.UpdateMOTD(_MOTD); } }
        public string LevelName { get => _MOTD.LevelName; set { _MOTD.LevelName = value; Network.UpdateMOTD(_MOTD); } }
        public int MaxPlayers { get => _MOTD.MaxPlayerCount; set { _MOTD.MaxPlayerCount = value; Network.UpdateMOTD(_MOTD); } }

        public Game Game { get; init; }
        public Protocol Protocol { get; init; }
        public Network.Network Network { get; init; }
        public World World => Game.World;
        public BedrockRunner()
        {
            Game = new(this);
            Protocol = new(729);
            Network = new(this);
            Network.UpdateMOTD(_MOTD);
            SetUpHandlers();
        }
        public async Task Run()
        {
            List<Task> tasks = [
                Game.Run(default),
                Network.Start()
            ];
            await Task.WhenAll(tasks);
        }
        public void SetUpHandlers()
        {
            Protocol.AddPacketHandler(LoginPacketHandler.GetHandler());
            Protocol.AddPacketHandler(RequestNetworkSettingsHandler.GetHandler());
            Protocol.AddPacketHandler(ResourcePackClientResponsePacketHandler.GetHandler());
            GeneralHandlers.AddProtocolHandlers(Protocol);
        }
    }
}
