using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.UnderdogMod
{
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Underdog))
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown *
                                                       (PerformKill.LastImp() ? 0.5f : 1.5f));
        }
    }
}