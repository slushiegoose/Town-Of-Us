using HarmonyLib;

namespace TownOfUs.CrewmateRoles.SwapperMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetInfected))]
    public class NoButtons
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) PlayerControl.LocalPlayer.RemainingEmergencies = 0;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
    public class NoButtonsHost
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) PlayerControl.LocalPlayer.RemainingEmergencies = 0;
        }
    }
}