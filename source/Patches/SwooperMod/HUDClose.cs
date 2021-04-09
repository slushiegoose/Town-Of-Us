using System;
using HarmonyLib;

namespace TownOfUs.SwooperMod
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class HUDClose
    {

        public static void Postfix(UnityEngine.Object obj)
        { 
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swooper))
            {
                var role = Roles.Role.GetRole<Roles.Swooper>(PlayerControl.LocalPlayer);
                role.LastSwooped = DateTime.UtcNow;
                
            }

        }
    }
}