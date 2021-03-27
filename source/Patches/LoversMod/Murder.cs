using HarmonyLib;

namespace TownOfUs.LoversMod
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class Murder
    {
        [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
        public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl opponent)
        {
            if (__instance.isLover())
            {
                __instance.Data.IsImpostor = true;
            }
        }

        [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl opponent)
        {
            if (__instance.isLover1())
            {
                __instance.Data.IsImpostor = false;
            }
            else if (__instance.isLover2() & !Utils.LoverImpostor)
            {
                __instance.Data.IsImpostor = false;
            }
        }
    }
}