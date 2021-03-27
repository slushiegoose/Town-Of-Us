using HarmonyLib;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    public static class VoteOut
    {
        
        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo exiled, ExileController __instance)
        {
            if (Utils.Jester == null) return;
            if (exiled == null) return;
            if (exiled.PlayerId != Utils.Jester.PlayerId) return;
            EndCriteria.JesterVotedOut = true;
            JesterWins();
        }

        private static void JesterWins()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.Data.IsImpostor = player.isJester();
            }
        }
    }
}