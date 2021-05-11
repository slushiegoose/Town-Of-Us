using System.Linq;
using HarmonyLib;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace TownOfUs.SeerMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class Update
    {

        public static string NameText(PlayerControl player, string str = "", bool meeting=false)
        {
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed)
            {
                if (meeting && !CustomGameOptions.MeetingColourblind)
                {
                    return player.name + str;
                }

                return "";
            }

            return player.name + str;
        }
        
        private static void UpdateMeeting(MeetingHud __instance) {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Seer))
            {
                var seerRole = (Roles.Seer) role;
                if (!seerRole.Investigated.Contains(PlayerControl.LocalPlayer.PlayerId)) continue;
                if (!seerRole.CheckSeeReveal(PlayerControl.LocalPlayer)) continue;
                var state = __instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == seerRole.Player.PlayerId);
                state.NameText.color = seerRole.Color;
                state.NameText.text = NameText(seerRole.Player,  " (Seer)", true);
            }
        }
        private static void UpdateMeeting(MeetingHud __instance, Roles.Seer seer)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (!seer.Investigated.Contains(player.PlayerId)) continue;
                foreach (var state in __instance.playerStates)
                {
                    if (player.PlayerId != state.TargetPlayerId) continue;
                    var roleType = Utils.GetRole(player);
                    switch (roleType)
                    {
                        case RoleEnum.Crewmate:
                            state.NameText.color =
                                CustomGameOptions.SeerInfo == SeerInfo.Faction ? Color.green : Color.white;
                            state.NameText.text = NameText(player,  (CustomGameOptions.SeerInfo == SeerInfo.Role ? " (Crew)" : ""), true);
                            break;
                        case RoleEnum.Impostor:
                            state.NameText.color = CustomGameOptions.SeerInfo == SeerInfo.Faction
                                ? Color.red
                                : Palette.ImpostorRed;
                            state.NameText.text = NameText(player, (CustomGameOptions.SeerInfo == SeerInfo.Role ? " (Imp)" : ""), true);
                            break;
                        default:
                            var role = Roles.Role.GetRole(player);
                            state.NameText.color = CustomGameOptions.SeerInfo == SeerInfo.Faction
                                ? role.FactionColor
                                : role.Color;
                            state.NameText.text = NameText(player,  (CustomGameOptions.SeerInfo == SeerInfo.Role ? $" ({role.Name})" : ""), true);
                            break;
                    }
                }
            }
        }

        [HarmonyPriority(Priority.Last)]
        private static void Postfix(HudManager __instance)
        
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Seer))
            {
                var seerRole = (Roles.Seer) role;
                if (!seerRole.Investigated.Contains(PlayerControl.LocalPlayer.PlayerId)) continue;
                if (!seerRole.CheckSeeReveal(PlayerControl.LocalPlayer)) continue;
                
                seerRole.Player.nameText.color = seerRole.Color;
                seerRole.Player.nameText.text = NameText(seerRole.Player, " (Seer)");
            }
            if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance);
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Seer)) return;
            var seer = Roles.Role.GetRole<Roles.Seer>(PlayerControl.LocalPlayer);
            if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance, seer);
            
            
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (!seer.Investigated.Contains(player.PlayerId)) continue;
                var roleType = Utils.GetRole(player);
                switch (roleType)
                {
                    case RoleEnum.Crewmate:
                        player.nameText.color =
                            CustomGameOptions.SeerInfo == SeerInfo.Faction ? Color.green : Color.white;
                        player.nameText.text = NameText(player, (CustomGameOptions.SeerInfo == SeerInfo.Role ? " (Crew)" : ""));
                        break;
                    case RoleEnum.Impostor:
                        player.nameText.color = CustomGameOptions.SeerInfo == SeerInfo.Faction
                            ? Color.red
                            : Palette.ImpostorRed;
                        player.nameText.text = NameText(player, (CustomGameOptions.SeerInfo == SeerInfo.Role ? " (Imp)" : ""));
                        break;
                    default:
                        var role = Roles.Role.GetRole(player);
                        player.nameText.color = CustomGameOptions.SeerInfo == SeerInfo.Faction
                            ? role.FactionColor
                            : role.Color;
                        player.nameText.text = NameText(player, (CustomGameOptions.SeerInfo == SeerInfo.Role ? $" ({role.Name})" : ""));
                        break;
                }

            }
            
        }
    }
}