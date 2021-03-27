using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var player in __instance.playerStates)
            {
                var flag = Utils.Swapper != null && player.NameText.Text == Utils.Swapper.nameText.Text;
                var flag2 = PlayerControl.LocalPlayer.isSwapper();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(0.4f, 0.9f, 0.4f, 1f);
                }
            }
        }

        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            var flag = MeetingHud.Instance != null;
            if (flag)
            {
                UpdateMeeting(MeetingHud.Instance);
            }

            var flag2 = PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isSwapper();
            if (flag2)
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.4f, 0.9f, 0.4f, 1f);
            }
        }
    }
}