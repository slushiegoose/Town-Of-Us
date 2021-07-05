using HarmonyLib;
using UnityEngine;

namespace TownOfUs.NeutralRoles.GlitchMod
{
    [HarmonyPatch]
    public static class PreventButtons
    {
        public static bool IsHacked => GlitchCoroutines.IsHacked;

        [HarmonyPriority(Priority.First)]
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
        public static bool CanSetTimer() => !IsHacked;

        [HarmonyPriority(Priority.First)]
        [HarmonyPrefix]
        [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.SetTarget))]
        public static bool CanSetTarget([HarmonyArgument(0)] PlayerControl target) =>
            target == null || !IsHacked;

        [HarmonyPriority(Priority.First)]
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UseButtonManager), nameof(UseButtonManager.SetTarget))]
        public static bool CanUse([HarmonyArgument(0)] IUsable target) =>
            target == null || !IsHacked;

        [HarmonyPriority(Priority.First)]
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UseButtonManager), nameof(UseButtonManager.SetTarget))]
        public static void ClearSabotageButton(UseButtonManager __instance)
        {
            var button = __instance.currentButtonShown;
            if (button.imageName == ImageNames.SabotageButton)
            {
                var color = IsHacked ? UseButtonManager.DisabledColor : new Color(1, 1, 1, 1);
                button.graphic.color = button.text.color = color;
            }
        }

        // Can't patch `ReportButtonManager#SetActive` because weird ???
        [HarmonyPriority(Priority.First)]
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static void DisableReport(PlayerControl __instance)
        {
            if (IsHacked && __instance.AmOwner)
            {
                HudManager.Instance.ReportButton.SetActive(false);
            }
        }
    }
}
