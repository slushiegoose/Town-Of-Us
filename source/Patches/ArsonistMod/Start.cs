using System;
using HarmonyLib;

namespace TownOfUs.ArsonistMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Arsonist))
            {
                var arsonist = (Roles.Arsonist) role;
                arsonist.LastDoused = DateTime.UtcNow;
                arsonist.LastDoused = arsonist.LastDoused.AddSeconds(-10.0);
            }
        }
    }
}