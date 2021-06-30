using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

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

        public static bool Prefix(Vent __instance, out float __result,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            [HarmonyArgument(1)] out bool canUse,
            [HarmonyArgument(2)] out bool couldUse)
        {
            __result = float.MaxValue;
            PlayerControl player = playerInfo.Object;
            if (player.inVent)
            {
                couldUse = canUse = true;
                __result = 0f;
                return false;
            }

            couldUse = canUse = !playerInfo.IsDead && player.CanMove;
            if (!canUse) return false;
            if (playerInfo.IsImpostor)
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
                Vector3 center = player.Collider.bounds.center;
                Vector3 position = __instance.transform.position;
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
