using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isShifter()) return;
            __instance.__this.Title.Text = "Shifter";
            __instance.__this.Title.Color = new Color(0.6f, 0.6f, 0.6f, 1f);
            __instance.__this.ImpostorText.Text = "Shift around different roles";
            __instance.__this.BackgroundBar.material.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        }
    }
}