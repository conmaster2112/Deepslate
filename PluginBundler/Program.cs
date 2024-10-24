using System.Reflection;

namespace PluginBundler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Plugin Bundler v" + Assembly.GetEntryAssembly()!.GetName().Version);
            Console.WriteLine(Environment.CurrentDirectory);
        }
    }
}
