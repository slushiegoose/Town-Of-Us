using Reactor;
using System;
using System.Linq;
using UnityEngine;
using static Palette;

namespace TownOfUs.RainbowMod
{
    public static class PalettePatch
    {
        public static StringNames RainbowStringNames;
        public static void Load()
        {
            RainbowStringNames = CustomStringName.Register("Rainbow");

            PlayerColors = PlayerColors.Concat(new Color32[]
            {
                new Color32(168, 50, 62, byte.MaxValue),
                new Color32(60, 48, 44, byte.MaxValue),
                new Color32(61, 129, 255, byte.MaxValue),
                new Color32(240, 211, 165, byte.MaxValue),
                new Color32(236, 61, 255, byte.MaxValue),
                new Color32(61, 255, 181, byte.MaxValue),
                new Color32(186, 161, 255, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
                new Color32(1, 166, 255, byte.MaxValue)
            }).ToArray();

            ShadowColors = ShadowColors.Concat(new Color32[]
            {
                new Color32(101, 30, 37, byte.MaxValue),
                new Color32(30, 24, 22, byte.MaxValue),
                new Color32(31, 65, 128, byte.MaxValue),
                new Color32(120, 106, 83, byte.MaxValue),
                new Color32(118, 31, 128, byte.MaxValue),
                new Color32(31, 128, 91, byte.MaxValue),
                new Color32(93, 81, 128, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
                new Color32(17, 104, 151, byte.MaxValue),
            }).ToArray();

            ColorNames = ColorNames.Concat(new StringNames[]
            {
                CustomStringName.Register("Watermelon"),
                CustomStringName.Register("Chocolate"),
                CustomStringName.Register("Sky Blue"),
                CustomStringName.Register("Beige"),
                CustomStringName.Register("Hot Pink"),
                CustomStringName.Register("Turquoise"),
                CustomStringName.Register("Lilac"),
                RainbowStringNames,
                CustomStringName.Register("Azure"),
            }).ToArray();
        }
    }
}
