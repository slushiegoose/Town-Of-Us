using HarmonyLib;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(EmergencyMinigame))]
    public class Button
    {

        [HarmonyPatch(nameof(EmergencyMinigame.Update))]
        public static void Postfix(EmergencyMinigame __instance)
        {
            if (__instance.Field_7 != 1) return;
            if (!PerformKill.UsedThisRound) return;
            __instance.StatusText.Text = "AN ENGINEER HAS\nUSED THEIR FIX\nFOR THIS ROUND.";
            __instance.NumberText.Text = string.Empty;
            __instance.ButtonActive = false;
            __instance.ClosedLid.gameObject.SetActive(true);
            __instance.OpenLid.gameObject.SetActive(false);
        }
    }
}