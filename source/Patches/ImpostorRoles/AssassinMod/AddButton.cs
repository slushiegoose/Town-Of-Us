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

        private static bool IsExempt(PlayerVoteArea voteArea) {
            if (voteArea.AmDead) return true;
            var role = Role.GetRole(voteArea);
            var player = Utils.PlayerById(voteArea.TargetPlayerId);
            if (
                player.Data.IsImpostor ||
                player.Data.IsDead ||
                player.Data.Disconnected
            ) return true;
            return role != null && role.Criteria();
        }


        public static void GenButton(Assassin role, PlayerVoteArea voteArea)
        {
            var targetId = voteArea.TargetPlayerId;
            if (IsExempt(voteArea))
            {
                role.Buttons[targetId] = (null, null);
                return;
            }

            var confirmButton = voteArea.Buttons.transform.GetChild(0).gameObject;
            var parent = confirmButton.transform.parent.parent;

            var cycle = Object.Instantiate(confirmButton, voteArea.transform);
            var cycleRenderer = cycle.GetComponent<SpriteRenderer>();
            cycleRenderer.sprite = CycleSprite;
            cycle.transform.position = confirmButton.transform.position - new Vector3(0.5f, -0.15f, 0f);
            cycle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            cycle.layer = 5;
            cycle.transform.parent = parent;
            var cycleEvent = new Button.ButtonClickedEvent();
            cycleEvent.AddListener(Cycle(role, voteArea));
            cycle.GetComponent<PassiveButton>().OnClick = cycleEvent;
            var cycleCollider = cycle.GetComponent<BoxCollider2D>();
            cycleCollider.size = cycleRenderer.sprite.bounds.size;
            cycleCollider.offset = Vector2.zero;
            cycle.transform.GetChild(0).gameObject.Destroy();


            var guess = Object.Instantiate(confirmButton, voteArea.transform);
            var guessRenderer = guess.GetComponent<SpriteRenderer>();
            guessRenderer.sprite = GuessSprite;
            guess.transform.position = confirmButton.transform.position - new Vector3(0.5f, 0.15f, 0f);
            guess.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            guess.layer = 5;
            guess.transform.parent = parent;
            var guessEvent = new Button.ButtonClickedEvent();
            guessEvent.AddListener(Guess(role, voteArea));
            guess.GetComponent<PassiveButton>().OnClick = guessEvent;
            var bounds = guess.GetComponent<SpriteRenderer>().bounds;
            bounds.size = new Vector3(0.52f, 0.3f, 0.16f);
            var guessCollider = guess.GetComponent<BoxCollider2D>();
            guessCollider.size = guessRenderer.sprite.bounds.size;
            guessCollider.offset = Vector2.zero;
            guess.transform.GetChild(0).gameObject.Destroy();


            role.Guesses.Add(targetId, "None");
            role.Buttons[targetId] = (cycle, guess);
        }

        private static Action Cycle(Assassin role, PlayerVoteArea voteArea)
        {
            void Listener()
            {
                if (MeetingHud.Instance.state == MeetingHud.VoteStates.Discussion) return;
                var currentGuess = role.Guesses[voteArea.TargetPlayerId];
                var guessIndex = currentGuess == "None"
                    ? -1
                    : role.PossibleGuesses.IndexOf(currentGuess);
                if (++guessIndex == role.PossibleGuesses.Count)
                    guessIndex = 0;

                role.Guesses[voteArea.TargetPlayerId] = role.PossibleGuesses[guessIndex];
            }

            return Listener;
        }

        private static Action Guess(Assassin role, PlayerVoteArea voteArea)
        {
            void Listener()
            {
                if (
                    MeetingHud.Instance.state == MeetingHud.VoteStates.Discussion ||
                    IsExempt(voteArea)
                ) return;
                var targetId = voteArea.TargetPlayerId;
                var currentGuess = role.Guesses[targetId];
                if (currentGuess == "None") return;

                var playerRole = Role.GetRole(voteArea);

                var toDie = playerRole.Name == currentGuess ? playerRole.Player : role.Player;

                AssassinKill.RpcMurderPlayer(toDie);
                role.RemainingKills--;
                ShowHideButtons.HideSingle(role, targetId, toDie == role.Player);
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
            foreach (var voteArea in __instance.playerStates)
            {
                GenButton(assassinRole, voteArea);
            }
        }
    }
}
