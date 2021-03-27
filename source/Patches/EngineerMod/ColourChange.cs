using HarmonyLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            PerformKill.UsedThisRound = false;
            foreach (var player in __instance.playerStates)
            {

                var flag = Utils.Engineer != null && player.NameText.Text == Utils.Engineer.nameText.Text;
                var flag2 = PlayerControl.LocalPlayer.isEngineer();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(1f, 0.7f, 0f, 1f);
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

            var flag2 = PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isEngineer();

            if (flag2)
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(1f, 0.7f, 0f, 1f);
            }
            
        }
    }
}