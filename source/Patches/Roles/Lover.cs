using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using Reactor.Extensions;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Lover : Role
    {
        public Lover OtherLover {get; set;} 
        public bool LoveCoupleWins {get; set;} 
        public int Num {get; set;} 
        public bool LoverImpostor {get; set;} 

        protected override void IntroPrefix(IntroCutscene.CoBegin__d __instance)
        {
            var loverTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            loverTeam.Add(PlayerControl.LocalPlayer);
            loverTeam.Add(OtherLover.Player);
            __instance.yourTeam = loverTeam;
        }

        protected override bool Criteria()
        {
            return base.Criteria() || GetRole(PlayerControl.LocalPlayer) == OtherLover;
        }


        public static void Gen(List<PlayerControl> crewmates, List<PlayerControl> impostors)
        {
            //System.Console.WriteLine("LOVER2");
            if (crewmates.Count <= 0) return;
            if (crewmates.Count <= 1 && impostors.Count < 1) return;

            //System.Console.WriteLine("LOVER3");
            var b = HashRandom.Method_1(3);

            if (b == 0 & impostors.Count < 1) b = 1;
            
            if (b != 0 & crewmates.Count <= 1) b = 0;

            //System.Console.WriteLine("LOVER4");
            var flag2 = b == 0;
            var num = HashRandom.Method_1(crewmates.Count);
            var player1 = crewmates[num];
            crewmates.Remove(player1);
            PlayerControl player2;
            if (flag2)
            {
                var num2 = HashRandom.Method_1(impostors.Count);
                player2 = impostors[num2];
                impostors.Remove(player2);
            }
            else
            {
                var num2 = HashRandom.Method_1(crewmates.Count);
                player2 = crewmates[num2];
                crewmates.Remove(player2);
            }

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SetCouple, SendOption.Reliable, -1);
            writer.Write(player1.PlayerId);
            writer.Write(player2.PlayerId);
            writer.Write(b);
            var lover1 = new Lover(player1, 1, b == 0);
            var lover2 = new Lover(player2, 2, b == 0);

            lover1.OtherLover = lover2;
            lover2.OtherLover = lover1;

            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        
        protected override bool CheckEndCriteria(ShipStatus __instance)
        {
            if (FourPeopleLeft())
            {
                return false;
            }

            if (CheckLoversWin())
            {
                //System.Console.WriteLine("LOVERS WIN");
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.LoveWin, SendOption.Reliable, -1);
                writer.Write(Player.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Win();
                ShipStatus.RpcEndGame(GameOverReason.ImpostorByVote, false);
                return false;
            }

            return true;
        }


        private bool FourPeopleLeft()
        {
            var players = PlayerControl.AllPlayerControls.ToArray();
            var alives = players.Where(x => !x.Data.IsDead).ToList();
            var lover1 = Player;
            var lover2 = OtherLover.Player;
            {
                return  !lover1.Data.IsDead && !lover2.Data.IsDead &&
                       alives.Count() == 4 && LoverImpostor;
            }
        }

        private bool CheckLoversWin()
        {
            //System.Console.WriteLine("CHECKWIN");
            var players = PlayerControl.AllPlayerControls.ToArray();
            var alives = players.Where(x => !x.Data.IsDead).ToList();
            var lover1 = Player;
            var lover2 = OtherLover.Player;

            return !lover1.Data.IsDead && !lover2.Data.IsDead &&
                   alives.Count == 3 | alives.Count == 2;
        }

        public void Win()
        {
            if (AllRoles.Where(x => x.RoleType == RoleEnum.Jester).Any(x => ((Jester) x).VotedOut)) return;
            /*var lover1 = Player;
            var lover2 = OtherLover.Player;
            //System.Console.WriteLine("reached révoila");
            lover1.Data.IsImpostor = true;
            lover1.Data.IsDead = false;
            lover2.Data.IsImpostor = true;
            lover2.Data.IsDead = false;
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == lover1.PlayerId) continue;
                if (player.PlayerId == lover2.PlayerId) continue;
                player.RemoveInfected();
                player.Die(0);
                player.Data.IsDead = true;
                player.Data.IsImpostor = false;
            }*/

            LoveCoupleWins = true;
            OtherLover.LoveCoupleWins = true;
        }

        
        
        public Lover(PlayerControl player, int num, bool loverImpostor) : base(player)
        {
            var imp = num == 2 && loverImpostor;
            Name = imp ? "Loving Impostor" : "Lover";
            Color = new Color(1f, 0.4f, 0.8f, 1f);
            ImpostorText = () => "You are in " + ColorString + "Love[] with " + ColorString + OtherLover.Player.name;
            TaskText = () => $"Stay alive with your love {OtherLover.Player.name} \n and win together";
            RoleType = imp ? RoleEnum.LoverImpostor : RoleEnum.Lover;
            Num = num;
            LoverImpostor = loverImpostor;
            Scale = imp ? 2.3f : 1f;
            Faction = imp ? Faction.Impostors : Faction.Crewmates;

        }
    }
}