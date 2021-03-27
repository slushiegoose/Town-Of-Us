using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.CustomHats
{
    public class HatCreation
    {
        private static bool modded = false;

        private static List<string> _hats = new List<string>() {"hat_falcone"};

        private static HatBehaviour CreateHat(string name)
        {
            System.Console.WriteLine($"Creating Hat {name}");
            var sprite = TownOfUs.CreateSprite($"TownOfUs.Resources.Hats.{name}.png", true);
            var newHat = ScriptableObject.CreateInstance<HatBehaviour>();
            newHat.MainImage = sprite;
            newHat.ProductId = name;
            newHat.Order = -1;
            newHat.InFront = true;
            newHat.NoBounce = false;

            return newHat;
        }

        private static IEnumerable<HatBehaviour> CreateAllHats()
        {
            return _hats.Select(CreateHat);
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
                        var hatsFromFilesystem = CreateAllHats();
                        foreach (var hat in hatsFromFilesystem)
                        {
                            __instance.AllHats.Add(hat);
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
        
    }
}