using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(AirshipStatus), nameof(AirshipStatus.OnMeetingCalled))]
    public class DumbAirshipMovement
    {
        public static void Postfix(AirshipStatus __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls) player.NetTransform.Halt();
        }
    }
}