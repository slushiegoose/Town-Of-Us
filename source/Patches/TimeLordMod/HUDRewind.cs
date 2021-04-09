using HarmonyLib;

namespace TownOfUs.TimeLordMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class HUDRewind
    {
        public static void Postfix(PlayerControl __instance)
        {
            UpdateRewindButton(__instance);
        }

        public static void UpdateRewindButton(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.TimeLord)) return;
            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;
            var rewindButton = DestroyableSingleton<HudManager>.Instance.KillButton;

            var role = Roles.Role.GetRole<Roles.TimeLord>(PlayerControl.LocalPlayer);
            
            
            
            if (isDead)
            {
                rewindButton.gameObject.SetActive(false);
                rewindButton.isActive = false;
            }
            else
            {
                rewindButton.gameObject.SetActive(!MeetingHud.Instance);
                rewindButton.isActive = !MeetingHud.Instance;
                rewindButton.SetCoolDown(role.TimeLordRewindTimer(), role.GetCooldown());
            }
            
            var renderer = rewindButton.renderer;
            if (!rewindButton.isCoolingDown & !RecordRewind.rewinding & rewindButton.enabled)
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