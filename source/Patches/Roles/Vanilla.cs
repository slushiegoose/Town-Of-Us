using UnityEngine;

namespace TownOfUs.Roles
{
    public class Impostor : Role
    {
        public override bool Hidden => RoleType == RoleEnum.Impostor;

        public Impostor(PlayerControl player) : base(player)
        {
            Name = "Impostor";
            Faction = Faction.Impostors;
            RoleType = RoleEnum.Impostor;
            Color = Palette.ImpostorRed;
        }
    }

    public class Crewmate : Role
    {
        public Crewmate(PlayerControl player) : base(player)
        {
            Name = "Crewmate";
            Hidden = true;
            Faction = Faction.Crewmates;
            RoleType = RoleEnum.Crewmate;
            Color = Color.white;
        }
    }
}
