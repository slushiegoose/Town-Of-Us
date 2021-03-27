using System;
using HarmonyLib;

namespace TownOfUs.CamouflageMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Camouflager))
            {
                var seer = (Roles.Camouflager) role;
                seer.LastCamouflaged = DateTime.UtcNow;
                
            }
        }
    }
}