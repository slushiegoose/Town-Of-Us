using System;
using HarmonyLib;

namespace TownOfUs.MafiaMod.Janitor
{
    
    [HarmonyPatch(typeof(ShipStatus))]
    public static class Start
    {
        
        [HarmonyPatch("Start")]
        public static void Postfix(ShipStatus __instance)
        {
            PerformKillButton.LastCleaned = DateTime.UtcNow;
            PerformKillButton.LastCleaned = PerformKillButton.LastCleaned.AddSeconds(-20.0);
        }
    }
}