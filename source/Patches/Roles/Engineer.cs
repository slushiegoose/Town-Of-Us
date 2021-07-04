using UnityEngine;

namespace TownOfUs.Roles
{
    public class Engineer : Role
    {
        public Engineer(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Maintain important systems on the ship";
            TaskText = () => "Vent and fix a sabotage from anywhere!";
            RoleType = RoleEnum.Engineer;
        }

        public bool UsedThisRound { get; set; } = false;
    }
}
