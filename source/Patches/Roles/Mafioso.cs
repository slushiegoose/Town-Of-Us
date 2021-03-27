namespace TownOfUs.Roles
{
    public class Mafioso : Role
    {
        public Janitor Janitor {get; set;} 
        public Godfather Godfather {get; set;} 
        
        protected internal override string NameText()
        {
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed)
            {
                return "";
            }
            return Player.name + " (M)";
        }
        
        protected override bool Criteria()
        {
            var localPlayerRole = GetRole(PlayerControl.LocalPlayer);

            return localPlayerRole == this || localPlayerRole == Janitor || localPlayerRole == Godfather;

        }
        
        public Mafioso(PlayerControl player) : base(player)
        {
            Name = "Mafioso";
            ImpostorText = () => "Work with the Mafia to kill the Crewmates";
            TaskText = () => "Inherit the Godfather once they die.";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Mafioso;
            Faction = Faction.Impostors;
        }
    }
}