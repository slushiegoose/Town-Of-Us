using BepInEx.Logging;
using Reactor;
using Reactor.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace TownOfUs.Patches.CustomHats
{
    internal static class HatLoader
    {
        private const string HAT_RESOURCE_NAMESPACE = "TownOfUs.Resources.Hats";
        private const string HAT_METADATA_JSON = "metadata.json";
        private const int HAT_ORDER_BASELINE = 99;

        private static ManualLogSource Log => PluginSingleton<TownOfUs>.Instance.Log;
        private static Assembly Assembly => typeof(TownOfUs).Assembly;

        internal static void LoadHats()
        {
            Log.LogMessage($"Generating Hats from namespace {HAT_RESOURCE_NAMESPACE}");
            try
            {
                var hatJson = LoadJson();

                var hatBehaviours = DiscoverHatBehaviours(hatJson);

                DestroyableSingleton<HatManager>.Instance.AllHats.ForEach(
                    (Action<HatBehaviour>)(x => x.StoreName = "Vanilla")
                );

                for (var i = 0; i < hatBehaviours.Count; i++)
                {
                    hatBehaviours[i].Order = HAT_ORDER_BASELINE + i;
                    HatManager.Instance.AllHats.Add(hatBehaviours[i]);
                }

            }
            catch (Exception e)
            {
                Log.LogError($"Error while loading hats: {e.Message}\nStack: {e.StackTrace}");
            }
        }

        private static HatMetadataJson LoadJson()
        {
            var stream = Assembly.GetManifestResourceStream($"{HAT_RESOURCE_NAMESPACE}.{HAT_METADATA_JSON}");
            var streamContent = Encoding.UTF8.GetString(stream.ReadFully());

            return JsonSerializer.Deserialize<HatMetadataJson>(streamContent);
        }

        private static List<HatBehaviour> DiscoverHatBehaviours(HatMetadataJson metadata)
        {
            var hatBehaviours = new List<HatBehaviour>();

            foreach (var hatCredit in metadata.Credits)
            {
                try
                {
                    var stream = Assembly.GetManifestResourceStream($"{HAT_RESOURCE_NAMESPACE}.{hatCredit.Id}.png");
                    if (stream != null)
                    {
                        var hatBehaviour = GenerateHatBehaviour(stream.ReadFully());
                        hatBehaviour.StoreName = hatCredit.Artist;
                        hatBehaviour.ProductId = hatCredit.Name;
                        hatBehaviours.Add(hatBehaviour);
                    }
                }
                catch (Exception e)
                {
                    // Log.LogError(
                    //     $"Error loading hat {hatCredit.Id} in metadata file ({HAT_METADATA_JSON})");
                    // Log.LogError($"{e.Message}\nStack:{e.StackTrace}");
                }
            }

            return hatBehaviours;
        }

        private static HatBehaviour GenerateHatBehaviour(byte[] mainImg)
        {

            //TODO: Move to Graphics Utils class
            var tex2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            TownOfUs.LoadImage(tex2D, mainImg, false);
            var sprite = Sprite.Create(tex2D, new Rect(0.0f, 0.0f, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100);


            var hat = ScriptableObject.CreateInstance<HatBehaviour>();
            hat.MainImage = sprite;
            hat.ChipOffset = new Vector2(-0.1f, 0.35f);

            hat.InFront = true;
            hat.NoBounce = true;

            return hat;
        }
    }
}