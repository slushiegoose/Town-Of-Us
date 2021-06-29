using HarmonyLib;
using Reactor.Extensions;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public class UpdateMeetingHud
    {
        private static void Postfix(MeetingHud __instance)
        {
            var role = Role.GetRole(PlayerControl.LocalPlayer);
            if (role?.RoleType != RoleEnum.Assassin) return;
            var assassin = (Assassin)role;
            foreach (var voteArea in __instance.playerStates)
            {
                var targetId = voteArea.TargetPlayerId;
                assassin.Guesses.TryGetValue(targetId, out var currentGuess);

                if (
                    assassin.GuessedThisMeeting ||
                    string.IsNullOrEmpty(currentGuess)
                ) continue;

                var nameText = "\n" + (currentGuess == "None"
                    ? "Guess"
                    : "<color=#" +
                        assassin.ColorMapping[currentGuess].ToHtmlStringRGBA() +
                    $">{currentGuess}??</color>"
                );

                voteArea.NameText.text += nameText;
                voteArea.NameText.transform.localPosition = new Vector3(0.6f, 0.03f, -0.1f);
            }
        }
    }
}
