using System;
using HarmonyLib;

namespace TownOfUs.SeerMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Seer))
            {
                var seer = (Roles.Seer) role;
                seer.LastInvestigated = DateTime.UtcNow;
                seer.LastInvestigated = seer.LastInvestigated.AddSeconds(-10.0);
            }
        }
    }
}