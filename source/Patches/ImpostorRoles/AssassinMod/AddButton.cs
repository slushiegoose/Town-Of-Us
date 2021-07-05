using System;
using HarmonyLib;
using Reactor.Extensions;
using TMPro;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.Events;

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

        private static GameObject CreateButton(
            PlayerVoteArea voteArea,
            Sprite sprite,
            float yOffset,
            Action onClick)
        {
            var confirmButton = voteArea.Buttons.transform.GetChild(0).gameObject;
            var parent = confirmButton.transform.parent.parent;

            var gameObject = Object.Instantiate(confirmButton, voteArea.transform);
            var renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            gameObject.transform.position = confirmButton.transform.position - new Vector3(0.7f, yOffset, 0f);
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            gameObject.layer = 5;
            gameObject.transform.parent = parent;
            var clickEvent = new Button.ButtonClickedEvent();
            clickEvent.AddListener(onClick);
            clickEvent.AddListener((UnityAction)(() =>
            {
                voteArea.Buttons.SetActive(false);
            }));
            var button = gameObject.GetComponent<PassiveButton>();
            button.OnClick = clickEvent;

            var bounds = renderer.bounds;
            bounds.size = new Vector3(0.52f, 0.3f, 0.16f);

            var collider = gameObject.GetComponent<BoxCollider2D>();
            collider.size = renderer.sprite.bounds.size;
            collider.offset = Vector2.zero;
            gameObject.transform.GetChild(0).gameObject.Destroy();

            return gameObject;
        }

        public static void GenButton(Assassin role, PlayerVoteArea voteArea)
        {
            var targetId = voteArea.TargetPlayerId;
            if (IsExempt(voteArea))
            {
                role.Buttons[targetId] = (null, null, null);
                return;
            }
            
            var nameText = Object.Instantiate(voteArea.NameText, voteArea.transform);
            voteArea.NameText.transform.localPosition = new Vector3(0.55f, 0.12f, -0.1f);
            nameText.transform.localPosition = new Vector3(0.55f, -0.12f, -0.1f);
            nameText.text = "Guess";

            var cycle = CreateButton(voteArea, CycleSprite, -0.15f, Cycle(role, voteArea, nameText));
            var guess = CreateButton(voteArea, GuessSprite, 0.15f, Guess(role, voteArea));


            role.Guesses.Add(targetId, "None");
            role.Buttons[targetId] = (cycle, guess, nameText);
        }

        private static Action Cycle(Assassin role, PlayerVoteArea voteArea, TextMeshPro nameText)
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

                var roleName = role.Guesses[voteArea.TargetPlayerId] = role.PossibleGuesses[guessIndex];
                nameText.text = Utils.ColorText(role.ColorMapping[roleName], $"{roleName}??");
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
                if (toDie.IsLover() && CustomGameOptions.BothLoversDie)
                {
                    var lover = ((Lover)playerRole).OtherLover.Player;
                    ShowHideButtons.HideSingle(role, lover.PlayerId, false);
                }
            }

            return Listener;
        }

        [HarmonyPriority(Priority.Last)]
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
