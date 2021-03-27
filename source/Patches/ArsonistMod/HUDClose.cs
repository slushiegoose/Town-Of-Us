using System;
using HarmonyLib;

namespace TownOfUs.ArsonistMod
{
    
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        
        public static void Postfix(MeetingHud __instance)
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