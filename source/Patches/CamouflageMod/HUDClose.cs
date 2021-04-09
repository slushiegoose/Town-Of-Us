using System;
using HarmonyLib;

namespace TownOfUs.CamouflageMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Camouflager))
            {
                var camouflager = (Roles.Camouflager) role;
                camouflager.LastCamouflaged = DateTime.UtcNow;
                camouflager.LastCamouflaged = camouflager.LastCamouflaged.AddSeconds(-10f);
            }
        }
    }
}