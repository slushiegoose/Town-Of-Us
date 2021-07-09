using System.Collections.Generic;
using TownOfUs.CustomHats;
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

        public Snitch(PlayerControl player) : base(player)
        {
            Name = "Snitch";
            ImpostorText = () => "Complete all your tasks to discover the Impostors";
            TaskText = () =>
                TasksDone
                    ? "Find the arrows pointing to the Impostors!"
                    : "Complete all your tasks to discover the Impostors!";
            Color = new Color(0.83f, 0.69f, 0.22f, 1f);
            Hidden = !CustomGameOptions.SnitchOnLaunch;
            RoleType = RoleEnum.Snitch;
        }

        public bool OneTaskLeft => TasksLeft <= 1;
        public bool TasksDone => TasksLeft <= 0;

        internal override bool Criteria()
        {
            return Player.AmOwner || (OneTaskLeft && PlayerControl.LocalPlayer.Data.IsImpostor) || base.Criteria();
        }
    }
}
