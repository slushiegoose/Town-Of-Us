using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reactor.Extensions;
using Reactor;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.CustomOption
{
    public static class Patches
    {
        public static ImportExport ExportButton;
        public static ImportExport ImportButton;
        public static List<OptionBehaviour> DefaultOptions;
        public static float LobbyTextRowHeight { get; set; } = 0.081F;

        public static Scroller OptionsScroller;

        public static bool DontRefresh = false;

        private static List<OptionBehaviour> CreateOptions()
        {
            var options = new List<OptionBehaviour>();

            var togglePrefab = Object.FindObjectOfType<ToggleOption>();
            var numberPrefab = Object.FindObjectOfType<NumberOption>();
            var stringPrefab = Object.FindObjectOfType<StringOption>();


            if (ExportButton.Setting != null)
            {
                ExportButton.Setting.gameObject.SetActive(true);
                options.Add(ExportButton.Setting);
            }
            else
            {
                var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent).DontDestroy();
                toggle.transform.GetChild(2).gameObject.SetActive(false);
                toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);

                ExportButton.Setting = toggle;
                ExportButton.OptionCreated();
                options.Add(toggle);
            }

            if (ImportButton.Setting != null)
            {
                ImportButton.Setting.gameObject.SetActive(true);
                options.Add(ImportButton.Setting);
            }
            else
            {
                var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent).DontDestroy();
                toggle.transform.GetChild(2).gameObject.SetActive(false);
                toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);

                ImportButton.Setting = toggle;
                ImportButton.OptionCreated();
                options.Add(toggle);
            }

            foreach (var defaultOption in DefaultOptions)
            {
                defaultOption.gameObject.SetActive(true);
                options.Add(defaultOption);
            }

            foreach (var option in CustomOption.AllOptions)
            {
                var shouldShow = option.ShouldShow();
                if (option.Setting != null)
                {
                    option.Setting.gameObject.SetActive(shouldShow);
                    if (shouldShow)
                        options.Add(option.Setting);
                    continue;
                }

                switch (option.Type)
                {
                    case CustomOptionType.Header:
                        var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent).DontDestroy();
                        toggle.transform.GetChild(1).gameObject.SetActive(false);
                        toggle.transform.GetChild(2).gameObject.SetActive(false);
                        option.Setting = toggle;
                        break;
                    case CustomOptionType.Toggle:
                        var toggle2 = Object.Instantiate(togglePrefab, togglePrefab.transform.parent).DontDestroy();
                        option.Setting = toggle2;
                        break;
                    case CustomOptionType.Number:
                        var number = Object.Instantiate(numberPrefab, numberPrefab.transform.parent).DontDestroy();
                        option.Setting = number;
                        break;
                    case CustomOptionType.String:
                        var str = Object.Instantiate(stringPrefab, stringPrefab.transform.parent).DontDestroy();
                        option.Setting = str;
                        break;
                }
                option.Setting.gameObject.SetActive(shouldShow);
                if (shouldShow)
                    options.Add(option.Setting);

                option.OptionCreated();
            }

            return options;
        }


        private static bool OnEnable(OptionBehaviour opt)
        {
            if (opt == ExportButton.Setting)
            {
                ExportButton.OptionCreated();
                return false;
            }

            if (opt == ImportButton.Setting)
            {
                ImportButton.OptionCreated();
                return false;
            }


            var customOption =
                CustomOption.AllOptions.FirstOrDefault(option =>
                    option.Setting == opt); // Works but may need to change to gameObject.name check

            if (customOption == null)
            {
                customOption = ExportButton.SlotButtons.FirstOrDefault(option => option.Setting == opt);
                if (customOption == null)
                {
                    customOption = ImportButton.SlotButtons.FirstOrDefault(option => option.Setting == opt);
                    if (customOption == null) return true;
                }
            }

            customOption.OptionCreated();

            return false;
        }

        [HarmonyPatch(typeof(GameOptionsMenu))]
        public static class GameOptionsMenuPatch
        {
            private static float OriginalMaxY;
            public static void RefreshOptions()
            {
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage("refreshing");
                var instance = Object.FindObjectOfType<GameOptionsMenu>();
                if (instance != null)
                    RefreshOptions(instance);
            }

            public static void RefreshOptions(GameOptionsMenu __instance)
            {
                var customOptions = CreateOptions();
                var child = DefaultOptions[1].transform.localPosition;
                var x = child.x;
                var z = child.z;
                var i = 0;

                foreach (var option in customOptions)
                    option.transform.localPosition = new Vector3(x, OriginalMaxY - i++ * 0.5f, z);

                __instance.Children = new Il2CppReferenceArray<OptionBehaviour>(customOptions.ToArray());
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(GameOptionsMenu.Start))]
            public static void Start(GameOptionsMenu __instance)
            {
                DefaultOptions = __instance.Children.ToList();
                OriginalMaxY = DefaultOptions.Max(option => option.transform.localPosition.y);
                RefreshOptions(__instance);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(GameOptionsMenu.Update))]
            public static void Update(GameOptionsMenu __instance)
            {
                var y = __instance.GetComponentsInChildren<OptionBehaviour>()
                    .Max(option => option.transform.localPosition.y);

                var child = __instance.Children[
                    __instance.Children.Length == 1 ? 0 : 1
                ].transform.localPosition;

                var x = child.x;
                var z = child.z;

                var i = 0;
                foreach (var option in __instance.Children)
                    option.transform.localPosition = new Vector3(x, y - i++ * 0.5f, z);
            }
        }

        [HarmonyPatch(typeof(ToggleOption), nameof(ToggleOption.OnEnable))]
        private static class ToggleOption_OnEnable
        {
            private static bool Prefix(ToggleOption __instance)
            {
                return OnEnable(__instance);
            }
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.OnEnable))]
        private static class NumberOption_OnEnable
        {
            private static bool Prefix(NumberOption __instance)
            {
                return OnEnable(__instance);
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
        private static class StringOption_OnEnable
        {
            private static bool Prefix(StringOption __instance)
            {
                return OnEnable(__instance);
            }
        }


        [HarmonyPatch(typeof(ToggleOption), nameof(ToggleOption.Toggle))]
        private class ToggleButtonPatch
        {
            public static bool Prefix(ToggleOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomToggleOption toggle)
                {
                    toggle.Toggle();
                    return false;
                }

                if (__instance == ExportButton.Setting)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    DontRefresh = true;
                    ExportButton.Do();
                    return false;
                }
                else if (__instance == ImportButton.Setting)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    DontRefresh = true;
                    ImportButton.Do();
                    return false;
                }


                if (option is CustomHeaderOption) return false;

                var option2 =
                    ExportButton.SlotButtons.FirstOrDefault(option => option.Setting == __instance) ??
                    ImportButton.SlotButtons.FirstOrDefault(option => option.Setting == __instance);
                if (option2 is CustomButtonOption button)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    button.Do();
                    return false;
                }

                return true;
            }

            public static void Postfix()
            {
                if (!DontRefresh) GameOptionsMenuPatch.RefreshOptions();
            }
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Increase))]
        private class NumberOptionPatchIncrease
        {
            public static bool Prefix(NumberOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomNumberOption number)
                {
                    number.Increase();
                    return false;
                }
                return true;
            }

            public static void Postfix() => GameOptionsMenuPatch.RefreshOptions();
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Decrease))]
        private class NumberOptionPatchDecrease
        {
            public static bool Prefix(NumberOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomNumberOption number)
                {
                    number.Decrease();
                    return false;
                }
                return true;
            }

            public static void Postfix() => GameOptionsMenuPatch.RefreshOptions();
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
        private class StringOptionPatchIncrease
        {
            public static bool Prefix(StringOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomStringOption str)
                {
                    str.Increase();
                    return false;
                }
                return true;
            }

            public static void Postfix() => GameOptionsMenuPatch.RefreshOptions();
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
        private class StringOptionPatchDecrease
        {
            public static bool Prefix(StringOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomStringOption str)
                {
                    str.Decrease();
                    return false;
                }
                return true;
            }

            public static void Postfix() => GameOptionsMenuPatch.RefreshOptions();
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
        private class PlayerControlPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.AllPlayerControls.Count < 2 || !AmongUsClient.Instance ||
                    !PlayerControl.LocalPlayer || !AmongUsClient.Instance.AmHost) return;

                Rpc.SendRpc();
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class HudManagerUpdate
        {
            private const float
                MinX = -5.233334F /*-5.3F*/,
                OriginalY = 2.9F,
                MinY = 3F; // Differs to cause excess options to appear cut off to encourage scrolling

            private static Vector3 LastPosition = new Vector3(MinX, MinY);

            public static void Prefix(HudManager __instance)
            {
                if (__instance.GameSettings?.transform == null) return;


                // Scroller disabled
                if (!CustomOption.LobbyTextScroller)
                {
                    // Remove scroller if disabled late
                    if (OptionsScroller != null)
                    {
                        __instance.GameSettings.transform.SetParent(OptionsScroller.transform.parent);
                        __instance.GameSettings.transform.localPosition = new Vector3(MinX, OriginalY);

                        Object.Destroy(OptionsScroller);
                    }

                    return;
                }

                CreateScroller(__instance);

                OptionsScroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

                if (!OptionsScroller.gameObject.active) return;

                var rows = __instance.GameSettings.text.Count(c => c == '\n');
                var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

                OptionsScroller.YBounds = new FloatRange(MinY, maxY);

                // Prevent scrolling when the player is interacting with a menu
                if (PlayerControl.LocalPlayer?.CanMove != true)
                {
                    __instance.GameSettings.transform.localPosition = LastPosition;

                    return;
                }

                if (__instance.GameSettings.transform.localPosition.x != MinX ||
                    __instance.GameSettings.transform.localPosition.y < MinY) return;

                LastPosition = __instance.GameSettings.transform.localPosition;
            }

            private static void CreateScroller(HudManager __instance)
            {
                if (OptionsScroller != null) return;

                var scroller = OptionsScroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
                scroller.transform.SetParent(__instance.GameSettings.transform.parent);
                scroller.gameObject.layer = 5;

                scroller.transform.localScale = Vector3.one;
                scroller.allowX = false;
                scroller.allowY = true;
                scroller.active = true;
                scroller.velocity = new Vector2(0, 0);
                scroller.ScrollerYRange = new FloatRange(0, 0);
                scroller.XBounds = new FloatRange(MinX, MinX);
                scroller.enabled = true;

                scroller.Inner = __instance.GameSettings.transform;
                __instance.GameSettings.transform.SetParent(scroller.transform);
            }
        }
    }
}
