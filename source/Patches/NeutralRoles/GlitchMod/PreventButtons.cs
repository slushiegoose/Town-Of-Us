using HarmonyLib;
using UnityEngine;

namespace TownOfUs.NeutralRoles.GlitchMod
{
    [HarmonyPatch]
    public static class PreventButtons
    {
        public static Sprite SabotageSprite = TranslationController.Instance.GetImage(ImageNames.SabotageButton);
        public static bool IsHacked => GlitchCoroutines.IsHacked;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
        public static bool CanSetTimer() => !IsHacked;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.SetTarget))]
        public static bool CanSetTarget([HarmonyArgument(0)] PlayerControl target) =>
            target == null || !IsHacked;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UseButtonManager), nameof(UseButtonManager.SetTarget))]
        public static bool CanUse([HarmonyArgument(0)] IUsable target) =>
            target == null || !IsHacked;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UseButtonManager), nameof(UseButtonManager.SetTarget))]
        public static void ClearSabotageButton(UseButtonManager __instance)
        {
            if (IsHacked && __instance.UseButton.sprite == SabotageSprite)
            {
                __instance.UseButton.color = UseButtonManager.DisabledColor;
            }
        }

        // Can't patch `ReportButtonManager#SetActive` because weird ???
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
