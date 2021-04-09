using System;
using HarmonyLib;

namespace TownOfUs.JanitorMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    public static class HUDClose
    {
        public static void Postfix(PlayerControl __instance)
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