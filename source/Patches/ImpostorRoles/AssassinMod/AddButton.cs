using System;
using HarmonyLib;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class AddButton
    {
        private static Sprite CycleSprite => TownOfUs.CycleSprite;

        private static Sprite GuessSprite => TownOfUs.GuessSprite;

        public static void GenButton(Assassin role, int index, bool isDead)
        {
            if (isDead)
            {
                role.Buttons.Add((null, null));
                return;
            }

            var confirmButton = MeetingHud.Instance.playerStates[index].Buttons.transform.GetChild(0).gameObject;
            var parent = confirmButton.transform.parent.parent;

            var cycle = Object.Instantiate(confirmButton, MeetingHud.Instance.playerStates[index].transform);
            cycle.GetComponent<SpriteRenderer>().sprite = CycleSprite;
            cycle.transform.position = confirmButton.transform.position - new Vector3(0.5f, -0.15f, 0f);
            cycle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            cycle.layer = 5;
            cycle.transform.parent = parent;
            cycle.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            cycle.GetComponent<PassiveButton>().OnClick.AddListener(Cycle(role, index));
            var s = cycle.GetComponent<SpriteRenderer>().sprite.bounds.size;
            cycle.GetComponent<BoxCollider2D>().size = s;
            cycle.GetComponent<BoxCollider2D>().offset = Vector2.zero;
            cycle.transform.GetChild(0).gameObject.Destroy();


            var guess = Object.Instantiate(confirmButton, MeetingHud.Instance.playerStates[index].transform);
            guess.GetComponent<SpriteRenderer>().sprite = GuessSprite;
            guess.transform.position = confirmButton.transform.position - new Vector3(0.5f, 0.15f, 0f);
            guess.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            guess.layer = 5;
            guess.transform.parent = parent;
            guess.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            guess.GetComponent<PassiveButton>().OnClick.AddListener(Guess(role, index));
            var bounds = guess.GetComponent<SpriteRenderer>().bounds;
            bounds.size = new Vector3(0.52f, 0.3f, 0.16f);
            var s2 = guess.GetComponent<SpriteRenderer>().sprite.bounds.size;
            guess.GetComponent<BoxCollider2D>().size = s2;
            guess.GetComponent<BoxCollider2D>().offset = Vector2.zero;
            guess.transform.GetChild(0).gameObject.Destroy();


            role.Guesses.Add(index, "None");
            role.Buttons.Add((cycle, guess));
        }

        private static Action Cycle(Assassin role, int index)
        {
            void Listener()
            {
                if (MeetingHud.Instance.state == MeetingHud.VoteStates.Discussion) return;
                var currentGuess = role.Guesses[index];
                int guessIndex;
                if (currentGuess == "None")
                    guessIndex = -1;
                else
                    guessIndex = role.PossibleGuesses.IndexOf(currentGuess);

                guessIndex++;
                guessIndex %= role.PossibleGuesses.Count;

                role.Guesses[index] = role.PossibleGuesses[guessIndex];
            }

            return Listener;
        }

        private static Action Guess(Assassin role, int index)
        {
            void Listener()
            {
                if (MeetingHud.Instance.state == MeetingHud.VoteStates.Discussion) return;
                var state = MeetingHud.Instance.playerStates[index];
                var currentGuess = role.Guesses[index];
                if (currentGuess == "None") return;

                var playerRole = Role.GetRole(state);

                var toDie = playerRole.Name == currentGuess ? playerRole.Player : role.Player;

                AssassinKill.RpcMurderPlayer(toDie);
                role.RemainingKills--;
                ShowHideButtons.HideSingle(role, index, toDie == role.Player);
            }

            return Listener;
        }

        public static void Postfix(MeetingHud __instance)
        {
            foreach (var role in Role.GetRoles(RoleEnum.Assassin))
            {
                var assassin = (Assassin) role;
                assassin.Guesses.Clear();
                assassin.Buttons.Clear();
                assassin.GuessedThisMeeting = false;
            }

            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Assassin)) return;

            var assassinRole = Role.GetRole<Assassin>(PlayerControl.LocalPlayer);
            if (assassinRole.RemainingKills <= 0) return;
            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                var isDead = __instance.playerStates[i].AmDead;
                var isImp = Role.GetRole(__instance.playerStates[i]).Faction == Faction.Impostors;
                var canSeeRole = Role.GetRole(__instance.playerStates[i]).Criteria();

                GenButton(assassinRole, i, isDead || isImp || canSeeRole);
            }
        }
    }
}