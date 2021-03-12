using HarmonyLib;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetInfected))]
    public class NoButtons
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer.isSwapper())
            {
                PlayerControl.LocalPlayer.RemainingEmergencies = 0;
            }
        }
    }
}