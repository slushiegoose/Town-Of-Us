/*using HarmonyLib;
using InnerNet;

namespace TownOfUs
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Method_26))]
    public class OnDisconnect
    {
        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ClientData data,
            [HarmonyArgument(1)] DisconnectReasons reason)
        {
            var pc = data.Character;
            if (pc == null) return;
            if (pc.Data == null) return;
            pc.Data.IsDead = true;
        }
    }
}*/