namespace TownOfUs.RainbowMod
{
    // [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    // public class FourColumnColor
    // {
    //     public static bool Prefix(PlayerTab __instance)
    //     {
    //         PlayerControl.SetPlayerMaterialColors(PlayerControl.LocalPlayer.Data.ColorId, __instance.DemoImage);
    //         __instance.HatImage.SetHat(SaveManager.LastHat, PlayerControl.LocalPlayer.Data.ColorId);
    //         PlayerControl.SetSkinImage(SaveManager.LastSkin, __instance.SkinImage);
    //         PlayerControl.SetPetImage(SaveManager.LastPet, PlayerControl.LocalPlayer.Data.ColorId, __instance.PetImage);
    //         var num = (float)Palette.PlayerColors.Length / 4f;
    //         for (var i = 0; i < Palette.PlayerColors.Length; i++)
    //         {
    //             var xCoord = __instance.XRange.Lerp((float)(i % 4) / 3f);
    //             float yCoord = __instance.YStart - (float)(i / 4) * 0.55f;
    //             var colorChip = Object.Instantiate<ColorChip>(__instance.ColorTabPrefab, __instance.ColorTabArea);
    //             colorChip.transform.localPosition = new Vector3(xCoord, yCoord, -1f);
    //             var j = i;
    //             colorChip.Button.OnClick.AddListener((Action)delegate()
    //             {
    //                 __instance.SelectColor(j);
    //                 SaveManager.BodyColor = (byte)(j < 12 ? j : 0);
    //             });
    //             colorChip.Inner.color = Palette.PlayerColors[i];
    //             __instance.ColorChips.Add(colorChip);
    //         }
    //
    //         return false;
    //     }
    //     
    // }
}