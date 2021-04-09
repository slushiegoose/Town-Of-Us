using HarmonyLib;
using Hazel;
using Reactor;
using UnityEngine;

namespace TownOfUs.JanitorMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKillButton
    
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Janitor);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            var role = Roles.Role.GetRole<Roles.Janitor>(PlayerControl.LocalPlayer);

            if (__instance == role.CleanButton)
            {

                var flag2 = __instance.isCoolingDown;
                if (flag2) return false;
                if (!__instance.enabled) return false;
                var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
                if (Vector2.Distance(role.CurrentTarget.TruePosition,
                    PlayerControl.LocalPlayer.GetTruePosition()) > maxDistance) return false;
                var playerId = role.CurrentTarget.ParentId;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.JanitorClean, SendOption.Reliable, -1);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                Coroutines.Start(Coroutine.CleanCoroutine(role.CurrentTarget, role));
                return false;
            }

            return true;
        }
        
        
    }
}