using System;
using HarmonyLib;
using Hazel.Udp;
using Reactor;

namespace TownOfUs
{
    public static class DirtyPatches
    {
        public static void Initialize(Harmony harmony)
        {
            try
            {
                harmony.Unpatch(
                    AccessTools.Method(typeof(UdpConnection), nameof(UdpConnection.HandleSend)),
                    HarmonyPatchType.Prefix,
                    ReactorPlugin.Id
                );
            }
            catch (Exception e)
            {
                Logger<TownOfUs>.Instance.LogError($"Exception unpatching Reactor's UdpConnection.HandleSend Prefix: {e.Message}, Stack: {e.StackTrace}");
            }
        }
    }
}