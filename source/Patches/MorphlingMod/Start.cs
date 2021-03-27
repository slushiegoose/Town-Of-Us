using System;
using HarmonyLib;

namespace TownOfUs.MorphlingMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Morphling))
            {
                var seer = (Roles.Morphling) role;
                seer.LastMorphed = DateTime.UtcNow;
                
            }
        }
    }
}