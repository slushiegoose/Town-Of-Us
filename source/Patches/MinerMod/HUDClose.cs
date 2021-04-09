using System;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MinerMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Miner))
            {
                var role = Roles.Role.GetRole<Roles.Miner>(PlayerControl.LocalPlayer);
                role.LastMined = DateTime.UtcNow;
                
            }

        }
    }
}