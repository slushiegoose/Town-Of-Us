using System;
using HarmonyLib;
using Hazel;
using Reactor;
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKillButton
    
    {
        
        public static DateTime LastCleaned;
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.isJanitor();
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = JanitorTimer() == 0f;
            if (!flag2) return false;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (Vector2.Distance(KillButtonTarget.CurrentTarget.TruePosition,
                PlayerControl.LocalPlayer.GetTruePosition()) > maxDistance) return false;
            var playerId = KillButtonTarget.CurrentTarget.ParentId;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.JanitorClean, SendOption.Reliable, -1);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Coroutines.Start(Coroutine.CleanCoroutine(KillButtonTarget.CurrentTarget));
            return false;
        }
        
        public static float JanitorTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastCleaned;
            var num = CustomGameOptions.JanitorCleanCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}