using System;
using HarmonyLib;

namespace TownOfUs.MafiaMod.Janitor
{
    [HarmonyPatch(typeof(MeetingHud))]
    public static class HUDClose
    {
        
        [HarmonyPatch("Close")]
        public static void Postfix(HudManager __instance)
        {
            PerformKillButton.LastCleaned = DateTime.UtcNow;
            PerformKillButton.LastCleaned = PerformKillButton.LastCleaned.AddSeconds(-20.0);
        }
    }
}