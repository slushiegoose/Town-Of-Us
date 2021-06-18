using HarmonyLib;

namespace TownOfUs.CrewmateRoles.InvestigatorMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class StartGame
    {
        public static void Postfix(ShipStatus __instance)
        {
            AddPrints.GameStarted = true;
        }
    }
}