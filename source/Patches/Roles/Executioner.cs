using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Executioner : Role
    {
        public PlayerControl Target;
        public bool TargetVotedOut;

        public Executioner(PlayerControl player) : base(player)
        {
            ImpostorText = () =>
                Target == null ? "You don't have a target for some reason... weird..." : $"Vote {Target.name} out";
            TaskText = () =>
                Target == null
                    ? "You don't have a target for some reason... weird..."
                    : $"Vote {Target.name} out\nFake Tasks:";
            RoleType = RoleEnum.Executioner;
            Faction = Faction.Neutral;
        }

        public override void IntroPrefix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var executionerteam = new List<PlayerControl>();
            executionerteam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = executionerteam;
        }

        public override bool CheckEndCriteria(ShipStatus __instance)
        {
            if (Player.Data.IsDead) return true;
            if (!TargetVotedOut || !Target.Data.IsDead) return true;
            Utils.EndGame();
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
    }
}
