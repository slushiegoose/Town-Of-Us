using System;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Reflection;

namespace TownOfUs.SheriffMod
{
    
    [HarmonyPatch(typeof(KillButtonManager))]
    public static class Kill
    {
        
        [HarmonyPatch( "PerformKill")]
        private static bool Prefix(MethodBase __originalMethod)
        {
            var flag = PlayerControl.LocalPlayer.isSheriff();
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = Methods.SheriffKillTimer() == 0f;
            if (!flag2) return false;
            var distBetweenPlayers = Methods.getDistBetweenPlayers(PlayerControl.LocalPlayer, Methods.ClosestPlayer);
            var flag3 = distBetweenPlayers < (double)GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (!flag3) return false;
            var flag4 = !Methods.ClosestPlayer.Data.IsImpostor;
            if (flag4)
            {
                var messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SheriffKill, SendOption.Reliable, -1);
                messageWriter.Write(PlayerControl.LocalPlayer.PlayerId);
                messageWriter.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                PlayerControl.LocalPlayer.MurderPlayer(PlayerControl.LocalPlayer);
            }
            else
            {
                var messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SheriffKill, SendOption.Reliable, -1);
                messageWriter2.Write(PlayerControl.LocalPlayer.PlayerId);
                messageWriter2.Write(Methods.ClosestPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
                PlayerControl.LocalPlayer.MurderPlayer(Methods.ClosestPlayer);
            }
            Methods.LastKilled = DateTime.UtcNow;

            return false;
        }
    }
}