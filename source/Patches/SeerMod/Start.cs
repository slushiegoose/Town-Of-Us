using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.SeerMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Seer))
            {
                var seer = (Roles.Seer) role;
                seer.LastInvestigated = DateTime.UtcNow;
                
            }
        }
    }
}