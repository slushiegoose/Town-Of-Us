using System;
using HarmonyLib;

namespace TownOfUs.ArsonistMod
{
    
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Arsonist))
            {
                var arsonist = (Roles.Arsonist) role;
                arsonist.LastDoused = DateTime.UtcNow;
                arsonist.LastDoused = arsonist.LastDoused.AddSeconds(-10.0);
            }
        }
    }
}