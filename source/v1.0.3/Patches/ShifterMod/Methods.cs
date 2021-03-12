using System;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
    public class Methods
    {
        
        public static PlayerControl ClosestPlayer;
        public static DateTime LastShifted;
        
        public static float ShifterShiftTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastShifted;
            var num = CustomGameOptions.SheriffKillCd * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
        
        public static PlayerControl getClosestPlayer(PlayerControl refplayer)
        {
            var num = double.MaxValue;
            PlayerControl result = null;
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var flag3 = player.Data.IsDead;
                if (flag3) continue;
                var flag = player.PlayerId != refplayer.PlayerId;
                if (!flag) continue;
                var distBetweenPlayers = getDistBetweenPlayers(player, refplayer);
                var flag2 = distBetweenPlayers < num;
                if (!flag2) continue;
                num = distBetweenPlayers;
                result = player;
            }
            return result;
        }
        
        public static double getDistBetweenPlayers(PlayerControl player, PlayerControl refplayer)
        {
            var truePosition = refplayer.GetTruePosition();
            var truePosition2 = player.GetTruePosition();
            return Vector2.Distance(truePosition, truePosition2);
        }
    }
}