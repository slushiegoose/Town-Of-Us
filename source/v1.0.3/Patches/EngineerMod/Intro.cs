using HarmonyLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isEngineer()) return;
            __instance.__this.Title.Text = "Engineer";
            __instance.__this.Title.Color = new Color(1f, 0.7f, 0f, 1f);
            __instance.__this.ImpostorText.Text = "Help fix sabotages";
            __instance.__this.BackgroundBar.material.color = new Color(1f, 0.7f, 0f, 1f);
        }
    }
}