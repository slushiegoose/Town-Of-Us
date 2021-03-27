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
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Engineer)) return;
            if (__instance.KillButton == null) return;

            var role = Roles.Role.GetRole<Roles.Engineer>(PlayerControl.LocalPlayer);
            
            __instance.KillButton.renderer.sprite = Sprite;
            __instance.KillButton.SetCoolDown(0f, 10f);
            __instance.KillButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && __instance.UseButton.isActiveAndEnabled);

            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            if (system == null) return;
            var specials = system.specials.ToArray();
            var dummyActive = system.dummy.IsActive;
            var sabActive = specials.Any(s => s.IsActive);
            var renderer = __instance.KillButton.renderer;
            if (sabActive & !dummyActive & !role.UsedThisRound & __instance.KillButton.enabled)
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