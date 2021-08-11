using HarmonyLib;
using Reactor.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(PlayerTab))]
    public class PlayerTabPatch
    {
        [HarmonyPatch(nameof(PlayerTab.OnEnable))]
        [HarmonyPostfix]
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
                    __instance.SelectColor(colorId);
                    SaveManager.BodyColor = colorId <= 17 ? colorId : (byte)0;
                }));

                colorChip.Inner.color = colors[i];
                __instance.ColorChips.Add(colorChip);
            }
        }

        [HarmonyPatch(nameof(PlayerTab.SelectColor))]
        [HarmonyPostfix]
        public static void SelectColorPatch(PlayerTab __instance, [HarmonyArgument(0)] int id)
        {
            __instance.HatImage.SetColor(id);
        }

        [HarmonyPatch(nameof(PlayerTab.Update))]
        [HarmonyPostfix]
        public static void UpdatePatch(PlayerTab __instance)
        {
            for (int i = 0; i < __instance.ColorChips.Count; i++)
            {
                if (RainbowUtils.IsRainbow(i))
                    __instance.ColorChips[i].Inner.color = RainbowUtils.Rainbow;
            }
        }
    }
}
