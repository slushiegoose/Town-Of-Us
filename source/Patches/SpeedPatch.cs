using HarmonyLib;
using TownOfUs.Extensions;

namespace TownOfUs.Patches
{
    [HarmonyPatch]
    public static class SpeedPatch
    {
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixPhysics(PlayerPhysics __instance)
        {
            if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                __instance.body.velocity *= __instance.myPlayer.GetAppearance().SpeedFactor;
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixNetwork(CustomNetworkTransform __instance)
        {
            if (__instance.AmOwner && __instance.interpolateMovement != 0.0f)
            {
                var player = __instance.gameObject.GetComponent<PlayerControl>();
                __instance.body.velocity *= player.GetAppearance().SpeedFactor;
            }
        }
    }
}