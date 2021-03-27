using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;
using Object = UnityEngine.Object;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class AddButton
    {
        private static Sprite ActiveSprite => TownOfUs.SwapperSwitch;
        public static Sprite DisabledSprite => TownOfUs.SwapperSwitchDisabled;
        private static int _mostRecentId;
        
        
        public static void GenButton(Swapper role, int index, bool isDead)
        {

            if (isDead)
            {
                role.Buttons.Add(null);
                role.ListOfActives.Add(false);
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
            passive.OnClick.AddListener(SetActive(role, index));
            role.Buttons.Add(newButton);
            role.ListOfActives.Add(false);

        }


        private static Action SetActive(Swapper role, int index)
        {
            void Listener()
            {
                if (role.ListOfActives.Count(x => x) == 2 &&
                    role.Buttons[index].GetComponent<SpriteRenderer>().sprite == DisabledSprite) return;
                
                role.Buttons[index].GetComponent<SpriteRenderer>().sprite =
                    role.ListOfActives[index] ? DisabledSprite : ActiveSprite;
                
                role.ListOfActives[index] = !role.ListOfActives[index];

                _mostRecentId = index;
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage(string.Join(" ", role.ListOfActives));
            }

            return Listener;
        }
        
        public static void Postfix(MeetingHud __instance)
        {
            
            foreach(var role in Role.GetRoles(RoleEnum.Swapper))
            {
                var swapper = (Swapper) role;
                swapper.ListOfActives.Clear();
                swapper.Buttons.Clear();
            }


            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) return;
            var swapperrole = Role.GetRole<Swapper>(PlayerControl.LocalPlayer);
            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                GenButton(swapperrole, i, __instance.playerStates[i].isDead);
            }
        }
    }
}