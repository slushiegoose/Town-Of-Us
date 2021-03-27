using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Mayor : Role
    {
        public List<byte> ExtraVotes = new List<byte>();
        public int VoteBank { get; set;}

        public Mayor(PlayerControl player) : base(player)
        {
            Name = "Mayor";
            ImpostorText = () => "Save your votes to double vote";
            TaskText = () => "Save your votes to vote multiple times";
            Color = new Color(0.44f, 0.31f, 0.66f, 1f);
            RoleType = RoleEnum.Mayor;
            VoteBank = CustomGameOptions.MayorVoteBank;
        }
    }
}