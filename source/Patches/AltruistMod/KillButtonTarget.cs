using HarmonyLib;
using UnityEngine;

namespace TownOfUs.AltruistMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.SetTarget))]
    public class KillButtonTarget
    {

        public static byte DontRevive = byte.MaxValue;
        public static bool Prefix(KillButtonManager __instance)
        {
            return !PlayerControl.LocalPlayer.Is(RoleEnum.Altruist);
        }

        public static void SetTarget(KillButtonManager __instance, DeadBody target, Roles.Altruist role)
        {
            if (role.CurrentTarget && role.CurrentTarget != target)
            {
                role.CurrentTarget.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 0f);
            }

            
            if (target != null && target.ParentId == DontRevive) target = null;
            role.CurrentTarget = target;
            if (role.CurrentTarget && __instance.enabled)
            {
                var component = role.CurrentTarget.GetComponent<SpriteRenderer>();
                component.material.SetFloat("_Outline", 1f);
                component.material.SetColor("_OutlineColor", Color.red);
                __instance.renderer.color = Palette.EnabledColor;
                __instance.renderer.material.SetFloat("_Desat", 0f);
                return;
            }

            __instance.renderer.color = Palette.DisabledClear;
            __instance.renderer.material.SetFloat("_Desat", 1f);
        }
    }
}