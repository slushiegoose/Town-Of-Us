using HarmonyLib;
using Hazel;

namespace TownOfUs.JesterMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    public static class EndCriteria
    {
        public static bool JesterVotedOut = false;

        public static bool Prefix(ShipStatus __instance)
        {
            if (Utils.Jester == null) return true;
            if (!JesterVotedOut || !Utils.Jester.Data.IsDead) return true;
            
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.JesterWin, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Utils.Jester.Data.IsDead = false;
            ShipStatus.RpcEndGame(GameOverReason.ImpostorByVote, !SaveManager.BoughtNoAds);
            return false;
        }
        
    }
}