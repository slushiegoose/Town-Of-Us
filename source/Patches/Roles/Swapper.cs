using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Swapper : Role
    {
        public readonly List<GameObject> Buttons = new List<GameObject>();

        public readonly List<bool> ListOfActives = new List<bool>();


        public Swapper(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Swap the votes of two people";
            TaskText = () => "Swap two people's votes and wreak havoc!";
            RoleType = RoleEnum.Swapper;
        }
    }
}
