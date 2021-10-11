using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.ExecutionerMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    internal class MeetingExiledEnd
    {
        private static void Postfix(ExileController __instance)
        {
            var exiled = __instance.exiled;
            if (exiled == null) return;

            var exec = Role.GetRole<Executioner>();

            if (exec?.Target?.PlayerId == exiled.PlayerId)
                exec.Wins();
        }
    }
}
