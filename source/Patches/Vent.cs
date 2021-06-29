using HarmonyLib;
using TownOfUs.Roles;

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
        public static bool Prefix(Vent __instance, out float __result,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            [HarmonyArgument(1)] out bool canUse,
            [HarmonyArgument(2)] out bool couldUse)
        {
            __result = float.MaxValue;

            var player = playerInfo.Object;
            if (player.Is(RoleEnum.Morphling)
                || player.Is(RoleEnum.Swooper)
                || (player.Is(RoleEnum.Phantom) && !player.CanMove)
                || (player.Is(RoleEnum.Undertaker) && CheckUndertaker(player)))
            {
                return canUse = couldUse = false;
            }


            if (player.Is(RoleEnum.Engineer) && !player.Data.IsDead)
            {
                
                canUse = couldUse = false;
                playerInfo.IsImpostor = true;
                return true;
            }
            
            canUse = couldUse = false;
            return true;
        }

        public static void Postfix(Vent __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
        {
            if (playerInfo.Object.Is(RoleEnum.Engineer))
            {
                playerInfo.IsImpostor = false;
            }
        }
    }
}