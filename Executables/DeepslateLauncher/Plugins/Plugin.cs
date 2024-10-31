using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Plugins;
using ConMaster.Deepslate.Service;
using System.Reflection;

namespace DeepslateLauncher.Plugins
{
    public class Plugin
    {
        public readonly Assembly Assembly;
        public readonly PluginDefinition Definition;
        public string Name => Definition.ScopeName;
        public Plugin(PluginDefinition definition)
        {
            Assembly = definition.Assembly;
            Definition = definition;
        }
        public IPluginEntryInstance Create(GameService service)
        {
            return Definition.Factory(service);
        }
    }
}
