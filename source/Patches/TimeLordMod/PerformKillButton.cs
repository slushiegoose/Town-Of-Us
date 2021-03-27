using HarmonyLib;
using Hazel;

namespace TownOfUs.TimeLordMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKillButton

    {

        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.TimeLord);
            if (!flag) return true;
            var role = Roles.Role.GetRole<Roles.TimeLord>(PlayerControl.LocalPlayer);
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = role.TimeLordRewindTimer() == 0f & !RecordRewind.rewinding;
            if (!flag2) return false;
            if (!__instance.enabled) return false;
            StartStop.StartRewind(role);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Rewind, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return false;
        }
    }
}