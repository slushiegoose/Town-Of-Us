using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Shifter))
            {
                var shifter = (Roles.Shifter) role;
                shifter.LastShifted = DateTime.UtcNow;
                shifter.LastShifted = shifter.LastShifted.AddSeconds(-10.0);
            }
        }
    }
}