﻿using System.Collections.Generic;
using TownOfUs.ImpostorRoles.CamouflageMod;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Snitch : Role
    {
        public List<ArrowBehaviour> ImpArrows = new List<ArrowBehaviour>();

        public List<ArrowBehaviour> SnitchArrows = new List<ArrowBehaviour>();

        public List<PlayerControl> SnitchTargets = new List<PlayerControl>();

        public int TasksLeft = int.MaxValue;

        public override bool Hidden => !CustomGameOptions.SnitchOnLaunch && !OneTaskLeft;

        public Snitch(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Complete all your tasks to discover the Impostors";
            TaskText = () =>
                TasksDone
                    ? "Find the arrows pointing to the Impostors!"
                    : "Complete all your tasks to discover the Impostors!";
            RoleType = RoleEnum.Snitch;
        }

        public bool OneTaskLeft => TasksLeft <= 1;
        public bool TasksDone => TasksLeft <= 0;

        public override bool Criteria()
        {
            return (
                Player.AmOwner && !Player.Data.Disconnected
            ) || (OneTaskLeft && PlayerControl.LocalPlayer.Data.IsImpostor) || base.Criteria();
        }
    }
}
