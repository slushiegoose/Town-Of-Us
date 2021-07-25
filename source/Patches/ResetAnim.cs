using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public class ResetAnim
    {
        private static bool resetAnim;
        private static bool isHeldDown;

        public static void Postfix()
        {
            /*
            if (Input.GetKeyDown(KeyCode.F5) && PlayerControl.LocalPlayer && PlayerControl.LocalPlayer.Collider)
            {
                PlayerControl.LocalPlayer.Collider.enabled = !PlayerControl.LocalPlayer.Collider.enabled;
            }
            */
        }
    }
}