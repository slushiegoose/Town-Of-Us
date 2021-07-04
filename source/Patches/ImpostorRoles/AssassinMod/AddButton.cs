using System;
using HarmonyLib;
using Reactor.Extensions;
using TMPro;
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
            var player = Utils.PlayerById(voteArea.TargetPlayerId);
            if (
                player == null ||
                player.Data.IsImpostor ||
                player.Data.IsDead ||
                player.Data.Disconnected
            ) return true;
            var role = Role.GetRole(player);
            return role != null && role.Criteria();
        }


        public static void GenButton(Assassin role, PlayerVoteArea voteArea)
        {
            var targetId = voteArea.TargetPlayerId;
            if (IsExempt(voteArea))
            {
                role.Buttons[targetId] = (null, null, null);
                return;
            }

            var confirmButton = voteArea.Buttons.transform.GetChild(0).gameObject;
            var parent = confirmButton.transform.parent.parent;
            
            var nameText = Object.Instantiate(voteArea.NameText, voteArea.transform);
            voteArea.NameText.transform.localPosition = new Vector3(0.55f, 0.12f, -0.1f);
            nameText.transform.localPosition = new Vector3(0.55f, -0.12f, -0.1f);
            nameText.text = "Guess";
            
            var cycle = Object.Instantiate(confirmButton, voteArea.transform);
            var cycleRenderer = cycle.GetComponent<SpriteRenderer>();
            cycleRenderer.sprite = CycleSprite;
            cycle.transform.localPosition = new Vector3(-0.35f, 0.15f, -2f);
            cycle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            cycle.layer = 5;
            cycle.transform.parent = parent;
            var cycleEvent = new Button.ButtonClickedEvent();
            cycleEvent.AddListener(Cycle(role, voteArea, nameText));
            cycle.GetComponent<PassiveButton>().OnClick = cycleEvent;
            var cycleCollider = cycle.GetComponent<BoxCollider2D>();
            cycleCollider.size = cycleRenderer.sprite.bounds.size;
            cycleCollider.offset = Vector2.zero;
            cycle.transform.GetChild(0).gameObject.Destroy();


            var guess = Object.Instantiate(confirmButton, voteArea.transform);
            var guessRenderer = guess.GetComponent<SpriteRenderer>();
            guessRenderer.sprite = GuessSprite;
            guess.transform.localPosition = new Vector3(-0.35f, -0.15f, -2f);
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


            role.Guesses.Add(targetId, -1);
            role.Buttons[targetId] = (cycle, guess, nameText);
        }

        private static Action Cycle(Assassin role, PlayerVoteArea voteArea, TextMeshPro nameText)
        {
            void Listener()
            {
                if (MeetingHud.Instance.state == MeetingHud.VoteStates.Discussion) return;

                var currentGuessIdx = role.Guesses[voteArea.TargetPlayerId];
                if (++currentGuessIdx == role.PossibleGuesses.Count)
                    currentGuessIdx = 0;

                var newGuess = role.PossibleGuesses[role.Guesses[voteArea.TargetPlayerId] = currentGuessIdx];

                nameText.text = Role.GetName(newGuess, true);
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
                var currentGuessIdx = role.Guesses[targetId];
                if (currentGuessIdx == -1) return;

                var currentGuess = role.PossibleGuesses[currentGuessIdx];
                var playerRole = Role.GetRole(voteArea);

                var toDie = playerRole.RoleType == currentGuess ? playerRole.Player : role.Player;

                AssassinKill.RpcMurderPlayer(toDie);
                role.RemainingKills--;
                ShowHideButtons.HideSingle(role, targetId, toDie == role.Player);
                if (toDie.isLover() && CustomGameOptions.BothLoversDie)
                {
                    var lover = ((Lover)playerRole).OtherLover.Player;
                    ShowHideButtons.HideSingle(role, lover.PlayerId, false);
                }
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
