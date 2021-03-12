using HarmonyLib;
using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var player in __instance.playerStates)
            {
                var flag = Utils.TimeMaster != null && player.NameText.Text == Utils.TimeMaster.nameText.Text;
                var flag2 = PlayerControl.LocalPlayer.isTimeMaster();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(0f, 0f, 1f, 1f);
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

            var flag2 = PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isTimeMaster();
            if (flag2)
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(0f, 0f, 1f, 1f);
            }
        }
    }
}