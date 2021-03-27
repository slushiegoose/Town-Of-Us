using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.SetTarget))]
    public class KillButtonTarget
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Janitor)) return true;
            return CustomGameOptions.JanitorKill && Utils.IsLastImp(PlayerControl.LocalPlayer);
        }

        public static void SetTarget(KillButtonManager __instance, DeadBody target, Roles.Janitor role)
        {
            if (role.CurrentTarget && role.CurrentTarget != target)
            {
                role.CurrentTarget.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 0f);
            }

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

            __instance.renderer.color = Palette.DisabledColor;
            __instance.renderer.material.SetFloat("_Desat", 1f);
        }
    }
}