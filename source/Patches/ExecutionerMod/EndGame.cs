using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using Reactor.Extensions;

namespace TownOfUs.ExecutionerMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.RpcEndGame))]
    public class EndGame
    {
        public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] GameOverReason reason)
        {
            if (reason != GameOverReason.HumansByVote && reason != GameOverReason.HumansByTask) return true;

            foreach (var role in Role.AllRoles)
            {
                if (role.RoleType == RoleEnum.Executioner)
                {
                    ((Executioner) role).Loses();
                }
            }
            
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.ExecutionerLose,
                SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            return true;
        }
    }
}