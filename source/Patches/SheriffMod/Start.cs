using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Sheriff))
            {
                var sheriff = (Roles.Sheriff) role;
                sheriff.LastKilled = DateTime.UtcNow;
                sheriff.LastKilled = sheriff.LastKilled.AddSeconds(-8.0);
            }
        }
    }
}