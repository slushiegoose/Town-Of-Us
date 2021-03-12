using HarmonyLib;

namespace TownOfUs {

    [HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTrackerUpdate 
    {
        public static void Postfix(PingTracker __instance)
        {
            //__instance.text.Text += "\n> [FFBFCCFF]T[FF80D5FF]o[FFCC00FF]w[704FA8FF]n[FF0000FF] of[CC4D00FF] Us [FFFFFFFF] <\nBy [00FF00FF]slushiegoose[FFFFFFFF]";
            __instance.text.Text += "\n[00FF00FF]TownOfUs Mod\nv1.0.3\nBy slushiegoose []";
        }
        
    }
}