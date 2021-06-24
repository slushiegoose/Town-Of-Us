using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
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

        [HarmonyPatch(typeof(MeetingIntroAnimation), nameof(MeetingIntroAnimation.Init))]
        public static class IntroPatch
        {
            public static void Prefix(
                [HarmonyArgument(1)] ref Il2CppReferenceArray<PlayerData> deadPlayers
            )
            {
                var players = deadPlayers.ToHashSet();
                players.UnionWith(DeadPlayers);
                deadPlayers = players.ToArray();
                DeadPlayers.Clear();
            }
        }
    }
}
