using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Plugins;
using ConMaster.Deepslate.Service;
using ConMaster.Raknet;
using System.Reflection;

namespace DeepslateLauncher.Plugins
{
    public class PluginManager
    {
        public List<Plugin> Plugins = [];
        public Config Config;
        public PluginManager(Config config)
        {
            Config = config;
        }
        public async Task Load()
        {
            string[] pluginPaths = Config.Plugins ?? [];
            int i = 1;
            int c = 0;
            foreach (var p in pluginPaths)
            {
                await Task.Yield();
                Console.WriteLine($"Loading Plugins [{i++}/{pluginPaths.Length}]");
                //Console.WriteLine(p);
                if (!File.Exists(p))
                {
                    Console.WriteLine("Plugin src doesn't exists: " + p);
                    continue;
                }
                byte[] bytes = await File.ReadAllBytesAsync(p);
                AssemblyName assemblyName = PluginLoader.GetAssemblyName(bytes);
                if (PluginLoader.TryLoad(bytes, out Plugin? plugin))
                {
                    Plugins.Add(plugin);
                    Console.WriteLine($"Registered Plugins '\x1b[3m{plugin.Name}\x1b[0m' [{++c}/{pluginPaths.Length}]");
                }
            }
            Console.WriteLine($"Registered Plugins [{c}/{pluginPaths.Length}]");
        }

        public IEnumerable<IPluginEntryInstance> CreateSeissons(GameService service)
        {
            foreach (var plugin in Plugins) yield return plugin.Create(service);
        }
    }
}