using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.ResetMoveState))]
    public class ResetMoveState
    {
        public static void Postfix(PlayerPhysics __instance)
        {
            if (!__instance.myPlayer.Is(RoleEnum.Phantom)) return;

            var role = Role.GetRole<Phantom>(__instance.myPlayer);
            __instance.myPlayer.Collider.enabled = !role.Caught;
        }
    }
}