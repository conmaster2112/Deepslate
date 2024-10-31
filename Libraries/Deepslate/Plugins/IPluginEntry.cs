using ConMaster.Deepslate.Service;

namespace ConMaster.Deepslate.Plugins
{
    public interface IPluginEntry: IPluginEntryInstance, IPluginEntryClass
    {

    }
    public interface IPluginEntryClass
    {
        public static abstract IPluginEntryInstance Create(GameService service);
    }
    public interface IPluginEntryInstance
    {
        public void OnLoad();
        public void OnShutdown();
        public void OnStart();
    }
}
