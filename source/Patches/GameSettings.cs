
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs {
    
    [HarmonyPatch]
    public static class GameSettings {
        public static string GameSettingsText;
        static float defaultBounds = 0f;

        [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Method_24))]
        [HarmonyBefore("com.comando.essentials")]
        public static class GameSettingsPatch1
        {
          
            
            public static void Postfix(ref string __result)
            {
                

                GameSettingsText = __result;

            }
        }
        
        [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Method_24))]
        [HarmonyAfter("com.comando.essentials")]
        public static class GameSettingsPatch2
        {
            
            public static void Postfix(ref string __result)
            {

                __result = GameSettingsText;
            }
        }



        public static string StringBuild()
        {
            var builder = new StringBuilder("Roles:\n");
            foreach (var option in TownOfUs.AllOptions)
            {
                builder.AppendLine(option.StringFormat == TownOfUs.PercentFormat
                    ? $"     {option.Name}: {option}"
                    : $"{option.Name}: {option}");
            }

            return builder.ToString();
        }

        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.FixedUpdate))]
        public static class LobbyFix
        {
            private static bool _isCustom;
            private static float _lastUpdated;

            public static bool Prefix(LobbyBehaviour __instance)
            {
                __instance.Field_6 += Time.deltaTime;
                if (__instance.Field_6 < 0.25f) return false;
                __instance.Field_6 = 0f;
                if (PlayerControl.GameOptions == null) return false;
                if (Time.time - _lastUpdated > 5.0)
                {
                    _lastUpdated = Time.time;
                    _isCustom = !_isCustom;
                }
                
                if (_isCustom)
                {
                    var numPlayers = GameData.Instance ? GameData.Instance.PlayerCount : 10;
                    DestroyableSingleton<HudManager>.Instance.GameSettings.Text = PlayerControl.GameOptions.Method_24(numPlayers);
                }
                else
                {
                    DestroyableSingleton<HudManager>.Instance.GameSettings.Text = StringBuild();
                }
                DestroyableSingleton<HudManager>.Instance.GameSettings.gameObject.SetActive(true);

                
                return false;
            }
        }


        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        [HarmonyAfter("com.comando.essentials")]
        public static class FixScale
        {
            public static void Prefix(HudManager __instance)
            {
                __instance.GameSettings.scale = 0.6f;
            }
        }
        
        
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        public static class Start {
            public static void Postfix(ref GameOptionsMenu __instance) {
                defaultBounds = __instance.GetComponentInParent<Scroller>().YBounds.max;
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static class Update {
            public static void Postfix(ref GameOptionsMenu __instance) {
                __instance.GetComponentInParent<Scroller>().YBounds.max = 20f;
            }
        }
    }
}