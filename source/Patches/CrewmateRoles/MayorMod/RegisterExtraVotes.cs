using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using InnerNet;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.CrewmateRoles.MayorMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class RegisterExtraVotes
    {
        [HarmonyPatch(nameof(MeetingHud.Update))]
        public static void Postfix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (__instance.TimerText.text.Contains("Can Vote")) return;
            var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
            __instance.TimerText.text = "Can Vote: " + role.VoteBank + " time(s) | " + __instance.TimerText.text;
        }

        public static Dictionary<byte, int> CalculateAllVotes(MeetingHud __instance)
        {
            var dictionary = new Dictionary<byte, int>();
            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                var playerVoteArea = __instance.playerStates[i];
                if (!playerVoteArea.DidVote
                    || playerVoteArea.AmDead
                    || playerVoteArea.VotedFor == PlayerVoteArea.MissedVote
                    || playerVoteArea.VotedFor == PlayerVoteArea.DeadVote) continue;

                if (dictionary.TryGetValue(playerVoteArea.VotedFor, out var num))
                    dictionary[playerVoteArea.VotedFor] = num + 1;
                else
                    dictionary[playerVoteArea.VotedFor] = 1;
            }

            foreach (var role in Role.GetRoles(RoleEnum.Mayor))
            foreach (var number in ((Mayor) role).ExtraVotes)
                if (dictionary.TryGetValue(number, out var num))
                    dictionary[number] = num + 1;
                else
                    dictionary[number] = 1;

            dictionary.MaxPair(out var tie);

            if (tie)
                foreach (var player in __instance.playerStates)
                {
                    if (!player.DidVote
                        || player.AmDead
                        || player.VotedFor == PlayerVoteArea.MissedVote
                        || player.VotedFor == PlayerVoteArea.DeadVote) continue;

                    var modifier = Modifier.GetModifier(player);
                    if (modifier == null) continue;
                    if (modifier.ModifierType == ModifierEnum.Tiebreaker)
                    {
                        if (dictionary.TryGetValue(player.VotedFor, out var num))
                            dictionary[player.VotedFor] = num + 1;
                        else
                            dictionary[player.VotedFor] = 1;
                    }
                }

            return dictionary;
        }

        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void Prefix()
        {
            foreach (var role in Role.GetRoles(RoleEnum.Mayor))
            {
                var mayor = (Mayor) role;
                mayor.ExtraVotes.Clear();
                if (mayor.VoteBank < 0)
                    mayor.VoteBank = 0;

                mayor.VoteBank++;
                mayor.SelfVote = false;
                mayor.VotedOnce = false;
            }
        }

        private static void Vote(MeetingHud __instance, GameData.PlayerInfo votingPlayer, int amountOfVotes,
            Component origin, bool isMayor = false)
        {
            ////System.Console.WriteLine(PlayerControl.GameOptions.AnonymousVotes);
            var renderer =
                Object.Instantiate(__instance.PlayerVotePrefab);
            if (PlayerControl.GameOptions.AnonymousVotes || CustomGameOptions.MayorAnonymous && isMayor
                ) //Should be AnonymousVotes but weird
                //System.Console.WriteLine("ANONS");
                PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, renderer);
            else
                //System.Console.WriteLine("NONANONS");
                PlayerControl.SetPlayerMaterialColors(votingPlayer.ColorId, renderer);


            renderer.transform.SetParent(origin.transform);
            renderer.transform.localPosition = __instance.VoteOrigin +
                                               new Vector3(__instance.VoteButtonOffsets.x * amountOfVotes,
                                                   0f, 0f);
            renderer.transform.localScale = Vector3.zero;
            __instance.StartCoroutine(Effects.Bloop(amountOfVotes * 0.3f, renderer.transform, 1f, 0.5f));
            origin.GetComponent<VoteSpreader>().AddVote(renderer);
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return true;
                if (__instance.state != MeetingHud.VoteStates.Voted) return true;
                __instance.state = MeetingHud.VoteStates.NotVoted;
                return true;
            }

            [HarmonyPriority(Priority.First)]
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (role.CanVote) __instance.SkipVoteButton.gameObject.SetActive(true);
            }
        }


        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        public static class CastVote
        {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId,
                [HarmonyArgument(1)] byte suspectPlayerId)
            {
                var player = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == srcPlayerId);
                if (!player.Is(RoleEnum.Mayor)) return true;
                
                var playerVoteArea = __instance.playerStates.ToArray().First(pv => pv.TargetPlayerId == srcPlayerId);
                
                if (playerVoteArea.AmDead)
                    return false;
                
                if (PlayerControl.LocalPlayer.PlayerId == srcPlayerId || AmongUsClient.Instance.GameMode != GameModes.LocalGame)
                {
                    SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f);
                }
                
                var role = Role.GetRole<Mayor>(player);
                if (playerVoteArea.DidVote)
                {
                    role.ExtraVotes.Add(suspectPlayerId);
                    role.VoteBank--;
                }
                else
                {
                    playerVoteArea.SetVote(suspectPlayerId);
                    playerVoteArea.Flag.enabled = true;
                    PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
                }
                __instance.Cast<InnerNetObject>().SetDirtyBit(1U);
                __instance.CheckForEndVoting();

                return false;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public static class VotingComplete
        {
            public static void Postfix(MeetingHud __instance,
                [HarmonyArgument(0)] Il2CppStructArray<MeetingHud.VoterState> states,
                [HarmonyArgument(1)] GameData.PlayerInfo exiled,
                [HarmonyArgument(2)] bool tie)
            {
                // __instance.exiledPlayer = __instance.wasTie ? null : __instance.exiledPlayer;
                var exiledString = exiled == null ? "null" : exiled.PlayerName;
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Exiled PlayerName = {exiledString}");
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Was a tie = {tie}");
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        public static class PopulateResults
        {
            public static bool Prefix(MeetingHud __instance,
                [HarmonyArgument(0)] Il2CppStructArray<MeetingHud.VoterState> statess)
            {
                // var joined = string.Join(",", statess);
                // var arr = joined.Split(',');
                // var states = arr.Select(byte.Parse).ToArray();

                // var allnums = new int[__instance.playerStates.Length];

                var allNums = new Dictionary<int, int>();


                __instance.TitleText.text = Object.FindObjectOfType<TranslationController>()
                    .GetString(StringNames.MeetingVotingResults, Array.Empty<Il2CppSystem.Object>());
                var amountOfSkippedVoters = 0;
                for (var i = 0; i < __instance.playerStates.Length; i++)
                {
                    var playerVoteArea = __instance.playerStates[i];
                    playerVoteArea.ClearForResults();
                    allNums.Add(i, 0);

                    for (var stateIdx = 0; stateIdx < statess.Length; stateIdx++)
                    {
                        var voteState = statess[stateIdx];
                        var playerInfo = GameData.Instance.GetPlayerById(voteState.VoterId);
                        if (playerInfo == null)
                        {
                            Debug.LogError(string.Format("Couldn't find player info for voter: {0}",
                                voteState.VoterId));
                        }
                        else if (i == 0 && voteState.SkippedVote)
                        {
                            __instance.BloopAVoteIcon(playerInfo, amountOfSkippedVoters, __instance.SkippedVoting.transform);
                            amountOfSkippedVoters++;
                        }
                        else if (voteState.VotedForId == playerVoteArea.TargetPlayerId)
                        {
                            Vote(__instance, playerInfo, allNums[i], playerVoteArea);
                            allNums[i]++;
                        }
                    }
                }

                foreach (var role in Role.GetRoles(RoleEnum.Mayor))
                {
                    var mayor = (Mayor) role;
                    var playerInfo = GameData.Instance.GetPlayerById(role.Player.PlayerId);
                    foreach (var extraVote in mayor.ExtraVotes)
                    {
                        if (extraVote == PlayerVoteArea.HasNotVoted ||
                            extraVote == PlayerVoteArea.MissedVote || 
                            extraVote == PlayerVoteArea.DeadVote)
                        {
                            continue;
                        }
                        if (extraVote == PlayerVoteArea.SkippedVote)
                        {
                            Vote(__instance, playerInfo, amountOfSkippedVoters, __instance.SkippedVoting, true);
                            amountOfSkippedVoters++;
                        }
                        else
                        {
                            for (var i = 0; i < __instance.playerStates.Length; i++)
                            {
                                var area = __instance.playerStates[i];
                                if (extraVote != area.TargetPlayerId) continue;
                                Vote(__instance, playerInfo, allNums[i], area, true);
                                allNums[i]++;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}