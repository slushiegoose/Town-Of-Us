using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    public class PlayerTabPatch
    {
        public static bool Prefix(PlayerTab __instance)
        {
            PlayerControl.SetPlayerMaterialColors(PlayerControl.LocalPlayer.Data.ColorId, __instance.DemoImage);
            __instance.HatImage.SetHat(SaveManager.LastHat, PlayerControl.LocalPlayer.Data.ColorId);
            PlayerControl.SetSkinImage(SaveManager.LastSkin, __instance.SkinImage);
            PlayerControl.SetPetImage(SaveManager.LastPet, PlayerControl.LocalPlayer.Data.ColorId, __instance.PetImage);
            var colors = Palette.PlayerColors;
            var num = colors.Length / 4f;
            for (int i = 0;i < colors.Length;i++)
            {
                var x = __instance.XRange.Lerp((i % 4) / 4f) + 0.25f;
                var y = __instance.YStart - (i / 4) * 0.55f;
                var colorChip = Object.Instantiate(__instance.ColorTabPrefab, __instance.ColorTabArea, true);
                colorChip.transform.localScale *= 0.8f;
                colorChip.transform.localPosition = new Vector3(x, y, -1f);
                var colorId = (byte)i;
                colorChip.Button.OnClick.AddListener((UnityAction)delegate ()
                {
                    __instance.SelectColor(colorId);
                    if (colorId <= 17) SaveManager.BodyColor = colorId;
                });
                colorChip.Inner.color = colors[i];
                __instance.ColorChips.Add(colorChip);
            }
            return false;
        }
    }
}
