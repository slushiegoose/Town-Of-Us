using System;
using HarmonyLib;

namespace TownOfUs.CamouflageMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix()
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Camouflager))
            {
                var camouflager = (Roles.Camouflager) role;
                camouflager.LastCamouflaged = DateTime.UtcNow;
                camouflager.LastCamouflaged = camouflager.LastCamouflaged.AddSeconds(-10f);
            }
        }
    }
}