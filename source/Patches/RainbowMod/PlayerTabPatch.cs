/*using System;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfUs.RainbowMod
{
    public class PlayerTabPatch
    {
        

        [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
        public static class OnEnablePatch
        {
            public static void Postfix(PlayerTab __instance)
            {
                for (int i = 0; i < __instance.ColorChips.Count; i++)
                {
                    var chip = __instance.ColorChips.ToArray()[i];
                    chip.transform.localScale *= 0.8f;
                    chip.Button.OnClick.AddListener(Button(i));
                }
            }

            private static System.Action Button(int i)
            {
                void Inner()
                {
                    SaveManager.Property_25 = (byte)(i < 12 ? i : 0);
                }

                return Inner;
            }
        }

    }
    
    
}*/

