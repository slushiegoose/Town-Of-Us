using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class MeetingHud_Start
    {
        public static void Postfix(MeetingHud __instance)
        {
            Utils.ShowDeadBodies = PlayerControl.LocalPlayer.Data.IsDead;
        }
    }
}