using ConMaster.Deepslate.Plugins;
using ConMaster.Deepslate.Service;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DeepslateLauncher.Plugins
{
    public delegate IPluginEntryInstance PluginFactory(GameService gameService);
    public class PluginDefinition
    {
        public const string START_METHOD_NAME = "OnStart";
        public const string LOAD_METHOD_NAME = "OnLoad";
        public const string SHUTDOWN_METHOD_NAME = "OnShutdown";
        public readonly string Name;
        public readonly string ScopeName;
        public readonly Version Version;
        public readonly AssemblyName AssemblyName;
        public readonly PluginFactory Factory;
        private PluginDefinition(Assembly assembly, Type entryClass, PluginFactory pluginFactory)
        {
            Assembly = assembly;
            AssemblyName = assembly.GetName();
            Name = AssemblyName.Name!;
            Version = AssemblyName.Version!;
            ScopeName = entryClass.Module.ScopeName;
            Factory = pluginFactory;
        }
        public readonly Assembly Assembly;
        public static bool TryGetValidMethod(Type type, string name, Type[] p, [MaybeNullWhen(false)] out MethodInfo method)
        {
            MethodInfo? m = type.GetMethod(name, p);
            if(IsValidMethod(m))
            {
                method = m!;
                return true;
            }
            method = null;
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidMethod(MethodInfo? method)
        {
            return method != null && !method.IsGenericMethod && method.IsStatic;
        }
        public static bool HasStartMethod(Type type)
        {
            MethodInfo? method = type.GetMethod(START_METHOD_NAME, [typeof(GameService)]);
            return IsValidMethod(method);
        }
        public static bool IsPluginEntryClass(Type type)
        {
            if (
                !type.IsClass ||
                !(type.IsAbstract && type.IsSealed) ||
                type.IsGenericType
                ) return false;
            return HasStartMethod(type);
        }
        public static bool TryBindMethod<T>(MethodInfo info, [MaybeNullWhen(false)] out T method) where T : Delegate
        {
            try
            {
                method = info.CreateDelegate<T>();
                return true;
            }
            catch (Exception)
            {
                method = null;
                return false;
            }
        }
        public static PluginDefinition? Create(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.IsAssignableTo(typeof(IPluginEntry)))
                {
                    MethodInfo? method = type.GetMethod(nameof(IPluginEntryClass.Create));
                    if (method == null) return null;
                    return new PluginDefinition(assembly, type, method.CreateDelegate<PluginFactory>());
                }/*
                if (IsPluginEntryClass(type) && TryGetValidMethod(type, START_METHOD_NAME, [typeof(GameService)], out MethodInfo? info))
                {
                    if(TryBindMethod<Func<GameService, object?>>(info, out Func<GameService, object?>? method))
                    {
                        //return new PluginDefinition(assembly, type, method);
                    }
                    else if(TryBindMethod<Action<GameService>>(info, out Action<GameService>? method2))
                    {
                        //return new PluginDefinition(assembly, type, (server) => { method2(server); return null; });
                    }
                    break;
                }*/
            }
            return null;
        }
    }
}
