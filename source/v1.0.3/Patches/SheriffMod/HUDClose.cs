using System;
using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public static class HUDClose
    {
        
        [HarmonyPatch("Close")]
        public static void Postfix(HudManager __instance)
        {
            Methods.LastKilled = DateTime.UtcNow;
            Methods.LastKilled = Methods.LastKilled.AddSeconds(8.0);
        }
    }
}