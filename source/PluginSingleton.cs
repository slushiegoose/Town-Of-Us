using System;
using System.Linq;
using BepInEx.IL2CPP;

namespace TownOfUs
{
    public static class PluginSingleton<T> where T : BasePlugin
    {
        private static T _instance;

        public static T Instance
        {
            get => _instance ??= IL2CPPChainloader.Instance.Plugins.Values.Select(x => x.Instance).OfType<T>().Single();

            set
            {
                if (_instance != null) throw new Exception($"Instance for {typeof(T).FullName} is already set");

                _instance = value;
            }
        }
    }
}