using HarmonyLib;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    public class PlayerTabPatch
    {
        public static void Postfix(PlayerTab __instance)
        {
            foreach (var chip in __instance.Field_8)
            {
                chip.transform.localScale *= 0.65f;
            }
        }
    }
}