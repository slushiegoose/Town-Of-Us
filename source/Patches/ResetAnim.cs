using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class ResetAnim
    {
        private static bool resetAnim;
        private static bool isHeldDown;

        public static void Postfix(PlayerControl __instance)
        {
            if (__instance != PlayerControl.LocalPlayer) return;
            // if (Input.GetKeyInt(KeyCode.B))
            // {
            //     PlayerControl.LocalPlayer.MyPhysics.ResetMoveState(true);
            //     PlayerControl.LocalPlayer.Collider.enabled = true;
            //     PlayerControl.LocalPlayer.moveable = true;
            //     PlayerControl.LocalPlayer.NetTransform.enabled = true;
            //
            //     
            //     var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            //         (byte) CustomRPC.FixAnimation, SendOption.Reliable, -1);
            //     writer.Write(PlayerControl.LocalPlayer.PlayerId);
            //     AmongUsClient.Instance.FinishRpcImmediately(writer);
            //
            // }

            // if (!isHeldDown)
            // {
            //     resetAnim = PlayerControl.LocalPlayer.Collider.enabled;
            // }
            if (Input.GetKeyInt(KeyCode.N))
            {
                //PlayerControl.LocalPlayer.Collider.enabled = false;
                //isHeldDown = true;
            }
        }
    }
}