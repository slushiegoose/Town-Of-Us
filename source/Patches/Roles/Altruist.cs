using UnityEngine;

namespace TownOfUs.Roles
{
    public class Altruist : Role
    {
        public bool CurrentlyReviving;
        public DeadBody CurrentTarget;

        public bool ReviveUsed;

        public Altruist(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Sacrifice yourself to save another";
            TaskText = () => "Revive a dead body at the cost of your own life.";
            RoleType = RoleEnum.Altruist;
        }
    }
}
