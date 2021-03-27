using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Arsonist : Role
    {
        public List<byte> DousedPlayers = new List<byte>();
        public bool IgniteUsed;
        public bool ArsonistWins;
        public DateTime LastDoused;

        private KillButtonManager _igniteButton;

        public KillButtonManager IgniteButton
        {
            get { return _igniteButton;}
            set
            {
                _igniteButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }
        public PlayerControl closestPlayer;


        public Arsonist(PlayerControl player) : base(player)
        {
            Name = "Arsonist";
            ImpostorText = () => "Douse players and ignite the light";
            TaskText = () => "Douse players and ignite to kill everyone\nFake Tasks:";
            Color = new Color(1f, 0.3f, 0f);
            RoleType = RoleEnum.Arsonist;
            Faction = Faction.Neutral;
        }

        protected override bool CheckEndCriteria(ShipStatus __instance)
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
                ShipStatus.RpcEndGame(GameOverReason.ImpostorByVote, false);
                return false;

            }

            if (IgniteUsed || Player.Data.IsDead) return true;
            
            return !CustomGameOptions.ArsonistGameEnd;
        }


        public void Wins()
        {
            //System.Console.WriteLine("Reached Here - Glitch Edition");
            ArsonistWins = true;
        }
        public void Loses()
        {
            Player.Data.IsImpostor = true;
        }

        public bool CheckEveryoneDoused()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == Player.PlayerId) continue;
                if (player.Data.IsDead) continue;
                if (player.Data.Disconnected) continue;
                if (!DousedPlayers.Contains(player.PlayerId)) return false;
            }

            return true;
        }
        
        protected override void IntroPrefix(IntroCutscene.CoBegin__d __instance)
        {
            var arsonistTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            arsonistTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = arsonistTeam;
        }
        
        public float DouseTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastDoused;
            var num = CustomGameOptions.DouseCd * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
        
    }
}