using HarmonyLib;
using Hazel;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKillButton

    {

        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.isTimeMaster();
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = Methods.TimeMasterRewindTimer() == 0f & !RecordRewind.rewinding;
            if (!flag2) return false;
            StartStop.StartRewind();
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Rewind, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return false;
        }
    }
}