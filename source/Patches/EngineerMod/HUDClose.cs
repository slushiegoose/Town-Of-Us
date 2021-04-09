using System;
using HarmonyLib;

namespace TownOfUs.EngineerMod
{
    public enum EngineerFixPer
    {
        Round,
        Game,
    }
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Engineer))
            {
                var engineer = (Roles.Engineer) role;
                if (CustomGameOptions.EngineerFixPer == EngineerFixPer.Round)
                {
                    engineer.UsedThisRound = false;
                }
            }
            
        }
    }
}