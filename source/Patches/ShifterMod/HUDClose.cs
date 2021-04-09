using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Shifter))
            {
                var shifter = (Roles.Shifter) role;
                shifter.LastShifted = DateTime.UtcNow;
                shifter.LastShifted = shifter.LastShifted.AddSeconds(-10.0);
            }
        }
    }
}