using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.UndertakerMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    public static class PatchKillTimer
    {
        public static bool Prefix(PlayerControl __instance)
        {
            var role = Role.GetRole(__instance);
            return role?.RoleType != RoleEnum.Undertaker || ((Undertaker)role).CurrentlyDragging == null;
        }
    }
}
