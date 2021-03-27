using System;
using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.ExecutionerMod
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
}