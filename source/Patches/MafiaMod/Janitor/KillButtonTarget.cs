using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{
    [HarmonyPatch(typeof(KillButtonManager))]
    public class KillButtonTarget
    {
        public static DeadBody CurrentTarget;
        
        [HarmonyPatch(nameof(KillButtonManager.SetTarget))]
        public static bool Prefix(KillButtonManager __instance)
        {
            return !PlayerControl.LocalPlayer.isJanitor();
        }

        public static void SetTarget(KillButtonManager __instance, DeadBody target)
        {
            if (CurrentTarget && CurrentTarget != target)
            {
                CurrentTarget.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 0f);
            }

            CurrentTarget = target;
            if (CurrentTarget)
            {
                var component = CurrentTarget.GetComponent<SpriteRenderer>();
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