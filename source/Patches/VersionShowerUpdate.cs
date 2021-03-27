using HarmonyLib;

namespace TownOfUs {

    [HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(VersionShower), "Start")]
    public static class VersionShowerUpdate 
    {

        public static void Postfix(VersionShower __instance)
        {
            var text = __instance.text;
            //text.Text += "\nloaded [FFBFCCFF]T[FF80D5FF]o[FFCC00FF]w[704FA8FF]n[FF0000FF] of[CC4D00FF] Us [FFFFFFFF]by [00FF00FF]slushiegoose[FFFFFFFF] ";
            text.Text += "\n\n\n\n\n\n\n[00FF00FF]loaded TownOfUs Mod v1.0.3 by slushiegoose[]";
        }
    }
}