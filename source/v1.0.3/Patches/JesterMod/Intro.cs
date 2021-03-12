using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d))]
    public static class Intro
    {
        
        [HarmonyPatch("MoveNext")]
        public static bool Prefix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isJester()) return true;
            
            var jesterTeam = new List<PlayerControl>();
            jesterTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = jesterTeam;

            return true;

        }
        [HarmonyPatch("MoveNext")]
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isJester()) return;
            __instance.__this.Title.Text = "Jester";
            __instance.__this.Title.Color = new Color(1f, 0.75f, 0.8f, 1f);
            __instance.__this.ImpostorText.Text = "Get voted out";
            __instance.__this.BackgroundBar.material.color = new Color(1f, 0.75f, 0.8f, 1f);
        }
    }
}