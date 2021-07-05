using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;
using TownOfUs.ImpostorRoles.UnderdogMod;

namespace TownOfUs
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    public static class ExtendedKillCooldown
    {
        public static float GetMaxCooldown(Role role, float timer)
        {
            var max = role.RoleType switch
            {
                RoleEnum.Sheriff => CustomGameOptions.SheriffKillCd,
                RoleEnum.Underdog => ((Underdog)role).MaxTimer(),
                RoleEnum.Glitch => CustomGameOptions.GlitchKillCooldown,
                _ => PlayerControl.GameOptions.KillCooldown
            };

            // Timer should only be bigger than max if killed player was diseased
            if (timer > max) max *= 3;

            return max;
        }

        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] float time)
        {
            var role = Role.GetRole(__instance);
            if (role == null) return true;
            var maxTimer = GetMaxCooldown(role, time);
            __instance.killTimer = Mathf.Clamp(time, 0, maxTimer);
            HudManager.Instance.KillButton.SetCoolDown(__instance.killTimer, maxTimer);
            return false;
        }
    }
}
