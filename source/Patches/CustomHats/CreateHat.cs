using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Rewired;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.CustomHats
{
    public class HatCreation
    {
        private static bool modded = false;


        protected internal struct HatData
        {
            public bool bounce;
            public string name;
            public bool highUp;
            public Vector2 offset;
            public string author;
        }

        private static List<HatData> _hatDatas = new List<HatData>()
        {
            new HatData {name = "aphex", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            new HatData {name = "junkyard", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            new HatData {name = "cheesy", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            new HatData {name = "shubble", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            new HatData {name = "aplatypuss", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            new HatData {name = "ze", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            new HatData {name = "chilled", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.2f), author="Nassegris"},
            
            
            new HatData {name = "harrie", bounce = true, highUp = true, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "razz", bounce = true, highUp = true, offset = new Vector2(-0.1f, 0.3f), author="TheLastShaymin"},
            new HatData {name = "kay", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "zylus", bounce = true, highUp = true, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "annie", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "annamaja", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "bloody", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "ellum", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "stumpy", bounce = false, highUp = true, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "breeh", bounce = false, highUp = true, offset = new Vector2(-0.1f, 0.2f), author="TheLastShaymin"},
            new HatData {name = "vikram_1", bounce = true, highUp = true, offset = new Vector2(-0.1f, 0.3f), author="TheLastShaymin"},
            new HatData {name = "vikram_2", bounce = true, highUp = true, offset = new Vector2(-0.1f, 0.3f), author="TheLastShaymin"},
            new HatData {name = "dizzilulu", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.3f), author="TheLastShaymin"},
            new HatData {name = "freya", bounce = true, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "lexie", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            new HatData {name = "slushie", bounce = false, highUp = true, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "falcone", bounce = true, highUp = false, offset = new Vector2(-0.1f, 0.4f), author="TheLastShaymin"},
            
            
            new HatData {name = "bisexual", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "asexual", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "gay", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "pansexual", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "nonbinary", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            
            new HatData {name = "trans_1", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "trans_4", bounce = false, highUp = true, offset = new Vector2(-0.1f, 0.5f), author="TheLastShaymin"},
            new HatData {name = "trans_3", bounce = false, highUp = false, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            new HatData {name = "trans_2", bounce = false, highUp = true, offset = new Vector2(-0.1f, 0.1f), author="TheLastShaymin"},
            
            
        };

        public static List<uint> TallIds = new List<uint>();

        protected internal static Dictionary<uint, HatData> IdToData = new Dictionary<uint, HatData>();

        private static HatBehaviour CreateHat(HatData hat, int id)
        {
            System.Console.WriteLine($"Creating Hat {hat.name}");
            var sprite = TownOfUs.CreateSprite($"TownOfUs.Resources.Hats.hat_{hat.name}.png", true);
            var newHat = ScriptableObject.CreateInstance<HatBehaviour>();
            newHat.MainImage = sprite;
            newHat.ProductId = hat.name;
            newHat.Order = 99 + id;
            newHat.InFront = true;
            newHat.NoBounce = !hat.bounce;
            newHat.ChipOffset = hat.offset;

            return newHat;
        }

        private static IEnumerable<HatBehaviour> CreateAllHats()
        {

            var i = 0;
            foreach (var hat in _hatDatas)
            {
                yield return CreateHat(hat, ++i);
            }
        }

        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
        public static class HatManagerPatch
        {
            static bool Prefix(HatManager __instance)
            {
                try
                {
                    if (!modded)
                    {
                        System.Console.WriteLine("Adding hats");
                        modded = true;
                        var id = 0;
                        foreach (var hatData in _hatDatas)
                        {
                            var hat = CreateHat(hatData, id++);
                            __instance.AllHats.Add(hat);
                            if (hatData.highUp)
                            {
                                TallIds.Add((uint)(__instance.AllHats.Count - 1));
                            }
                            IdToData.Add((uint)__instance.AllHats.Count - 1, hatData);
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
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetHat))]
        public static class PlayerControl_SetHat
        {
            public static void Postfix(PlayerControl __instance, uint __0, int __1)
            {
                __instance.nameText.transform.localPosition = new Vector3(
                    0f,
                    __0 == 0U ? 0.7f : TallIds.Contains(__0) ? 1.2f : 1.05f,
                    -0.5f
                );
            }
        }
        
        
    }
}