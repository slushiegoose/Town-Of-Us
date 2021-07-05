using System.Collections.Generic;
using System.Linq;
using Hazel;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Arsonist : Role
    {
        public bool ArsonistWins;
        public PlayerControl ClosestPlayer;
        public List<byte> DousedPlayers = new List<byte>();
        public bool IgniteUsed;

        public Arsonist(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Douse players and ignite the light";
            TaskText = () => "Douse players and ignite to kill everyone\nFake Tasks:";
            RoleType = RoleEnum.Arsonist;
            Faction = Faction.Neutral;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = DouseCallback,
                    MaxTimer = CustomGameOptions.DouseCd,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    TargetFilter = player => !IsDoused(player),
                    Icon = TownOfUs.DouseSprite,
                    Position = AbilityPositions.KillButton
                });

                AbilityManager.Add(new PlainAbilityData
                {
                    Callback = IgniteCallback,
                    IsHighlighted = CanIgnite,
                    MaxTimer = 10f,
                    Icon = TownOfUs.IgniteSprite,
                    Position = AbilityPositions.OverKillButton
                });
            }
        }

        public bool CanIgnite()
        {
            if (IgniteUsed) return false;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == Player.PlayerId) continue;

                var data = player.Data;

                if (data.IsDead || data.Disconnected) continue;

                if (!DousedPlayers.Contains(player.PlayerId))
                    return false;
            }
            return true;
        }

        public void IgniteCallback()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Ignite, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            foreach (var playerId in DousedPlayers)
            {
                var player = Utils.PlayerById(playerId);
                if (player == null) continue;
                var data = player.Data;
                if (data.IsDead || data.Disconnected) continue;
                Utils.MurderPlayer(player, player);
            }

            Utils.MurderPlayer(Player, Player);

            IgniteUsed = true;
        }

        public bool IsDoused(PlayerControl player) => DousedPlayers.Contains(player.PlayerId);

        public void DouseCallback(PlayerControl player)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Douse, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            DousedPlayers.Add(player.PlayerId);
            NamePatch.UpdateSingle(player);
            NamePatch.UpdateDisplay(player);
        }

        public override bool CheckEndCriteria(ShipStatus __instance)
        {
            if (PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) == 0)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.ArsonistWin,
                    SendOption.Reliable,
                    -1
                );
                writer.Write(Player.PlayerId);
                Wins();
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            if (IgniteUsed || Player.Data.IsDead) return true;

            return !CustomGameOptions.ArsonistGameEnd;
        }


        public void Wins()
        {
            ArsonistWins = true;
        }

        public void Loses()
        {
            Player.Data.IsImpostor = true;
        }

        public override void IntroPrefix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var arsonistTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            arsonistTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = arsonistTeam;
        }
    }
}
