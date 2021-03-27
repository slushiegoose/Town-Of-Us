using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.MafiaMod.Janitor
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix(HudManager __instance)
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