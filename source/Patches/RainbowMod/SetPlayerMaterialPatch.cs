using HarmonyLib;
using UnityEngine;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
    public class SetPlayerMaterialPatch
    {
        public static bool Prefix([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer rend)
        {
            var r = rend.gameObject.GetComponent<RainbowBehaviour>()
                ?? rend.gameObject.AddComponent<RainbowBehaviour>();

            r.AddRend(rend, colorId);
            return !RainbowUtils.IsRainbow(colorId);
        }
    }
}
