using ConMaster.Deepslate.Plugins;
using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Service;

namespace GeneralPlugin
{
    public class PluginEntry(GameService service): IPluginEntry
    {
        public static IPluginEntryInstance Create(GameService service) => new PluginEntry(service);
        public readonly GameService Service = service;
        public void OnStart()
        {
            var server = Service;
            Console.WriteLine("[Plugin] Game Service Name: " + server.Name);
            Console.WriteLine("[Plugin] Game Service Description: " + server.Description);
            Console.WriteLine("[Plugin] Running Network Servers: " + server.Servers.Count);
            Console.WriteLine("[Plugin] Running Game Servers: " + server.Games.Count);
        }
        public void OnLoad()
        {
            Console.WriteLine("[Plugin] Plugin executed at " + DateTime.Now);
            var game = new Game(Service, new());
            Service.AddGame(game);
        }

        public void OnShutdown()
        {
            Console.WriteLine("[Plugin] Plugin Shotdown at " + DateTime.Now);
        }
    }
}