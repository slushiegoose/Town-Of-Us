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
            foreach (var (cycle, guess) in role.Buttons)
            {
                if (cycle == null) continue;
                cycle.SetActive(false);
                guess.SetActive(false);

                cycle.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                guess.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                role.GuessedThisMeeting = true;
            }
        }

        public static void HideSingle(Assassin role, int index, bool isDead)
        {
            if (isDead || role.RemainingKills == 0 || !CustomGameOptions.AssassinMultiKill)
            {
                HideButtons(role);
                return;
            }

            var (cycle, guess) = role.Buttons[index];
            if (cycle == null) return;
            cycle.SetActive(false);
            guess.SetActive(false);

            cycle.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            guess.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            role.Buttons[index] = (null, null);
            role.Guesses.Remove(index);
        }


        public static void Prefix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Assassin)) return;
            var assassin = Role.GetRole<Assassin>(PlayerControl.LocalPlayer);
            HideButtons(assassin);
        }
    }
}