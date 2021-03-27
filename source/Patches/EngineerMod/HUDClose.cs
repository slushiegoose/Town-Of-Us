using System;
using HarmonyLib;

namespace TownOfUs.EngineerMod
{
    public enum EngineerFixPer
    {
        Round,
        Game,
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix(MeetingHud __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Engineer))
            {
                var engineer = (Roles.Engineer) role;
                if (CustomGameOptions.EngineerFixPer == EngineerFixPer.Round)
                {
                    engineer.UsedThisRound = false;
                }
            }
            
        }
    }
}