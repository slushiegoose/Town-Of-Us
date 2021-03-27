using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.MafiaMod.Janitor
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Janitor))
            {
                var janitor = (Roles.Janitor) role;
                janitor.LastCleaned = DateTime.UtcNow;
                janitor.LastCleaned = janitor.LastCleaned.AddSeconds(-20.0);
            }
        }
    }
}