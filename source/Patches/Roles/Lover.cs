using System.Collections.Generic;
using System.Linq;
using Hazel;
using TownOfUs.ImpostorRoles.CamouflageMod;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Lover : Role
    {
        public Lover(PlayerControl player, bool impostor, bool eitherLoverImpostor) : base(player)
        {
            Name = impostor ? "Loving Impostor" : "Lover";
            Color = new Color(1f, 0.4f, 0.8f, 1f);
            ImpostorText = () =>
                "You are in " + ColorString + "Love</color> with " + ColorString + OtherLover.Player.name;
            TaskText = () => $"Stay alive with your love {OtherLover.Player.name} \n and win together";
            RoleType = impostor ? RoleEnum.LoverImpostor : RoleEnum.Lover;
            LoverImpostor = eitherLoverImpostor;
            Scale = impostor ? 2.3f : 1f;
            Faction = impostor ? Faction.Impostors : Faction.Crewmates;
        }

        public Lover OtherLover { get; set; }
        public bool LoveCoupleWins { get; set; }
        
        // Returns true if either lover is an impostor
    
        public bool LoverImpostor { get; set; }

        protected override void IntroPrefix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var loverTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            loverTeam.Add(PlayerControl.LocalPlayer);
            loverTeam.Add(OtherLover.Player);
            __instance.yourTeam = loverTeam;
        }

        internal override bool Criteria()
        {
            return base.Criteria() || GetRole(PlayerControl.LocalPlayer) == OtherLover;
        }

        protected override string NameText(PlayerVoteArea player = null)
        {
            if (CamouflageUnCamouflage.IsCamoed && player == null) return "";
            if (PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer == Player)
                return base.NameText(player);
            if (player != null && (MeetingHud.Instance.state == MeetingHud.VoteStates.Proceeding ||
                                   MeetingHud.Instance.state == MeetingHud.VoteStates.Results)) return Player.name;
            if (!CustomGameOptions.RoleUnderName && player == null) return Player.name;
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.HatId == 0U ? 1.5f : 2f,
                -0.5f
            );
            if (PlayerControl.LocalPlayer.Data.IsImpostor && RoleType == RoleEnum.LoverImpostor)
            {
                Player.nameText.color = Palette.ImpostorRed;
                if (player != null) player.NameText.color = Palette.ImpostorRed;
                return Player.name + "\n" + "Impostor";
            }


            return Player.name + "\n" + "Lover";
        }

        private static readonly int LOVING_IMPOSTOR_CHANCE = 25;
        public static void Gen(List<PlayerControl> crewmates, List<PlayerControl> impostors)
        {
            var canMakeCrewCrewLovers = crewmates.Count >= 2;
            var canMakeCrewImpostorLovers = crewmates.Count >= 1 && impostors.Count >= 2 && CustomGameOptions.AllowLovingImpostor;
            if (!canMakeCrewCrewLovers && !canMakeCrewImpostorLovers) {
                return;
            }

            bool lovingImpostor;
            if(canMakeCrewCrewLovers && !canMakeCrewImpostorLovers) {
                lovingImpostor = false;
            } else if (!canMakeCrewCrewLovers && canMakeCrewImpostorLovers) {
                lovingImpostor = true;
            } else {
                lovingImpostor = Random.RandomRangeInt(1, 101) <= LOVING_IMPOSTOR_CHANCE;
            }

            var num = Random.RandomRangeInt(0, crewmates.Count);
            var player1 = crewmates[num];
            crewmates.Remove(player1);
            PlayerControl player2;
            if (lovingImpostor)
            {
                var num2 = Random.RandomRangeInt(0, impostors.Count);
                player2 = impostors[num2];
                impostors.Remove(player2);
            }
            else
            {
                var num2 = Random.RandomRangeInt(0, crewmates.Count);
                player2 = crewmates[num2];
                crewmates.Remove(player2);
            }

            // These writes appear to be read by `case CustomRPC.SetCouple` in RpcHandling.cs
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SetCouple, SendOption.Reliable, -1);
            writer.Write(player1.PlayerId);
            writer.Write(player2.PlayerId);
            writer.Write(lovingImpostor);

            var lover1 = new Lover(player1, false, lovingImpostor);
            var lover2 = new Lover(player2, lovingImpostor, lovingImpostor);

            lover1.OtherLover = lover2;
            lover2.OtherLover = lover1;

            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (FourPeopleLeft()) return false;

            if (CheckLoversWin())
            {
                //System.Console.WriteLine("LOVERS WIN");
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.LoveWin, SendOption.Reliable, -1);
                writer.Write(Player.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Win();
                Utils.EndGame();
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
                return !lover1.Data.IsDead && !lover2.Data.IsDead &&
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
                   (alives.Count == 3) | (alives.Count == 2);
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
    }
}
