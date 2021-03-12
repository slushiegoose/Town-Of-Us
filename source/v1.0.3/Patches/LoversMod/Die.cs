using HarmonyLib;
using Hazel;


namespace TownOfUs.LoversMod
{ 
    [HarmonyPatch(typeof(PlayerControl))]
    public class Die
    {
        [HarmonyPatch("Die")]
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] DeathReason oecopgmhmkc)
        {
            var flag = AmongUsClient.Instance.ClientId == AmongUsClient.Instance.HostId;
            if (!flag) return true;
            __instance.Data.IsDead = true;

            var flag3 = __instance.isLover() && CustomGameOptions.BothLoversDie;
            if (!flag3) return true;
            var otherLover = __instance.OtherLover();
            if (otherLover.Data.IsDead) return true;
            var messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.LoveSuicide, SendOption.Reliable, -1);
            messageWriter2.Write(otherLover.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
            otherLover.MurderPlayer(otherLover);
            return true;
        }
    }
}