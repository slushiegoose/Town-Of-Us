using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class PlayerVentTimeExtension
    {
        
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc,
            [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            var num = float.MaxValue;
            var localPlayer = pc.Object;
            couldUse = (PlayerControl.LocalPlayer.Is(RoleEnum.Engineer) || localPlayer.Data.IsImpostor && !localPlayer.Is(RoleEnum.Morphling)  && !localPlayer.Is(RoleEnum.Swooper)) &&
                       !localPlayer.Data.IsDead;
            canUse = couldUse;
    
            num = Vector2.Distance(localPlayer.GetTruePosition(), __instance.transform.position);
            canUse &= num <= __instance.UsableDistance;


            __result = num;
            return false;
        }
        
    }
}