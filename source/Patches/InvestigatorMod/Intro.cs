using HarmonyLib;
using UnityEngine;

namespace TownOfUs.InvestigatorMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isInvestigator()) return;
            __instance.__this.Title.Text = "Investigator";
            __instance.__this.Title.scale /= 1.4f;
            __instance.__this.Title.Color = new Color(0f, 0.7f, 0.7f, 1f);
            __instance.__this.ImpostorText.Text = "Find all imposters by examining footprints";
            __instance.__this.BackgroundBar.material.color = new Color(0f, 0.7f, 0.7f, 1f);
        }
    }
}