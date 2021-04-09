using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MorphlingMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.SetTarget))]
    public class SetTarget
    {
        public static void Postfix(KillButtonManager __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Morphling)) return;
            var role = Roles.Role.GetRole<Roles.Morphling>(PlayerControl.LocalPlayer);
            if (target != null && __instance == DestroyableSingleton<HudManager>.Instance.KillButton)
            {
                if (target.Data.IsImpostor)
                {
                    __instance.renderer.color = Palette.DisabledClear;
                    __instance.renderer.material.SetFloat("_Desat", 1f);
                }
            }
        }
    }
}