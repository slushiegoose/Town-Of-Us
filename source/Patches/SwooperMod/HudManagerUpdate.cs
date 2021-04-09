using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SwooperMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static Sprite SwoopSprite => TownOfUs.SwoopSprite;

        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Swooper)) return;
            var role = Roles.Role.GetRole<Roles.Swooper>(PlayerControl.LocalPlayer);
            if (role.SwoopButton == null)
            {
                role.SwoopButton = Object.Instantiate(__instance.KillButton, HudManager.Instance.transform);
                role.SwoopButton.renderer.enabled = true;

            }

            role.SwoopButton.renderer.sprite = SwoopSprite;
            role.SwoopButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance);
            var position = __instance.KillButton.transform.localPosition;
            role.SwoopButton.transform.localPosition = new Vector3(position.x,
                __instance.ReportButton.transform.localPosition.y, position.z);

            if (role.IsSwooped)
            {
                role.SwoopButton.SetCoolDown(role.TimeRemaining, CustomGameOptions.SwoopDuration);
                return;
            }
            
            role.SwoopButton.SetCoolDown(role.SwoopTimer(), CustomGameOptions.MineCd);


            role.SwoopButton.renderer.color = Palette.EnabledColor;
            role.SwoopButton.renderer.material.SetFloat("_Desat", 0f);



        }
    }
}