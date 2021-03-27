using System;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Reflection;

namespace TownOfUs.SheriffMod
{
    
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class Kill
    {

        [HarmonyPriority(Priority.First)]
        private static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Sheriff);
            if (!flag) return true;
            var role = Roles.Role.GetRole<Roles.Sheriff>(PlayerControl.LocalPlayer);
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = role.SheriffKillTimer() == 0f;
            if (!flag2) return false;
            if (!__instance.enabled) return false;
            var distBetweenPlayers = Utils.getDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
            var flag3 = distBetweenPlayers < (double)GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (!flag3) return false;
            if(role.ClosestPlayer.isShielded()) {
                if (CustomGameOptions.PlayerMurderIndicator)
                {
                    var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.AttemptSound, Hazel.SendOption.None, -1);
                    writer1.Write(role.ClosestPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer1);
                    MedicMod.StopKill.BreakShield(role.ClosestPlayer.PlayerId, false);
                }

                return false;
            }

            var flag4 = role.ClosestPlayer.Data.IsImpostor || role.ClosestPlayer.Is(RoleEnum.Glitch) ||
                        role.ClosestPlayer.Is(RoleEnum.Jester) && CustomGameOptions.SheriffKillsJester;
            if (!flag4)
            {
                if (CustomGameOptions.SheriffKillOther)
                {
                    Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, role.ClosestPlayer);
                }
                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer);
            }
            else
            {
                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, role.ClosestPlayer);
            }
            role.LastKilled = DateTime.UtcNow;

            return false;
        }
    }
}