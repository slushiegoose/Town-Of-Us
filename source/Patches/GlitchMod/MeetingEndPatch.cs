using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.GlitchMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    class MeetingExiledEnd
    {
        static void Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance != null && obj == ExileController.Instance.gameObject)
            {
                var glitch = Roles.Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
                if (glitch != null)
                    ((Roles.Glitch)glitch).LastKill = DateTime.UtcNow;
            }
        }
    }
}
