using HarmonyLib;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(PlayerControl))]
    public class HUDRewind
    {

        [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
        public static void Postfix(PlayerControl __instance)
        {
            UpdateRewindButton(__instance);
        }

        public static void UpdateRewindButton(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.isTimeMaster()) return;
            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;
            var rewindButton = DestroyableSingleton<HudManager>.Instance.KillButton;

            
            
            
            if (isDead)
            {
                rewindButton.gameObject.SetActive(false);
                rewindButton.isActive = false;
            }
            else
            {
                rewindButton.gameObject.SetActive(true);
                rewindButton.isActive = true;
                rewindButton.SetCoolDown(Methods.TimeMasterRewindTimer(), Methods.GetCooldown());
            }
            
            var renderer = rewindButton.renderer;
            if (!rewindButton.isCoolingDown & !RecordRewind.rewinding)
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