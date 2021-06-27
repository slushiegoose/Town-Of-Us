using HarmonyLib;
using UnityEngine;

namespace TownOfUs.CustomHats
{
    public static class HatsTabPatch
    {
        [HarmonyPatch(typeof(HatsTab), nameof(HatsTab.OnEnable))]
        public static class HatsTab_OnEnable
        {
            public static void Postfix(HatsTab __instance)
            {
                foreach (var colorChip in __instance.ColorChips)
                {
                    if (colorChip.Inner.Hat.StoreName == HatCreation.TouHatIdentifier)
                    {
                        colorChip.Inner.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                    }
                }
            }
        }
        
    }
}