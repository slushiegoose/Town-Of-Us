using System;
using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    
    [HarmonyPatch(typeof(ShipStatus))]
    public static class Start
    {
        
        [HarmonyPatch("Start")]
        public static void Postfix(ShipStatus __instance)
        {
            Methods.LastShifted = DateTime.UtcNow;
            Methods.LastShifted = Methods.LastShifted.AddSeconds(-10.0);
        }
    }
}