using UnityEngine;

namespace TownOfUs.Roles
{
    public class Impostor : Role
    {
        public override bool Hidden => RoleType == RoleEnum.Impostor;

        public Impostor(PlayerControl player) : base(player)
        {
            Faction = Faction.Impostors;
            RoleType = RoleEnum.Impostor;
        }
    }

    public class Crewmate : Role
    {
        public Crewmate(PlayerControl player) : base(player)
        {
            Hidden = true;
            Faction = Faction.Crewmates;
            RoleType = RoleEnum.Crewmate;
        }
    }
}
