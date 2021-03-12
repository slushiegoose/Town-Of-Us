using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.LoversMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d))]
    public static class Intro
    {
        
        [HarmonyPatch("MoveNext")]
        public static bool Prefix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isLover()) return true;
            
            var loverTeam = new List<PlayerControl>();
            loverTeam.Add(PlayerControl.LocalPlayer);
            loverTeam.Add(PlayerControl.LocalPlayer.OtherLover());
            __instance.yourTeam = loverTeam;

            return true;

        }
        [HarmonyPatch("MoveNext")]
        public static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            if (!PlayerControl.LocalPlayer.isLover()) return;
            __instance.__this.Title.Text = "Lover";
            
            if (PlayerControl.LocalPlayer.Data.IsImpostor)
            {
                __instance.__this.Title.Text = "Loving Impostor";
                __instance.__this.Title.scale /= 2.3f;
            }
            __instance.__this.Title.Color = new Color(1f, 0.4f, 0.8f, 1f);
            __instance.__this.ImpostorText.Text = "You are in [FF66CCFF]Love [FFFFFFFF]with [FF66CCFF]" +
                                                  PlayerControl.LocalPlayer.OtherLover().name;
            
            __instance.__this.BackgroundBar.material.color = new Color(1f, 0.4f, 0.8f, 1f);
        }
    }
}