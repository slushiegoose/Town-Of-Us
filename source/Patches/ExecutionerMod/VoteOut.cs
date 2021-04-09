using System;
using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.ExecutionerMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    class MeetingExiledEnd
    {
        static void Postfix(ExileController __instance)
        {
                var exiled = __instance.exiled;
                if (exiled == null) return;
                var player = exiled.Object;

                foreach (var role in Roles.Role.GetRoles(RoleEnum.Executioner))
                {
                    if (player.PlayerId == ((Executioner) role).target.PlayerId)
                    {
                        ((Executioner) role).Wins();
                    }
                }
        }
    }
}