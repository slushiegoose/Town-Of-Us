using System.Collections.Generic;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(PlayerVoteArea))]
    public class AllowExtraVotes
    {



        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        public static class Select
        {
            public static bool Prefix(PlayerVoteArea __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return true;
                var role = Roles.Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (PlayerControl.LocalPlayer.Data.IsDead) return false;
                if (__instance.isDead) return false;
                if (role.VoteBank <= 0 || !__instance.Parent.Select(__instance.TargetPlayerId)) return false;
                __instance.Buttons.SetActive(true);
                return false;
            }
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.VoteForMe))]
        public static class VoteForMe
        {
            public static bool Prefix(PlayerVoteArea __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return true;
                var role = Roles.Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding ||
                    __instance.Parent.state == MeetingHud.VoteStates.Results)
                {
                    return false;
                }

                if (role.VoteBank <= 0) return false;
                role.VoteBank--;
                __instance.Parent.Confirm(__instance.TargetPlayerId);
                return false;
            }
        }



    }
}