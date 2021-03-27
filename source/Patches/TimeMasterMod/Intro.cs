using HarmonyLib;
using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isTimeMaster()) return;
            __instance.__this.Title.Text = "Time Master";
            __instance.__this.Title.scale /= 1.4f;
            __instance.__this.Title.Color = new Color(0f, 0f, 1f, 1f);;
            __instance.__this.ImpostorText.Text = "Rewind time";
            __instance.__this.BackgroundBar.material.color = new Color(0f, 0f, 1f, 1f);;
        }
    }
}