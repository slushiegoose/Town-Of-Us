using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.ChildMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    public class VoteOut
    {
        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo exiled, ExileController __instance)
        {
            if (exiled == null) return;
            var player = exiled.Object;

            var role = Role.GetRole(player);
            if (role == null) return;
            if (role.RoleType == RoleEnum.Child)
            {
                ((Child) role).Wins();
            }
        }
    }
}