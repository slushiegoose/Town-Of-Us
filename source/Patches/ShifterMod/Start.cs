using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Shifter))
            {
                var shifter = (Roles.Shifter) role;
                shifter.LastShifted = DateTime.UtcNow;
            }
        }
    }
}