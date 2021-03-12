using System;
using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    
    [HarmonyPatch(typeof(ShipStatus))]
    public static class Start
    {
        
        [HarmonyPatch("Start")]
        public static void Postfix(ShipStatus __instance)
        {
            Methods.LastKilled = DateTime.UtcNow;
            Methods.LastKilled = Methods.LastKilled.AddSeconds(-10.0);
        }
    }
}