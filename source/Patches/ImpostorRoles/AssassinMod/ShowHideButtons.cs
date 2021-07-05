using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine.UI;
using System.Linq;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class ShowHideButtons
    {
        public static void HideButtons(Assassin role)
        {
            foreach (var (_, (cycle, guess, guessText)) in role.Buttons)
            {
                if (cycle == null) continue;
                cycle.SetActive(false);
                guess.SetActive(false);
                guessText.gameObject.SetActive(false);

                cycle.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                guess.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                role.GuessedThisMeeting = true;
            }
        }

        public static void HideSingle(
            Assassin role,
            byte targetId,
            bool killedSelf
        )
        {
            if (
                killedSelf ||
                role.RemainingKills == 0 ||
                !CustomGameOptions.AssassinMultiKill
            )
            {
                HideButtons(role);
                return;
            }

            var (cycle, guess, guessText) = role.Buttons[targetId];
            if (cycle == null) return;
            cycle.SetActive(false);
            guess.SetActive(false);
            guessText.gameObject.SetActive(false);

            cycle.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            guess.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            role.Buttons[targetId] = (null, null, null);
            role.Guesses.Remove(targetId);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MeetingHud.Confirm))]
        public static void ConfirmVote(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Assassin)) return;
            var assassin = Role.GetRole<Assassin>(PlayerControl.LocalPlayer);
            HideButtons(assassin);
        }

        [HarmonyPrefix]
        [HarmonyPatch(
            nameof(MeetingHud.HandleDisconnect),
            typeof(PlayerControl), typeof(InnerNet.DisconnectReasons)
        )]
        public static void HandleDisconnect(
            MeetingHud __instance, [HarmonyArgument(0)] PlayerControl player)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Assassin)) return;
            var assassin = Role.GetRole<Assassin>(PlayerControl.LocalPlayer);
            HideSingle(assassin, player.PlayerId, false);
            var voteArea = __instance.playerStates.FirstOrDefault(
                area => area.TargetPlayerId == player.PlayerId
            );

            voteArea.AmDead = true;
            voteArea.Overlay.gameObject.SetActive(true);
        }
    }
}
