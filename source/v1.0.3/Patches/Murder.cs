using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class Murder
    {
        [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
        public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl opponent)
        {

            if (__instance.isSheriff())
            {
                __instance.Data.IsImpostor = true;
            }

            if (__instance.isShifter())
            {
                __instance.Data.IsImpostor = true;
            }
        }

        [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl opponent)
        {
            if (__instance.isSheriff() || __instance.isShifter())
            {
                __instance.Data.IsImpostor = false;
            }
        }
    }
}