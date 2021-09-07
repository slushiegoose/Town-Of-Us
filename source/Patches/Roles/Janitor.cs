namespace TownOfUs.Roles
{
    public class Janitor : Role
    {
        public KillButtonManager _cleanButton;

        public Janitor(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Clean up bodies";
            TaskText = () => "Clean bodies to prevent Crewmates from discovering them.";
            RoleType = RoleEnum.Janitor;
            Faction = Faction.Impostors;
        }

        public DeadBody CurrentTarget { get; set; }

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
    }
}
