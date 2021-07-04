using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Assassin : Role
    {
        public Dictionary<byte, (GameObject, GameObject, TMP_Text)> Buttons = new Dictionary<byte, (GameObject, GameObject, TMP_Text)>();

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

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Count: {PossibleGuesses.Count}");
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }
    }
}
