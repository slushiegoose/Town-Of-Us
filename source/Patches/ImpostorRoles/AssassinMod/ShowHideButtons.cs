using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine.UI;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
    public class ShowHideButtons
    {
        public static void HideButtons(Assassin role)
        {
            foreach (var (_, (cycle, guess)) in role.Buttons)
            {
                if (cycle == null) continue;
                cycle.SetActive(false);
                guess.SetActive(false);

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

            var (cycle, guess) = role.Buttons[targetId];
            if (cycle == null) return;
            cycle.SetActive(false);
            guess.SetActive(false);

            cycle.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            guess.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            role.Buttons[targetId] = (null, null);
            role.Guesses.Remove(targetId);
        }


        public static void Prefix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Assassin)) return;
            var assassin = Role.GetRole<Assassin>(PlayerControl.LocalPlayer);
            HideButtons(assassin);
        }
    }
}
