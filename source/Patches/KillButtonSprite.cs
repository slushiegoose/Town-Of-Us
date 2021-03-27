using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class KillButtonSprite
    {
        private static Sprite Janitor => TownOfUs.JanitorClean;
        private static Sprite Shift => TownOfUs.Shift;
        private static Sprite Rewind => TownOfUs.Rewind;
        private static Sprite Medic => TownOfUs.MedicSprite;
        private static Sprite Seer => TownOfUs.SeerSprite;
        private static Sprite Douse => TownOfUs.DouseSprite;

        private static Sprite Kill => TownOfUs.Kill;


        private static Sprite[] Sprites => new[] {Janitor, Shift, Rewind, Medic, Seer};

        public static void Postfix(HudManager __instance)
        {
    
            if (__instance.KillButton == null) return;
            var flag = false;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Janitor) &&
                !(CustomGameOptions.JanitorKill && Utils.IsLastImp(PlayerControl.LocalPlayer)))
            {
                __instance.KillButton.renderer.sprite = Janitor;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Shifter))
            {
                __instance.KillButton.renderer.sprite = Shift;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.TimeLord))
            {
                __instance.KillButton.renderer.sprite = Rewind;
                flag = true;
            } 
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Seer))
            {
                __instance.KillButton.renderer.sprite = Seer;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Medic))
            {
                __instance.KillButton.renderer.sprite = Medic;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist))
            {
                __instance.KillButton.renderer.sprite = Douse;
                flag = true;
            }
            else
            {
                __instance.KillButton.renderer.sprite = Kill;
                flag = PlayerControl.LocalPlayer.Is(RoleEnum.Sheriff);
            }

            var keyInt = Input.GetKeyInt(KeyCode.Q);
            if (keyInt && __instance.KillButton != null && flag && !PlayerControl.LocalPlayer.Data.IsDead)
            {
                __instance.KillButton.PerformKill();
            }
        }
    }
}