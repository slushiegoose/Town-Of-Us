using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class PlayerVentTimeExtension
    {
        private static bool CheckImpostors(PlayerControl player)
        {
            if (player.Is(RoleEnum.Morphling)) return false;
            if (player.Is(RoleEnum.Swooper)) return false;

            return (player.Data.IsImpostor || player.Is(RoleEnum.Engineer)) && !player.Data.IsDead;
        }


        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc,
            [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            var num = float.MaxValue;
            var localPlayer = pc.Object;
            couldUse = CheckImpostors(localPlayer);
            canUse = couldUse;

            num = Vector2.Distance(localPlayer.GetTruePosition(), __instance.transform.position);
            canUse &= num <= __instance.UsableDistance;


            __result = num;
            return false;
        }
    }
}