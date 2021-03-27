using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class KillButtonSprite
    {
        private static Sprite Sprite => TownOfUs.EngineerFix;



        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.isEngineer()) return;
            if (__instance.KillButton == null) return;
            
            __instance.KillButton.renderer.sprite = Sprite;
            __instance.KillButton.SetCoolDown(0f, 10f);
            __instance.KillButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead);

            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var specials = system.Field_2.ToArray();
            var dummyActive = system.Field_4.BCKOBJLJEFE;
            var sabActive = specials.Any(s => s.Property_0);
            var renderer = __instance.KillButton.renderer;
            if (sabActive & !dummyActive & !PerformKill.UsedThisRound)
            {
                renderer.color = Palette.EnabledColor;
                renderer.material.SetFloat("_Desat", 0f);
                return;
            }

            renderer.color = Palette.DisabledColor;
            renderer.material.SetFloat("_Desat", 1f);
        }
        
    }
}