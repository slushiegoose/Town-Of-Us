using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reactor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class AddButton
    {
        private static Sprite ActiveSprite => TownOfUs.SwapperSwitch;
        public static Sprite DisabledSprite => TownOfUs.SwapperSwitchDisabled;
        public static readonly List<bool> ListOfActives = new List<bool>();
        public static readonly List<GameObject> Buttons = new List<GameObject>();
        private static int _mostRecentId;
        
        
        public static void GenButton(int index, bool isDead)
        {

            if (isDead)
            {
                Buttons.Add(null);
                ListOfActives.Add(false);
                return;
            }
            
            var confirmButton = MeetingHud.Instance.playerStates[index].Buttons.transform.GetChild(0).gameObject;
            
            
            var newButton = Object.Instantiate(confirmButton, MeetingHud.Instance.playerStates[index].transform);
            var renderer = newButton.GetComponent<SpriteRenderer>();
            var passive = newButton.GetComponent<PassiveButton>();
            
            renderer.sprite = DisabledSprite;
            newButton.transform.position = confirmButton.transform.position - new Vector3(0.5f, 0f, 0f);
            newButton.transform.localScale *= 0.8f;
            newButton.layer = 5;
            newButton.transform.parent = confirmButton.transform.parent.parent;

            passive.OnClick = new Button.ButtonClickedEvent();
            passive.OnClick.AddListener(SetActive(index));
            Buttons.Add(newButton);
            ListOfActives.Add(false);

        }


        private static Action SetActive(int index)
        {
            void Listener()
            {
                if (ListOfActives.Count(x => x) == 2 &&
                    Buttons[index].GetComponent<SpriteRenderer>().sprite == DisabledSprite) return;
                
                Buttons[index].GetComponent<SpriteRenderer>().sprite =
                    ListOfActives[index] ? DisabledSprite : ActiveSprite;
                
                ListOfActives[index] = !ListOfActives[index];

                _mostRecentId = index;
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage(string.Join(" ", ListOfActives));
            }

            return Listener;
        }

        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void Postfix(MeetingHud __instance)
        {
            ListOfActives.Clear();
            Buttons.Clear();

            if (!PlayerControl.LocalPlayer.isSwapper()) return;
            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                GenButton(i, __instance.playerStates[i].isDead);
            }
        }
    }
}