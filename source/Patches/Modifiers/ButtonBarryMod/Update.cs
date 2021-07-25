using HarmonyLib;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Modifiers.ButtonBarryMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class Update
    {
        public static Sprite Button => TownOfUs.ButtonSprite;

        public static void Postfix(HudManager __instance)
        {
            UpdateButtonButton(__instance);
        }

        private static void UpdateButtonButton(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(ModifierEnum.ButtonBarry)) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Glitch)) return;

            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;


            var role = Modifier.GetModifier<ButtonBarry>(PlayerControl.LocalPlayer);

            if (role.ButtonButton == null)
            {
                role.ButtonButton = Object.Instantiate(__instance.KillButton, HudManager.Instance.transform);
                role.ButtonButton.renderer.enabled = true;
                role.ButtonButton.renderer.sprite = Button;
            }

            role.ButtonButton.renderer.sprite = Button;


            role.ButtonButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance);

            role.ButtonButton.SetCoolDown(0f, 1f);
            var renderer = role.ButtonButton.renderer;

            var position1 = __instance.UseButton.transform.position;
            role.ButtonButton.transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x + 0.75f, position1.y,
                position1.z);

            if (!role.ButtonUsed && PlayerControl.LocalPlayer.RemainingEmergencies > 0)
            {
                renderer.color = Palette.EnabledColor;
                renderer.material.SetFloat("_Desat", 0f);
                return;
            }

            renderer.color = Palette.DisabledClear;
            renderer.material.SetFloat("_Desat", 1f);
        }
    }
}