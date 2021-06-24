using HarmonyLib;
using System.Collections.Generic;
using UnhollowerBaseLib;

using PlayerData = GameData.PlayerInfo;

namespace TownOfUs
{
    
    public static class MeetingIntroPatch
    {
        public static List<PlayerData> DeadPlayers =
            new List<PlayerData>();

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static class MeetingHud_Start
        {
            public static void Postfix() =>
                Utils.ShowDeadBodies = PlayerControl.LocalPlayer.Data.IsDead;
        }

        [HarmonyPatch(typeof(MeetingIntroAnimation), nameof(MeetingIntroAnimation.CoRun))]
        public static class IntroPatch
        {
            public static void Prefix(
                [HarmonyArgument(1)] ref Il2CppReferenceArray<PlayerData> deadPlayers
            )
            {
                var players = new List<PlayerData>(deadPlayers);
                foreach (var playerData in DeadPlayers)
                {
                    if (!players.Contains(playerData))
                        players.Add(playerData);
                }
<<<<<<< HEAD
                deadPlayers = players.ToArray();
=======
                deadPlayers = new Il2CppReferenceArray<PlayerData>(players.ToArray());
>>>>>>> a092cc6 (fix: show destroyed bodies as dead)
                DeadPlayers.Clear();
            }
        }
    }
}
