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
    [HarmonyPatch(typeof(PlayerTab))]
    public static class PlayerTabPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerTab.OnEnable))]
        public static void OnEnablePatch(PlayerTab __instance)
        {
            foreach (ColorChip instanceColorChip in __instance.ColorChips)
                instanceColorChip.gameObject.Destroy();

            __instance.ColorChips.Clear();

            var colors = Palette.PlayerColors;
            var num = colors.Length / 4f;

            for (int i = 0; i < colors.Length; i++)
            {
                var x = __instance.XRange.Lerp((i % 4) / 4f) + 0.26f;
                var y = (__instance.YStart - 0.16f) - (i / 4) * 0.5f;
                var colorChip = Object.Instantiate(__instance.ColorTabPrefab, __instance.ColorTabArea, true);
                colorChip.transform.localScale *= 0.8f;
                colorChip.transform.localPosition = new Vector3(x, y, -1f);
                var colorId = (byte)i;

                colorChip.Button.OnClick.AddListener((Action)(() =>
                {
                    if (colorId <= 17) __instance.SelectColor(colorId);
                    __instance.SelectCustomColor(colorId);
                    __instance.HatImage.SetColor(colorId);
                }));

                colorChip.Inner.color = colors[i];
                __instance.ColorChips.Add(colorChip);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerTab.Update))]
        public static void UpdatePatch(PlayerTab __instance)
        {
            for (int i = 0; i < __instance.ColorChips.Count; i++)
            {
                if (RainbowUtils.IsRainbow(i))
                {
                    __instance.ColorChips[i].Inner.color = RainbowUtils.Rainbow;
                    break;
                }
            }
        }

        public static void SelectCustomColor(this PlayerTab playerTab, byte colorId)
        {
            PluginSingleton<TownOfUs>.Instance.CustomColor.Value = colorId;
            if (colorId <= 17) return;

            playerTab.UpdateAvailableColors();

            PlayerControl.LocalPlayer.CmdCheckColor(colorId);
        }
    }
}
