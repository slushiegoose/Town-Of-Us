using HarmonyLib;

namespace TownOfUs.Modifiers
{
    public class Flash
    {
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        public static class PlayerPhysics_FixedUpdate
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                if (__instance.myPlayer.Is(ModifierEnum.Flash))
                {
                    if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                    {
                        __instance.body.velocity *= 2;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        public static class CustomNetworkTransform_FixedUpdate
        {
            public static void Postfix(CustomNetworkTransform __instance)
            {
                if (!__instance.AmOwner)
                {
                    if (__instance.interpolateMovement != 0f)
                    {
                        if (__instance.gameObject.GetComponent<PlayerControl>().Is(ModifierEnum.Flash))
                        {
                            __instance.body.velocity *= 2;
                        }
                    }
                }
            }
        }
    }

}