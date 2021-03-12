using System;
using HarmonyLib;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public static class HUDClose
    {
        
        [HarmonyPatch("Close")]
        public static void Postfix(HudManager __instance)
        {
            Methods.FinishRewind = DateTime.UtcNow;
            Methods.StartRewind = DateTime.UtcNow;
            Methods.FinishRewind = Methods.FinishRewind.AddSeconds(-10.0);
            Methods.StartRewind = Methods.StartRewind.AddSeconds(-20.0);
        }
    }
}