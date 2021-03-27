using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var player in __instance.playerStates)
            {
                var flag = Utils.Mayor != null && player.NameText.Text == Utils.Mayor.nameText.Text;
                var flag2 = PlayerControl.LocalPlayer.isMayor();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(0.44f, 0.31f, 0.66f, 1f);
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

            var flag2 = PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isMayor();
            if (flag2)
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.44f, 0.31f, 0.66f, 1f);
            }
        }
    }
}