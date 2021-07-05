using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using System.Collections.Generic;
using TownOfUs.Roles.Modifiers;
using TownOfUs.CrewmateRoles.SwapperMod;
using UnityEngine;
using System;
using UnhollowerBaseLib;
using System.Linq;
using TownOfUs.Extensions;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public static class RegisterExtraVotes
    {
        public static bool IsMayor => MayorRole != null;
        public static Mayor MayorRole
        {
            get
            {
                var role = Role.GetRole(PlayerControl.LocalPlayer);
                if (role?.RoleType != RoleEnum.Mayor) return null;
                return (Mayor)role;
            }
        }

        private static bool SetVote(PlayerVoteArea area, byte suspectPlayerId, Mayor role)
        {
            if (area.DidVote)
            {
                role.ExtraVotes.Add(suspectPlayerId);
                if (!IsMayor) role.VoteBank--;
                return false;
            }

            area.SetVote(suspectPlayerId);
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MeetingHud.Confirm))]
        public static void ConfirmVote(MeetingHud __instance)
        {
            if (!IsMayor || __instance.state != MeetingHud.VoteStates.Voted) return;
            __instance.state = MeetingHud.VoteStates.NotVoted;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(MeetingHud.Confirm))]
        public static void AllowSkip(MeetingHud __instance)
        {
            if (!IsMayor) return;
            var role = MayorRole;
            if (role.CanVote)
            {
                __instance.SkipVoteButton.gameObject.SetActive(true);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(
            nameof(MeetingHud.HandleDisconnect),
            typeof(PlayerControl), typeof(InnerNet.DisconnectReasons)
        )]
        public static void HandleDisconnect(MeetingHud __instance, [HarmonyArgument(0)] PlayerControl player)
        {
            if (!AmongUsClient.Instance.AmHost) return;
            var mayor = Role.GetRole<Mayor>();
            var votesRegained = mayor.ExtraVotes.RemoveAll(x => x == player.PlayerId);
                        
            if (mayor.Player == PlayerControl.LocalPlayer)
                mayor.VoteBank += votesRegained;
                        
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.AddMayorVoteBank, SendOption.Reliable, -1);
            writer.Write(mayor.Player.PlayerId);
            writer.Write(mayor.VoteBank);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }


        public static byte GetTargetId(byte targetId) {
            var s1 = SwapVotes.Swap1?.TargetPlayerId ?? byte.MaxValue;
            var s2 = SwapVotes.Swap2?.TargetPlayerId ?? byte.MaxValue;

            if (s1 != byte.MaxValue && s2 != byte.MaxValue)
            {
                if (targetId == s1) return s2;
                if (targetId == s2) return s1;
            }

            return targetId;
        }

        private static bool IsRealVote(byte targetId) =>
            targetId != PlayerVoteArea.DeadVote &&
            targetId != PlayerVoteArea.MissedVote &&
            targetId != PlayerVoteArea.HasNotVoted;

        private static MeetingHud.VoterState[] CalculateVotes(MeetingHud __instance)
        {
            var states = new List<MeetingHud.VoterState>();

            foreach (var voteArea in __instance.playerStates)
                states.Add(new MeetingHud.VoterState()
                {
                    VotedForId = GetTargetId(voteArea.VotedFor),
                    VoterId = voteArea.TargetPlayerId
                });

            var mayor = Role.GetRole<Mayor>();
            if (mayor != null)
                foreach (var targetId in mayor.ExtraVotes)
                {
                    states.Add(new MeetingHud.VoterState()
                    {
                        VotedForId = GetTargetId(targetId),
                        VoterId = byte.MaxValue
                    });
                }

            return states.Where(state =>
            {
                var voter = Utils.PlayerById(state.VoterId);
                if (voter != null && voter.Data.IsDead) return false;
                var target = Utils.PlayerById(state.VotedForId);
                if (target != null && target.Data.IsDead) return false;
                return true;
            }).ToArray();
        }

        private static (bool, GameData.PlayerInfo) CalculateVoteResults(MeetingHud.VoterState[] states)
        {
            var votes = new Dictionary<byte, int>();
            foreach (var state in states)
            {
                var targetId = state.VotedForId;
                if (!IsRealVote(targetId)) continue;
                if (votes.TryGetValue(targetId, out var totalVotes))
                    votes[targetId] = totalVotes + 1;
                else
                    votes[targetId] = 1;
            }

            var (exiledId, maxVotes) = votes.MaxPair(out var tie);

            if (tie)
            {
                var possiblePlayers = votes.Where(kvp => kvp.Value == maxVotes).Select(x => x.Key).ToList();
                foreach (var state in states)
                {
                    if (state.VoterId == byte.MaxValue || !possiblePlayers.Contains(state.VotedForId)) continue;
                    var player = Utils.PlayerById(state.VoterId);
                    if (player == null || !player.Is(ModifierEnum.Tiebreaker)) continue;
                    return (false, GameData.Instance.GetPlayerById(state.VotedForId));
                }
                return (true, null);
            }

            return (
                false,
                GameData.Instance.GetPlayerById(exiledId)
            );
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MeetingHud.CheckForEndVoting))]
        public static bool CheckForEndVoting(MeetingHud __instance)
        {
            if (__instance.playerStates.All((PlayerVoteArea ps) => ps.AmDead || ps.DidVote))
            {
                var states = CalculateVotes(__instance);
                var (tie, exiled) = CalculateVoteResults(states);

                __instance.RpcVotingComplete(states, exiled, tie);

                SendMayorVotes();
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MeetingHud.PopulateResults))]
        public static bool PopulateResults(
            MeetingHud __instance,
            [HarmonyArgument(0)] Il2CppStructArray<MeetingHud.VoterState> voteStates
        )
        {
            __instance.TitleText.text = TranslationController.Instance.GetString(
                StringNames.MeetingVotingResults,
                Array.Empty<Il2CppSystem.Object>()
            );

            var skippedVoting = 0;
            for (var i = 0;i < __instance.playerStates.Length;i++)
            {
                var voteArea = __instance.playerStates[i];
                // bloops the vote icons on the incorrect player
                // but the swapper swaps the vote boxes so it looks like the correct player
                var targetId = GetTargetId(voteArea.TargetPlayerId);
                voteArea.ClearForResults();
                var num2 = 0;
                var mayor = Role.GetRole<Mayor>()?.Player.Data;
                foreach (MeetingHud.VoterState voterState in voteStates)
                {
                    var playerById = GameData.Instance.GetPlayerById(voterState.VoterId);
                    if (i == 0 && voterState.SkippedVote)
                    {
                        __instance.BloopAVoteIcon(playerById, skippedVoting++, __instance.SkippedVoting.transform);
                    }
                    else if (voterState.VotedForId == targetId)
                    {
                        BloopAVoteIcon(
                            __instance,
                            playerById,
                            num2++,
                            voteArea.transform,
                            mayor
                        );
                    }
                }
            }
            return false;
        }

        public static void BloopAVoteIcon(
            MeetingHud __instance,
            GameData.PlayerInfo voterPlayer,
            int index,
            Transform parent,
            GameData.PlayerInfo mayor
        )
        {
            var spriteRenderer = UnityEngine.Object.Instantiate(__instance.PlayerVotePrefab);

            if ((voterPlayer == null && CustomGameOptions.MayorAnonymous) || PlayerControl.GameOptions.AnonymousVotes)
                PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, spriteRenderer);
            else
                PlayerControl.SetPlayerMaterialColors((voterPlayer ?? mayor).ColorId, spriteRenderer);

            spriteRenderer.transform.SetParent(parent);
            spriteRenderer.transform.localScale = Vector3.zero;
            __instance.StartCoroutine(Effects.Bloop(index * 0.3f, spriteRenderer.transform, 1f, 0.5f));
            parent.GetComponent<VoteSpreader>().AddVote(spriteRenderer);
        }

        public static void SendMayorVotes()
        {
            var role = Role.GetRole<Mayor>();
            if (role == null) return;
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SetExtraVotes, SendOption.Reliable, -1);
            writer.WriteBytesAndSize(role.ExtraVotes.ToArray());
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MeetingHud.CastVote))]
        public static bool CastVote(
            MeetingHud __instance,
            [HarmonyArgument(0)] byte srcPlayerId,
            [HarmonyArgument(1)] byte targetPlayerId
        )
        {
            var voteArea = __instance.playerStates.FirstOrDefault(pv => pv.TargetPlayerId == srcPlayerId);
            var role = Role.GetRole(voteArea);
            if (role?.RoleType != RoleEnum.Mayor) return true;
            if (voteArea.AmDead) return false;
            if (PlayerControl.LocalPlayer.PlayerId == srcPlayerId)
                SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f);

            if (SetVote(voteArea, targetPlayerId, (Mayor)role))
                PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
            __instance.SetDirtyBit(1U);
            __instance.CheckForEndVoting();
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void ResetVotes()
        {
            var role = Role.GetRole<Mayor>();
            if (role == null) return;
            role.ExtraVotes.Clear();
            role.VoteBank++;
            role.SelfVote = role.VotedOnce = false;
        }
    }
}
