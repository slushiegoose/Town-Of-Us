using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame))]
    public static class EndGamePatch
    {
        /*public static void Prefix(AmongUsClient __instance) {
            Utils.Null();
        }*/
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    [HarmonyPriority(Priority.Last)]
    public static class EndGameManagerPatch
    {
        /*public static bool Prefix(EndGameManager __instance) {
            Utils.Null();
            return true;
        }*/
    }
}