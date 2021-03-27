using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isMayor()) return;
            __instance.__this.Title.Text = "Mayor";
            __instance.__this.Title.Color = new Color(0.44f, 0.31f, 0.66f, 1f);
            __instance.__this.ImpostorText.Text = "Save your votes to double vote";
            __instance.__this.BackgroundBar.material.color = new Color(0.44f, 0.31f, 0.66f, 1f);
        }
    }
}