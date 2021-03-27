using HarmonyLib;
using UnityEngine;

namespace TownOfUs.CamouflageMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {

        public static Sprite Camouflage => TownOfUs.Camouflage;

        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Camouflager)) return;
            var role = Roles.Role.GetRole<Roles.Camouflager>(PlayerControl.LocalPlayer);
            if (role.CamouflageButton == null)
            {
                role.CamouflageButton = Object.Instantiate(__instance.KillButton);
                role.CamouflageButton.renderer.enabled = true;
                role.CamouflageButton.renderer.sprite = Camouflage;
            }

            role.CamouflageButton.renderer.sprite = Camouflage;

            role.CamouflageButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead);
            var position = __instance.KillButton.transform.position;
            role.CamouflageButton.transform.position = new Vector3(position.x,
                __instance.ReportButton.transform.position.y, position.z);

            if (role.Enabled)
            {
                role.CamouflageButton.SetCoolDown(role.TimeRemaining, CustomGameOptions.CamouflagerDuration);
                return;
            }
            role.CamouflageButton.SetCoolDown(role.CamouflageTimer(), CustomGameOptions.CamouflagerCd);
            role.CamouflageButton.renderer.color = Palette.EnabledColor;
            role.CamouflageButton.renderer.material.SetFloat("_Desat", 0f);
        }
    }
}