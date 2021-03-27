using System;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    class MeetingExiledEnd
    {
        static void Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance != null && obj == ExileController.Instance.gameObject)
            {
                var exiled = ExileController.Instance.exiled;
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
}