using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.InvestigatorMod
{
    [HarmonyPatch]
    public class EndGame
    {
        public static void Reset()
        {
            var investigator = Role.GetRole<Investigator>();
            investigator?.AllPrints.Clear();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame))]
        public static void ExitGame(AmongUsClient __instance) => Reset();

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
        public static void EndStart(EndGameManager __instance) => Reset();
    }
}
