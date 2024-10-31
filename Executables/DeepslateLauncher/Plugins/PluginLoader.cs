
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace DeepslateLauncher.Plugins
{
    public static class PluginLoader
    {
        public static AssemblyName GetAssemblyName(byte[] rawBytes)
        {
            AssemblyDefinition s = new PEReader(new MemoryStream(rawBytes)).GetMetadataReader().GetAssemblyDefinition();
            return s.GetAssemblyName();
        }
        public static bool TryLoad(byte[] rawBytes, [MaybeNullWhen(false)] out Plugin plugin)
        {
            Assembly assembly = Assembly.Load(rawBytes);
            PluginDefinition? definition = PluginDefinition.Create(assembly);
            if(definition == null)
            {
                plugin = null;
                return false;
            }
            plugin = new(definition);
            return true;
        }
    }
}
