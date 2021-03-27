using Hazel;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Executioner : Role
    {
        public bool TargetVotedOut = false;
        public PlayerControl target;
        
        protected override void IntroPrefix(IntroCutscene.CoBegin__d __instance)
        {
            var executionerteam = new List<PlayerControl>();
            executionerteam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = executionerteam;
        }

        protected override bool CheckEndCriteria(ShipStatus __instance)
        {
            if (Player.Data.IsDead) return true;
            if (!TargetVotedOut || !target.Data.IsDead) return true;
            ShipStatus.RpcEndGame(GameOverReason.ImpostorByVote, false);
            return false;
        }
        
        public void Wins()
        {
            if (Player.Data.IsDead || Player.Data.Disconnected) return;
            TargetVotedOut = true;


        }
        public void Loses()
        {
            Player.Data.IsImpostor = true;
        }

        public Executioner(PlayerControl player) : base(player)
        {
            Name = "Executioner";
            ImpostorText = () => $"Vote {target.name} out";
            TaskText = () => $"Vote {target.name} out\nFake Tasks:";
            Color = new Color(0.55f, 0.25f, 0.02f, 1f);
            RoleType = RoleEnum.Executioner;
            Faction = Faction.Neutral;
            Scale = 1.4f;
        }
    }
}