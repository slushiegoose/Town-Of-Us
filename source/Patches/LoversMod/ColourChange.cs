using HarmonyLib;
using UnityEngine;

namespace TownOfUs.LoversMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var player in __instance.playerStates)
            {

                var flag = Utils.Lover1 != null && Utils.Lover2 != null &&
                           (player.NameText.Text == Utils.Lover1.nameText.Text ||
                            player.NameText.Text == Utils.Lover2.nameText.Text);
                var flag2 = PlayerControl.LocalPlayer.isLover();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(1f, 0.4f, 0.8f, 1f);
                }

            }
        }

        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            var flag = MeetingHud.Instance != null;
            if (flag)
            {
                UpdateMeeting(MeetingHud.Instance);
            }

            var flag2 = PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isLover();

            if (flag2)
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(1f, 0.4f, 0.8f, 1f);
                PlayerControl.LocalPlayer.OtherLover().nameText.Color = new Color(1f, 0.4f, 0.8f, 1f);
            }

            if (PlayerControl.LocalPlayer.isLover() & !__instance.Chat.isActiveAndEnabled)
            {
                __instance.Chat.SetVisible(true);
            }
        }
    }
}
