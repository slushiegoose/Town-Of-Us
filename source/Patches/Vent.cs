using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;
using System.Linq;

namespace TownOfUs
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class PlayerVentTimeExtension
    {
        private static bool CheckUndertaker(PlayerControl player)
        {
            var role = Role.GetRole(player);
            return
                role.RoleType != RoleEnum.Undertaker ||
                ((Undertaker)role).CurrentlyDragging == null;
        }
        public static bool Prefix(Vent __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            [HarmonyArgument(1)] ref bool canUse,
            [HarmonyArgument(2)] ref bool couldUse,
            ref float __result)
        {
            __result = float.MaxValue;
            var player = playerInfo.Object;
            var center = player.Collider.bounds.center;
            var position = __instance.transform.position;
            if (player.inVent)
            {
                couldUse = canUse = true;
                __result = Vector2.Distance(center, position);
                return false;
            }

            var isImpostor = player.Data.IsImpostor;

            if (player.AmOwner && !isImpostor)
            {
                var task = player.myTasks.ToArray().FirstOrDefault(
                    tsk => tsk.TaskType == TaskTypes.VentCleaning
                );
                if (task?.FindConsoles()[0].ConsoleId == __instance.Id)
                {
                    return couldUse = canUse = false;
                }
            }

            couldUse = canUse = !playerInfo.IsDead && player.CanMove;
            if (!canUse) return false;
            if (isImpostor)
            {
                couldUse = canUse = !player.IsAny(new RoleEnum[] {
                    RoleEnum.Swooper, RoleEnum.Morphling
                }) && CheckUndertaker(player);
            }
            else
            {
                couldUse = canUse = player.Is(RoleEnum.Engineer);
            }

            if (canUse)
            {
                __result = Vector2.Distance(center, position);
                canUse &= __result <= __instance.UsableDistance &&
                    !PhysicsHelpers.AnythingBetween(
                        player.Collider, center, position, Constants.ShipOnlyMask, false
                     );
            }

            return false;
        }
    }
}
