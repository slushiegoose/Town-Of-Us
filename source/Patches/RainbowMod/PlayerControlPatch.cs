using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
        public static bool SetMaterialPatch([HarmonyArgument(0)] int colorId, [HarmonyArgument(1)] Renderer rend)
        {
            var r = rend.gameObject.GetComponent<RainbowBehaviour>()
                ?? rend.gameObject.AddComponent<RainbowBehaviour>();

            r.AddRend(rend, colorId);
            return !RainbowUtils.IsRainbow(colorId);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControl.CmdCheckColor))]
        public static void CmdPatch(PlayerControl __instance, [HarmonyArgument(0)] byte bodyColor)
        {
            if (__instance != PlayerControl.LocalPlayer) return;
            var savedValue = PluginSingleton<TownOfUs>.Instance.CustomColor.Value;

            if (bodyColor <= 17 && savedValue != bodyColor)
                __instance?.CmdCheckColor(savedValue);
        }
    }
}
