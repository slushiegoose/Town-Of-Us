using HarmonyLib;
using TownOfUs.Extensions;
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
                ;

                if (
                    assassin.GuessedThisMeeting ||
                    !assassin.Guesses.TryGetValue(targetId, out var currentGuessIdx)
                ) continue;

                var currentGuess = currentGuessIdx == -1
                    ? RoleEnum.None
                    : assassin.PossibleGuesses[currentGuessIdx];

                var playerData = Utils.PlayerById(targetId)?.Data;
                if (playerData == null || playerData.Disconnected)
                {
                    assassin.Guesses.Remove(targetId);
                    ShowHideButtons.HideSingle(assassin, targetId, false);
                    continue;
                }

                var nameText = "\n" + (currentGuess == RoleEnum.None
                    ? "Guess"
                    : "<color=#" +
                        Role.GetColor(currentGuess).ToHtmlStringRGBA() +
                    $">{Role.GetName(currentGuess)}??</color>"
                );

                voteArea.NameText.text += nameText;
                voteArea.NameText.transform.localPosition = new Vector3(0.6f, 0.03f, -0.1f);
            }
        }
    }
}
