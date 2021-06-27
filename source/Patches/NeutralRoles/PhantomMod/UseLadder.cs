using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(Ladder), nameof(Ladder.CanUse))]
    public class UseLadder
    {
        public static bool Prefix(Ladder __instance, [HarmonyArgument(0)] GameData.PlayerInfo pc,
            [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse, ref float __result)
        {
            var num = float.MaxValue;
            var @object = pc.Object;
            couldUse = (!pc.IsDead || @object.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(@object).Caught)
                       && @object.CanMove;
            canUse = couldUse;
            if (canUse)
            {
                var truePosition = @object.GetTruePosition();
                var position = __instance.transform.position;
                num = Vector2.Distance(truePosition, position);
                canUse &= num <= __instance.UsableDistance &&
                          !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, false);
            }

            __result = num;
            return false;
        }
    }

    [HarmonyPatch(typeof(Ladder), nameof(Ladder.Use))]
    public class UseLadder2
    {
        public static bool Prefix(Ladder __instance)
        {
            var data = PlayerControl.LocalPlayer.Data;
            __instance.CanUse(data, out var flag, out var flag2);
            if (flag) PlayerControl.LocalPlayer.MyPhysics.RpcClimbLadder(__instance);

            return false;
        }
    }
}