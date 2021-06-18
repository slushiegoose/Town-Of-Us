using System;
using HarmonyLib;
using TownOfUs.Roles;
using Object = UnityEngine.Object;

namespace TownOfUs.CrewmateRoles.SeerMod
{
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Role.GetRoles(RoleEnum.Seer))
            {
                var seer = (Seer) role;
                seer.LastInvestigated = DateTime.UtcNow;
                seer.LastInvestigated = seer.LastInvestigated.AddSeconds(-10.0);
            }
        }
    }
}