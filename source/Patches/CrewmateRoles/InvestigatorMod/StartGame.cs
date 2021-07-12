using HarmonyLib;

namespace TownOfUs.CrewmateRoles.InvestigatorMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class StartGame
    {
        public static void Postfix() => AddPrints.GameStarted = true;
    }
}
