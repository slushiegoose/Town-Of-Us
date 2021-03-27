using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        
        public static void Postfix(MeetingHud __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Sheriff))
            {
                var sheriff = (Roles.Sheriff) role;
                sheriff.LastKilled = DateTime.UtcNow;
                sheriff.LastKilled = sheriff.LastKilled.AddSeconds(8.0);
            }
            
        }
    }
}