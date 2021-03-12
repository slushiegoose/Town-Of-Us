using System;
using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public static class HUDClose
    {
        
        [HarmonyPatch("Close")]
        public static void Postfix(HudManager __instance)
        {
            Methods.LastShifted = DateTime.UtcNow;
            Methods.LastShifted = Methods.LastShifted.AddSeconds(-10.0);
        }
    }
}