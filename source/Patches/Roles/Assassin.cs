using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Assassin : Role
    {
        public Dictionary<byte, (GameObject, GameObject)> Buttons = new Dictionary<byte, (GameObject, GameObject)>();

        public Dictionary<byte, int> Guesses = new Dictionary<byte, int>();

        public List<RoleEnum> PossibleGuesses = new List<RoleEnum>();
        public Assassin(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Kill during meetings if you can guess their roles";
            TaskText = () => "Guess the roles of the people and kill them mid-meeting";
            RoleType = RoleEnum.Assassin;
            Faction = Faction.Impostors;

            RemainingKills = CustomGameOptions.AssassinKills;

            PossibleGuesses = CustomGameOptions.GetEnabledRoles(
                CustomGameOptions.AssassinGuessNeutrals
            );

            if (CustomGameOptions.AssassinCrewmateGuess)
                PossibleGuesses.Add(RoleEnum.Crewmate);

        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }
    }
}
