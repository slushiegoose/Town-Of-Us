using System;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Object = Il2CppSystem.Object;

namespace TownOfUs
{
    public class DumbAirshipRegionText
    {
        [HarmonyPatch(typeof(MatchMaker), nameof(MatchMaker.Start))]
        public static class MatchMakerPatch
        {
            public static void Prefix(MatchMaker __instance)
            {
                var parent = __instance.GetComponentInParent<Transform>().parent; // Returns NormalMenu
                var textmeshes = parent.GetComponentsInChildren<TextMeshPro>();
                foreach (var textmesh in textmeshes)
                    if (textmesh.name == "RegionText_TMP")
                    {
                        var region = DestroyableSingleton<ServerManager>.Instance.CurrentRegion;

                        var name = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(
                            region.TranslateName, region.Name, Array.Empty<Object>());
                        textmesh.text = name;
                        break;
                    }
            }
        }
    }
}