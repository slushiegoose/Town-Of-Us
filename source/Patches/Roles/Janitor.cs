using System;

namespace TownOfUs.Roles
{
    public class Janitor : Role
    {
        public DateTime LastCleaned {get; set;}
        public DeadBody CurrentTarget {get; set;}

        public KillButtonManager _cleanButton;

        public KillButtonManager CleanButton
        {
            get => _cleanButton;
            set
            {
                _cleanButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }
        
        public Janitor(PlayerControl player) : base(player)
        {
            Name = "Janitor";
            ImpostorText = () => "Clean up bodies";
            TaskText = () => "Clean bodies to prevent Crewmates from discovering them.";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Janitor;
            Faction = Faction.Impostors;
        }
    }
}