using HarmonyLib;
using UnhollowerBaseLib;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        new[] {typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>)})]
    public class PatchColours
    {
        public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name)
        {
            switch ((int)name)
            {
                case 999991:
                    __result = "MELON";
                    return false;
                case 999992:
                    __result = "CHOCO";
                    return false;
                case 999993:
                    __result = "LTBL";
                    return false;
                case 999994:
                    __result = "BEIGE";
                    return false;
                case 999995:
                    __result = "LTPNK";
                    return false;
                case 999996:
                    __result = "TURQ";
                    return false;
                case 999997:
                    __result = "RNBW";
                    return false;
                case 999981:
                    __result = "Watermelon";
                    return false;
                case 999982:
                    __result = "Chocolate";
                    return false;
                case 999983:
                    __result = "Sky Blue";
                    return false;
                case 999984:
                    __result = "Beige";
                    return false;
                case 999985:
                    __result = "Hot Pink";
                    return false;
                case 999986:
                    __result = "Turquoise";
                    return false;
                case 999987:
                    __result = "Rainbow";
                    return false;
                
            }

            return true;
        }
    }
}