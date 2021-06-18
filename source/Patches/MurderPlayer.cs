using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcMurderPlayer))]
    public class MurderPlayer
    {
        public static bool Prefix(PlayerControl __instance, PlayerControl __0)
        {
            Utils.RpcMurderPlayer(__instance, __0);
            return false;
        }
    }
}