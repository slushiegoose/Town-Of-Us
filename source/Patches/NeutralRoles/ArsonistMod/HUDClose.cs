using System;
using HarmonyLib;
using TownOfUs.Roles;
using Object = UnityEngine.Object;

namespace TownOfUs.NeutralRoles.ArsonistMod
{
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Role.GetRoles(RoleEnum.Arsonist))
            {
                var arsonist = (Arsonist) role;
                arsonist.LastDoused = DateTime.UtcNow;
                // Stops arsonist timer from being lowered by 10 seconds after meetings
                // arsonist.LastDoused = arsonist.LastDoused.AddSeconds(-10.0);
            }
        }
    }
}