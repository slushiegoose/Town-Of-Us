using HarmonyLib;
using Hazel;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Jester : Role
    {

        public bool VotedOut = false;

        protected override void IntroPrefix(IntroCutscene.CoBegin__d __instance)
        {
            var jesterTeam = new List<PlayerControl>();
            jesterTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = jesterTeam;
        }

        protected override bool CheckEndCriteria(ShipStatus __instance)
        {
            if (!VotedOut || !Player.Data.IsDead) return true;
            ShipStatus.RpcEndGame(GameOverReason.ImpostorByVote, false);
            return false;
        }

        public void Wins()
        {
            //System.Console.WriteLine("Reached Here - Jester edition");
            VotedOut = true;
        }

        public void Loses()
        {
            Player.Data.IsImpostor = true;
        }
        
        
        public Jester(PlayerControl player) : base(player)
        {
            Name = "Jester";
            ImpostorText = () => "Get voted out";
            TaskText = () => "Get voted out!\nFake Tasks:";
            Color = new Color(1f, 0.75f, 0.8f, 1f);
            RoleType = RoleEnum.Jester;
            Faction = Faction.Neutral;
        }
        
  }
}