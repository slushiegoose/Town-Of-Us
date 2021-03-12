using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(HudManager))]
    public class KillButtonSprite
    {
        private static Sprite Janitor => TownOfUs.JanitorClean;
        private static Sprite Shift => TownOfUs.Shift;
        private static Sprite Kill => TownOfUs.Kill;
        private static Sprite Rewind => TownOfUs.Rewind;
        
        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            var flag = false;
            if (PlayerControl.LocalPlayer.isJanitor())
            {
                __instance.KillButton.renderer.sprite = Janitor;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.isShifter())
            {
                __instance.KillButton.renderer.sprite = Shift;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.isTimeMaster())
            {
                __instance.KillButton.renderer.sprite = Rewind;
            }
            else
            {
                __instance.KillButton.renderer.sprite = Kill;
            }
            
            var keyInt = Input.GetKeyInt(KeyCode.Q);
            if (keyInt && __instance.KillButton != null && flag)
            {
                __instance.KillButton.PerformKill();
            }
        }
    }
}