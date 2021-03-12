using HarmonyLib;
using UnityEngine;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var player in __instance.playerStates)
            {

                var flag = Utils.Jester != null && player.NameText.Text == Utils.Jester.nameText.Text;
                var flag2 = PlayerControl.LocalPlayer.isJester();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(1f, 0.75f, 0.8f, 1f);
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

            var flag2 = PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isJester();

            if (flag2)
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(1f, 0.75f, 0.8f, 1f);
            }
        }
    }
}