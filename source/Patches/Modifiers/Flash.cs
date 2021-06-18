using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.Modifiers
{
    public class Flash
    {
        public static bool shouldSpeedUp(PlayerControl player)
        {
            if (!player.Is(ModifierEnum.Flash))
            {
                if (player.Is(RoleEnum.Morphling))
                {
                    var morphling = Role.GetRole<Morphling>(player);
                    return morphling.MorphedPlayer != null && morphling.MorphedPlayer.Is(ModifierEnum.Flash);
                }

                if (player.Is(RoleEnum.Glitch))
                {
                    var glitch = Role.GetRole<Glitch>(player);
                    return glitch.MimicTarget != null && glitch.MimicTarget.Is(ModifierEnum.Flash);
                }

                return false;
            }

            if (player.Is(RoleEnum.Morphling))
            {
                var morphling = Role.GetRole<Morphling>(player);
                return morphling.MorphedPlayer == null;
            }

            if (player.Is(RoleEnum.Glitch))
            {
                var glitch = Role.GetRole<Glitch>(player);
                return glitch.MimicTarget == null;
            }

            return true;
        }

        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        public static class PlayerPhysics_FixedUpdate
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                if (shouldSpeedUp(__instance.myPlayer))
                    if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                        __instance.body.velocity *= 1.5f;
            }
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        public static class CustomNetworkTransform_FixedUpdate
        {
            public static void Postfix(CustomNetworkTransform __instance)
            {
                if (!__instance.AmOwner)
                    if (__instance.interpolateMovement != 0f)
                        if (shouldSpeedUp(__instance.gameObject.GetComponent<PlayerControl>()))
                            __instance.body.velocity *= 1.5f;
            }
        }
    }
}