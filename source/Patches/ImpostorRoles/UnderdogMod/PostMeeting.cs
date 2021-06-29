using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.UnderdogMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class HUDClose
    {
        public static void Postfix()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            if (localPlayer.Is(RoleEnum.Underdog))
                localPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown * (
                    PerformKill.LastImp() ? 0.5f : 1.5f
                ));
        }
    }
}
