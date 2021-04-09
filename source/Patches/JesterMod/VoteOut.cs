using System;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    class MeetingExiledEnd
    {
        static void Postfix(ExileController __instance)
        {
            var exiled = __instance.exiled;
            if (exiled == null) return;
            var player = exiled.Object;

            var role = Role.GetRole(player);
            if (role == null) return;
            if (role.RoleType == RoleEnum.Jester)
            {
                ((Jester) role).Wins();
            }
        }
    }
}