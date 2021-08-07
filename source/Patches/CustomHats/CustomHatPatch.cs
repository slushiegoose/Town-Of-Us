using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reactor.Extensions;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.Patches.CustomHats
{
    [HarmonyPatch(typeof(HatsTab), nameof(HatsTab.OnEnable))]
    public static class HatsTab_OnEnable
    {
        public static void Postfix(HatsTab __instance)
        {
            var allHats = DestroyableSingleton<HatManager>.Instance.GetUnlockedHats();
            var hatGroups = new SortedList<string, List<HatBehaviour>>(
                new PaddedComparer<string>("Vanilla", "")
            );

            foreach (var hat in allHats)
            {
                if (!hatGroups.ContainsKey(hat.StoreName)) 
                    hatGroups[hat.StoreName] = new List<HatBehaviour>();
                hatGroups[hat.StoreName].Add(hat);
            }

            foreach (ColorChip instanceColorChip in __instance.ColorChips) 
                instanceColorChip.gameObject.Destroy();

            __instance.ColorChips.Clear();

            
            var groupNameText = __instance.transform.parent.parent.GetComponentInChildren<GameSettingMenu>(true).GetComponentInChildren<TextMeshPro>(true);
            
            int hatIdx = 0;
            foreach ((string groupName, List<HatBehaviour> hats) in hatGroups)
            {
                var text = Object.Instantiate(groupNameText.gameObject);
                text.transform.localScale = Vector3.one;
                text.transform.parent = __instance.scroller.Inner;

                var tmp = text.GetComponent<TextMeshPro>();
                tmp.text = groupName;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = 3f;
                tmp.fontSizeMax = 3f;
                tmp.fontSizeMin = 0f;
                
                hatIdx = (hatIdx + 3) / 4 * 4;

                float xLerp = __instance.XRange.Lerp(0.5f);
                float yLerp = __instance.YStart - (hatIdx / __instance.NumPerRow) * __instance.YOffset;
                text.transform.localPosition = new Vector3(xLerp, yLerp, -1f);
                
                hatIdx += 4;
                foreach (var hat in hats.OrderBy(HatManager.Instance.GetIdFromHat))
                {
                    float num = __instance.XRange.Lerp(hatIdx % __instance.NumPerRow / (__instance.NumPerRow - 1f));
                    float num2 = __instance.YStart - hatIdx / __instance.NumPerRow * __instance.YOffset;
                    ColorChip colorChip = Object.Instantiate(__instance.ColorTabPrefab, __instance.scroller.Inner);
                    colorChip.transform.localPosition = new Vector3(num, num2, -1f);
                    colorChip.Button.OnClick.AddListener((Action)(() => __instance.SelectHat(hat)));
                    colorChip.Inner.SetHat(hat, PlayerControl.LocalPlayer.Data.ColorId);
                    colorChip.Inner.transform.localPosition = hat.ChipOffset + new Vector2(0f, -0.3f);
                    colorChip.Tag = hat;
                    __instance.ColorChips.Add(colorChip);
                    hatIdx += 1;
                }

            }
            
            __instance.scroller.YBounds.max = -(__instance.YStart - (hatIdx + 1) / __instance.NumPerRow * __instance.YOffset) - 3f;
        }
    }
}
