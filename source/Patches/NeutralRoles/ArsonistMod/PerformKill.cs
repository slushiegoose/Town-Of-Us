using System;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.ArsonistMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist);
            if (!flag) return true;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var role = Role.GetRole<Arsonist>(PlayerControl.LocalPlayer);
            if (role.IgniteUsed) return false;
            if (__instance == role.IgniteButton)
            {
                if (!__instance.isActiveAndEnabled) return false;
                if (!role.CheckEveryoneDoused()) return false;
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.Ignite, SendOption.Reliable, -1);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Ignite(role);
                return false;
            }

            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            if (!__instance.isActiveAndEnabled) return false;
            if (role.ClosestPlayer == null) return false;
            if (role.DouseTimer() != 0) return false;
            if (role.DousedPlayers.Contains(role.ClosestPlayer.PlayerId)) return false;
            var distBetweenPlayers = Utils.getDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
            var flag3 = distBetweenPlayers <
                        GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (!flag3) return false;
            var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Douse, SendOption.Reliable, -1);
            writer2.Write(PlayerControl.LocalPlayer.PlayerId);
            writer2.Write(role.ClosestPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer2);
            role.DousedPlayers.Add(role.ClosestPlayer.PlayerId);
            role.LastDoused = DateTime.UtcNow;

            __instance.SetTarget(null);
            return false;
        }

        public static void Ignite(Arsonist role)
        {
            foreach (var playerId in role.DousedPlayers)
            {
                var player = Utils.PlayerById(playerId);
                if (
                    player == null ||
                    player.Data.Disconnected ||
                    player.Data.IsDead
                ) continue;
                Utils.MurderPlayer(player, player);
            }

            Utils.MurderPlayer(role.Player, role.Player);


            role.IgniteUsed = true;
        }
    }
}
