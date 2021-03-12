using System;
using HarmonyLib;

namespace TownOfUs.TimeMasterMod
{
    
    [HarmonyPatch(typeof(ShipStatus))]
    public static class Start
    {
        
        [HarmonyPatch("Start")]
        public static void Postfix(ShipStatus __instance)
        {
            Methods.FinishRewind = DateTime.UtcNow;
            Methods.StartRewind = DateTime.UtcNow;
            Methods.FinishRewind = Methods.FinishRewind.AddSeconds(-10.0);
            Methods.StartRewind = Methods.StartRewind.AddSeconds(-20.0);
        }
    }
}