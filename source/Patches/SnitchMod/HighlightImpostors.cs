using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SnitchMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HighlightImpostors
    {

        private static void UpdateMeeting(MeetingHud __instance)
        {
            foreach (var state in __instance.playerStates)
            {
                if (Utils.PlayerById((byte) state.TargetPlayerId).Data.IsImpostor)
                {
                    state.NameText.color = Palette.ImpostorRed;
                }
            }
            

        }
        public static void Postfix(HudManager __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Snitch)) return;
            var role = Roles.Role.GetRole<Roles.Snitch>(PlayerControl.LocalPlayer);
            if (!role.TasksDone) return;
            if (MeetingHud.Instance) UpdateMeeting(MeetingHud.Instance);

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.Data.IsImpostor) player.nameText.color = Palette.ImpostorRed;
            }

        }
    }
}