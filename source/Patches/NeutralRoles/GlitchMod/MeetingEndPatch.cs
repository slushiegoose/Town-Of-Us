using System;
using System.Linq;
using HarmonyLib;
using TownOfUs.Roles;
using Object = UnityEngine.Object;

namespace TownOfUs.NeutralRoles.GlitchMod
{
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    internal class MeetingExiledEnd
    {
        private static void Prefix(Object obj)
        {
            if (ExileController.Instance != null && obj == ExileController.Instance.gameObject)
            {
                var glitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
                if (glitch != null)
                    ((Glitch) glitch).LastKill = DateTime.UtcNow;
            }
        }
    }
}