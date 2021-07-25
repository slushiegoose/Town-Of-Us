using System;
using System.Collections.Generic;
using HarmonyLib;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CustomHats
{
    public class HatCreation
    {
        public const string TouHatIdentifier = "TownOfUsHat";
        
        private static bool modded;

        public static Sprite EmptySprite = TownOfUs.CreateSprite("TownOfUs.Resources.Hats.transparent.png", true);

        public static List<uint> TallIds = new List<uint>();

        protected internal static Dictionary<uint, HatData> IdToData = new Dictionary<uint, HatData>();

        protected internal static Dictionary<HatData, List<Sprite>> AnimatedHats =
            new Dictionary<HatData, List<Sprite>>();

        private static HatBehaviour CreateHat(HatData hat, int id)
        {
            //PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Creating Hat {hat.name}");

            var sprite = hat.new_hat
                ? TownOfUs.CreatePolusHat($"TownOfUs.Resources.Hats.{hat.name}.png")
                : TownOfUs.CreateSprite($"TownOfUs.Resources.Hats.{hat.name}.png", true);

            var newHat = ScriptableObject.CreateInstance<HatBehaviour>();
            // TODO: Used as a global identifier that this hatbehaviour was created by TOU. Change later
            newHat.StoreName = TouHatIdentifier;
            newHat.MainImage = sprite;
            newHat.ProductId = hat.name;
            newHat.Order = 99 + id;
            newHat.InFront = true;
            /*if (hat.has_back)
            {
                var sprite2 = TownOfUs.CreatePolusHat($"TownOfUs.Resources.Hats.hat_{hat.name}_back.png");
                newHat.InFront = false;
                newHat.BackImage = sprite2;
            }*/
            newHat.NoBounce = !hat.bounce;
            newHat.ChipOffset = hat.offset;

            return newHat;
        }

        protected internal struct HatData
        {
            public bool bounce;
            public string name;
            public bool highUp;
            public Vector2 offset;
            public string author;
            public bool new_hat;

        }

        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
        public static class HatManagerPatch
        {
            private static bool Prefix(HatManager __instance)
            {
                try
                {
                    if (!modded)
                    {
                        System.Console.WriteLine("Adding hats");
                        modded = true;
                        List<HatData> hatDatas = new List<HatData>();
                        
                        hatDatas.AddRange(GenerateHat());

                        var hatId = 0;
                        foreach (var hatData in hatDatas)
                        {
                            try
                            {
                                var hat = CreateHat(hatData, ++hatId);
                                __instance.AllHats.Add(hat);
                                if (hatData.highUp) TallIds.Add((uint) (__instance.AllHats.Count - 1));
                                IdToData.Add((uint) __instance.AllHats.Count - 1, hatData);
                            }
                            catch (Exception)
                            {
                                PluginSingleton<TownOfUs>.Instance.Log.LogError($"Couldn't generate {hatData.name}");
                            }
                        }
                    }

                    return true;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("During Prefix, an exception occured");
                    System.Console.WriteLine("------------------------------------------------");
                    System.Console.WriteLine(e);
                    System.Console.WriteLine("------------------------------------------------");
                    throw;
                }
            }

            private static List<HatData> GenerateHat()
            {
                const string prefix = "TownOfUs.Resources.Hats.";
                var hatDatas = new List<HatData>();
                foreach (var resourceName in typeof(TownOfUs).Assembly.GetManifestResourceNames())
                {
                    if (!resourceName.StartsWith(prefix))
                        continue;
                    
                    hatDatas.Add(new HatData
                    {
                        author = "",
                        bounce = false,
                        name = resourceName[prefix.Length..^4],
                        offset = new Vector2(-0.1f, 0.35f),
                        highUp = false,
                        new_hat = true
                    });
                }

                return hatDatas;
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetHat))]
        public static class PlayerControl_SetHat
        {
            public static void Postfix(PlayerControl __instance, uint __0, int __1)
            {
                __instance.nameText.transform.localPosition = new Vector3(
                    0f,
                    __0 == 0U ? 1.5f : TallIds.Contains(__0) ? 2.2f : 2.0f,
                    -0.5f
                );
            }
        }
    }
}
