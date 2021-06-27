using HarmonyLib;
using UnhollowerBaseLib;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        new[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class PatchColours
    {
        public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name)
        {
            var newResult = (int)name switch
            {
                999990 => "Watermelon",
                999991 => "Chocolate",
                999992 => "Sky Blue",
                999993 => "Beige",
                999994 => "Hot Pink",
                999995 => "Turquoise",
                999996 => "Lilac",
                999997 => "Rainbow",
                999998 => "Azure",
                _ => null
            };
            if (newResult != null)
            {
                __result = newResult;
                return false;
            }

            return true;
        }
    }
}
