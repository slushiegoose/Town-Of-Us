using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.MayorMod
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
                var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (PlayerControl.LocalPlayer.Data.IsDead) return false;
                if (__instance.AmDead) return false;
                if (!role.CanVote || !__instance.Parent.Select(__instance.TargetPlayerId)) return false;
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
                var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding ||
                    __instance.Parent.state == MeetingHud.VoteStates.Results)
                    return false;

                if (!role.CanVote) return false;
                if (__instance != role.Abstain)
                {
                    role.VoteBank--;
                    role.VotedOnce = true;
                }
                else
                {
                    role.SelfVote = true;
                }

                __instance.Parent.Confirm(__instance.TargetPlayerId);
                return false;
            }
        }
    }
}