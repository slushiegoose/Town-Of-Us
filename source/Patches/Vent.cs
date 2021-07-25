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
            var role = Role.GetRole<Undertaker>(player);
            return player.Data.IsDead || role.CurrentlyDragging != null;
        }
        public static bool Prefix(Vent __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            [HarmonyArgument(1)] ref bool canUse,
            [HarmonyArgument(2)] ref bool couldUse,
            ref float __result)
        {
            __result = float.MaxValue;
            canUse = couldUse = false;

            var player = playerInfo.Object;
            if (player.inVent)
            {
                __result = Vector2.Distance(player.Collider.bounds.center, __instance.transform.position);
                canUse = couldUse = true;
                return false;
            }
                

            if (player.Is(RoleEnum.Morphling)
                || player.Is(RoleEnum.Swooper)
                || (player.Is(RoleEnum.Undertaker) && Role.GetRole<Undertaker>(player).CurrentlyDragging != null))
                return false;


            if (player.Is(RoleEnum.Engineer))
                playerInfo.IsImpostor = true;
            
            return true;
        }

        public static void Postfix(Vent __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
        {
            if (playerInfo.Object.Is(RoleEnum.Engineer))
                playerInfo.IsImpostor = false;
        }
    }
}