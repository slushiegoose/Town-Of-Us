using HarmonyLib;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.TimeLordMod
{
    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
    public class NoVitals
    {
        public static bool Prefix(VitalsMinigame __instance)
        {
            if (CustomGameOptions.TimeLordVitals) return true;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.TimeLord))
            {
                Object.Destroy(__instance.gameObject);
                return false;
            }

            return true;
        }
    }
}