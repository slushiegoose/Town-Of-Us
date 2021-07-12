using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.EngineerMod
{
    public enum EngineerFixPer
    {
        Round,
        Game
    }

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class PostMeeting
    {
        public static void Postfix()
        {
            var role = Role.GetRole<Engineer>();
            if (role != null) role.UsedThisRound = false;
        }
    }
}
