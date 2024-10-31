using ConMaster.Deepslate.Plugins;
using ConMaster.Deepslate.Service;
using DeepslateLauncher.Plugins;
using System.Text.Json;
using ConMaster.Deepslate.Protocol;

namespace DeepslateLauncher
{
    internal class Program
    {
        public const string CONFIG_FILE_NAME = "config.json";
        static void Main(string[] args)
        {
            ConMaster.Raknet.Server provider = new();
            provider.SetMotd(new ConMaster.Raknet.ServerMotdInfo() { 
                CurrentPlayerCount = 0,
                MaxPlayerCount = 255,
                GameVersion = "100.0.0.0",
                Name = "§hSuper duper Sky Gen",
                LevelName = "This is the most unique Server ever"
            });
            GameService gameService = new()
            {
                Name = "BaseGameService"
            };
            gameService.OnError += (sender, erro) => Console.WriteLine(erro.GetException());
            gameService.OnWarn += (sender, erro) => Console.WriteLine("[Warn] " + erro);
            gameService.AddServer(new(provider, gameService.Protocol));
            //Console.SetOut(new TextLogger(Console.OpenStandardOutput()));
            if (!File.Exists(CONFIG_FILE_NAME))
            {
                File.WriteAllText(CONFIG_FILE_NAME, JsonSerializer.Serialize(new Config() { Plugins = [] }));
            }
            Config? config = JsonSerializer.Deserialize<Config>(File.ReadAllBytes(CONFIG_FILE_NAME));
            if (config == null)
            {
                Console.WriteLine("No config found");
                return;
            }
            PluginManager manager = new(config);
            manager.Load().Wait();
            IPluginEntryInstance[] array = manager.CreateSeissons(gameService).ToArray();
            foreach (var s in array) s.OnLoad();
            foreach (var s in array) s.OnStart();
            gameService.Start();
            Console.ReadLine();
            foreach (var s in array) s.OnShutdown();
        }
    }
}
