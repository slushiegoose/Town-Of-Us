using HarmonyLib;
using UnityEngine;
using TownOfUs.Roles;

namespace TownOfUs
{
    internal static class TaskPatches
    {
        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        private class GameData_RecomputeTaskCounts
        {
            private static bool Prefix(GameData __instance)
            {
                __instance.TotalTasks = 0;
                __instance.CompletedTasks = 0;
                for (var i = 0; i < __instance.AllPlayers.Count; i++)
                {
                    var playerInfo = __instance.AllPlayers.ToArray()[i];
                    if (!playerInfo.Disconnected && playerInfo.Tasks != null && playerInfo.Object &&
                        (PlayerControl.GameOptions.GhostsDoTasks || !playerInfo.IsDead) && !playerInfo.IsImpostor &&
                        !(
                            playerInfo._object.Is(RoleEnum.Jester) || playerInfo._object.Is(RoleEnum.Shifter) ||
                            playerInfo._object.Is(RoleEnum.Glitch) || playerInfo._object.Is(RoleEnum.Executioner) ||
                            playerInfo._object.Is(RoleEnum.Arsonist) || playerInfo._object.Is(RoleEnum.Phantom)
                        ))
                        for (var j = 0; j < playerInfo.Tasks.Count; j++)
                        {
                            __instance.TotalTasks++;
                            if (playerInfo.Tasks.ToArray()[j].Complete) __instance.CompletedTasks++;
                        }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
        private class Console_CanUse
        {
            private static void Postfix(
                Console __instance,
                [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
                [HarmonyArgument(1)] ref bool canUse,
                [HarmonyArgument(2)] ref bool couldUse,
                ref float __result
            )
            {
                var player = playerInfo.Object;

                var role = Role.GetRole(player);

                var isNeutral = role.Faction == Faction.Neutral && role.RoleType != RoleEnum.Phantom;
                // If the console is not a sabotage repair console
                if (!__instance.AllowImpostor && isNeutral)
                {
                    __result = float.MaxValue;
                    canUse = couldUse = false;
                }
            }
        }
    }
}
