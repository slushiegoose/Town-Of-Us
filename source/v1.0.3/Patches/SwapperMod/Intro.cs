using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isSwapper()) return;
            __instance.__this.Title.Text = "Swapper";
            __instance.__this.Title.Color = new Color(0.4f, 0.9f, 0.4f, 1f);
            __instance.__this.ImpostorText.Text = "Swap the votes of two people";
            __instance.__this.BackgroundBar.material.color = new Color(0.4f, 0.9f, 0.4f, 1f);
        }
    }
}