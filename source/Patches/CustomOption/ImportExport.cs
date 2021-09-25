// This folder is a Stripped down version of Reactor-Essentials
// Please use https://github.com/DorCoMaNdO/Reactor-Essentials because it is more updated and less buggy

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Reactor;
using Reactor.Extensions;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;
using TownOfUs.Extensions;
using Object = UnityEngine.Object;

namespace TownOfUs.CustomOption
{
    public class ImportExport : CustomButtonOption
    {
        public bool IsImport;
        public static string DataPath = Path.Combine(
            Application.persistentDataPath, $"{nameof(TownOfUs)} Settings"
        );
        public List<CustomButtonOption> SlotButtons = new List<CustomButtonOption>();
        public List<OptionBehaviour> OldButtons;

        private List<OptionBehaviour> CreateOptions()
        {
            var options = new List<OptionBehaviour>();

            var togglePrefab = Object.FindObjectOfType<ToggleOption>();

            foreach (var button in SlotButtons)
            {
                if (button.Setting != null)
                {
                    button.Setting.gameObject.SetActive(true);
                    options.Add(button.Setting);
                }
                else
                {
                    var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent).DontDestroy();
                    toggle.transform.GetChild(2).gameObject.SetActive(false);
                    toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);

                    button.Setting = toggle;
                    button.OptionCreated();
                    options.Add(toggle);
                }
            }

            return options;
        }

        protected internal void Cancel(Func<IEnumerator> flashCoro) => Coroutines.Start(CancelCoro(flashCoro));

        protected internal IEnumerator CancelCoro(Func<IEnumerator> flashCoro)
        {
            var __instance = Object.FindObjectOfType<GameOptionsMenu>();
            foreach (var option in SlotButtons.Skip(1))
            {
                option.Setting.gameObject.Destroy();
            }

            var loadingButton = SlotButtons[0].Setting;
            loadingButton.Cast<ToggleOption>().TitleText.text = "Loading...";

            __instance.Children = new[] { loadingButton };
            yield return new WaitForSeconds(0.5f);

            loadingButton.gameObject.Destroy();

            __instance.Children = OldButtons.ToArray();

            Patches.DontRefresh = false;

            Patches.GameOptionsMenuPatch.RefreshOptions();

            yield return flashCoro();
        }

        protected internal void ToDo()
        {
            if (SlotButtons.Count > 0)
            {
                foreach (var slot in SlotButtons)
                {
                    var setting = slot.Setting;
                    if (setting == null) continue;
                    setting.Destroy();
                    setting.gameObject?.Destroy();
                }
                SlotButtons.Clear();
            }
            var settingFiles = new string[] { };
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            else
            {
                settingFiles = Directory.GetFiles(DataPath, "GameSettings-Slot*", SearchOption.TopDirectoryOnly);
            }
            Action ButtonHandler(int slotId)
            {
                void Handler()
                {
                    if (IsImport) ImportSlot(slotId);
                    else ExportSlot(slotId);
                }
                return Handler;
            }
            var id = 0;
            foreach (var slotName in settingFiles)
            {
                var slotId = int.Parse(slotName.Last().ToString());
                SlotButtons.Add(new CustomButtonOption(++id, $"Slot {slotId}", ButtonHandler(slotId)));
            }
            if (SlotButtons.Count < 3)
            {
                while (SlotButtons.Count < 3)
                {
                    var slotId = SlotButtons.Count + 1;
                    SlotButtons.Add(new CustomButtonOption(++id, $"Slot {slotId}", ButtonHandler(slotId)));
                    ExportSlot(slotId);
                }
            }

            SlotButtons.Add(new CustomButtonOption(++id, "Add Slot", delegate
            {
                ButtonHandler(++id)();
            }));

            SlotButtons.Add(new CustomButtonOption(++id, "Cancel", delegate
            {
                Cancel(FlashWhite);
            }));

            var options = CreateOptions();

            var __instance = Object.FindObjectOfType<GameOptionsMenu>();

            var y = __instance.GetComponentsInChildren<OptionBehaviour>()
                .Max(option => option.transform.localPosition.y);
            var x = __instance.Children[1].transform.localPosition.x;
            var z = __instance.Children[1].transform.localPosition.z;
            var i = 0;

            OldButtons = __instance.Children.ToList();
            foreach (var option in __instance.Children)
            {
                option.gameObject.SetActive(false);
            }

            foreach (var option in options)
            {
                option.transform.localPosition = new Vector3(x, y - i++ * 0.5f, z);
            }

            __instance.Children = new Il2CppReferenceArray<OptionBehaviour>(options.ToArray());
        }

        private void ExportSlot(int slotId)
        {
            var builder = new StringBuilder();
            foreach (var option in AllOptions)
            {
                if (option.Type == CustomOptionType.Button || option.Type == CustomOptionType.Header) continue;
                builder.AppendLine(option.Name);
                builder.AppendLine(option.Value.ToString());
            }

            var exportTo = Path.Combine(DataPath, $"GameSettings-Slot{slotId}");

            try
            {
                File.WriteAllText(exportTo, builder.ToString());
                Cancel(FlashGreen);
            }
            catch
            {
                Cancel(FlashRed);
            }
        }

        private void ImportSlot(int slotId)
        {
            string text;
            var path = Path.Combine(DataPath, $"GameSettings-Slot{slotId}");

            try
            {
                text = File.ReadAllText(path);
            }
            catch
            {
                Cancel(FlashRed);
                return;
            }


            var splitText = text.Split("\n").ToList();

            while (splitText.Count > 0)
            {
                var name = splitText[0].Trim();
                splitText.RemoveAt(0);
                var option = AllOptions.FirstOrDefault(o => o.Name.Equals(name, StringComparison.Ordinal));
                if (option == null)
                {
                    try
                    {
                        splitText.RemoveAt(0);
                    }
                    catch
                    {
                    }

                    continue;
                }

                var value = splitText[0];
                splitText.RemoveAt(0);
                switch (option.Type)
                {
                    case CustomOptionType.Number:
                        option.Set(float.Parse(value), false);
                        break;
                    case CustomOptionType.Toggle:
                        option.Set(bool.Parse(value), false);
                        break;
                    case CustomOptionType.String:
                        option.Set(int.Parse(value), false);
                        break;
                }
            }

            Rpc.SendRpc();

            Cancel(FlashGreen);
        }


        private IEnumerator FlashGreen()
        {
            Setting.Cast<ToggleOption>().TitleText.color = Color.green;
            yield return new WaitForSeconds(0.5f);
            Setting.Cast<ToggleOption>().TitleText.color = Color.white;

        }

        private IEnumerator FlashRed()
        {
            Setting.Cast<ToggleOption>().TitleText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            Setting.Cast<ToggleOption>().TitleText.color = Color.white;

        }

        private IEnumerator FlashWhite()
        {
            yield return null;
        }

        protected internal ImportExport(int id, string name) : base(id, name)
        {
            Do = ToDo;
            IsImport = name == "Load Custom Settings";
        }
    }
}
