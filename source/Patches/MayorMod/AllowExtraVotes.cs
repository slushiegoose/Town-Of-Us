using System.Collections.Generic;
using HarmonyLib;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(PlayerVoteArea))]
    public class AllowExtraVotes
    {
        public static List<byte> ExtraVotes = new List<byte>();
        public static int VoteBank;
        
        
        [HarmonyPatch(nameof(PlayerVoteArea.Select))]
        public static bool Prefix(PlayerVoteArea __instance)
        {
            if (!PlayerControl.LocalPlayer.isMayor()) return true;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            if (__instance.isDead) return false;
            if (VoteBank <= 0 || !__instance.Parent.Select(__instance.TargetPlayerId)) return false;
            __instance.Buttons.SetActive(true);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerVoteArea.VoteForMe))]
        public static bool Prefix2(PlayerVoteArea __instance)
        {
            if (!PlayerControl.LocalPlayer.isMayor()) return true;
            if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding ||
                __instance.Parent.state == MeetingHud.VoteStates.Results)
            {
                return false;
            }
            if (VoteBank <= 0) return false;
            VoteBank--;
            __instance.Parent.Confirm(__instance.TargetPlayerId);
            return false;
        }
        
        

    }
}