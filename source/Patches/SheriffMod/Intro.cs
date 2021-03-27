using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class Intro
    {
        
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isSheriff()) return;
            __instance.__this.Title.Text = "Sheriff";
            __instance.__this.Title.Color = new Color(1f, 1f, 0f, 1f);
            __instance.__this.ImpostorText.Text = "Shoot the [FF0000FF]Impostor";
            __instance.__this.BackgroundBar.material.color = new Color(1f, 1f, 0f, 1f);
        }
    }
}