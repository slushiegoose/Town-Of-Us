using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class ColourChange
    {
        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var player in __instance.playerStates)
            {
                var flag = Utils.Shifter != null && player.NameText.Text == Utils.Shifter.nameText.Text;
                var flag2 = PlayerControl.LocalPlayer.isShifter();
                if (flag & flag2)
                {
                    player.NameText.Color = new Color(0.6f, 0.6f, 0.6f, 1f);
                }
            }
        }

        [HarmonyPatch(nameof(HudManager.Update))]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.First)]
        public static void Postfix2(HudManager __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.Data != null && !player.Data.IsImpostor)
                {
                    player.nameText.Color = Color.white;
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

            if (PlayerControl.AllPlayerControls.Count > 1 & PlayerControl.LocalPlayer.isShifter())
            {
                PlayerControl.LocalPlayer.nameText.Color = new Color(0.6f, 0.6f, 0.6f, 1f);
            }

        }
    }
}