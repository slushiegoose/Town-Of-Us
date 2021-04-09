using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Sheriff))
            {
                var sheriff = (Roles.Sheriff) role;
                sheriff.LastKilled = DateTime.UtcNow;
            }
            
        }
    }
}