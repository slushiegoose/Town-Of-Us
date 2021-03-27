using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using InnerNet;
using TownOfUs.Roles;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class RegisterExtraVotes
    {
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
            
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var role = Roles.Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (role.VoteBank > 0)
                {
                    __instance.SkipVoteButton.gameObject.SetActive(true);
                }
            }
        }


        private static int AreaIndexOf(MeetingHud __instance, sbyte srcPlayerId)
        {
            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                if (__instance.playerStates[i].TargetPlayerId == srcPlayerId)
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool SetVote(PlayerVoteArea area, sbyte suspectPlayerId, Mayor role)
        {
            if (area.didVote)
            {
                role.ExtraVotes.Add((byte)(suspectPlayerId + 1));
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor))
                {
                    role.VoteBank--;
                }
                return false;
            }
            area.didVote = true;
            area.votedFor = suspectPlayerId;
            area.Flag.enabled = true;
            return true;

        }

        [HarmonyPatch(nameof(MeetingHud.Update))]
        public static void Postfix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
            var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
            __instance.TimerText.Text = "Can Vote: " + role.VoteBank + " time(s) | " + __instance.TimerText.Text;
        }
        
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CalculateVotes))] //CalculateVotes
        public static class CalculateVotes
        {
            public static bool Prefix(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
            {
                var array = new byte[Mathf.Max(PlayerControl.AllPlayerControls.Count + 1, 11)];
                foreach (var player in __instance.playerStates)
                {
                    if (!player.didVote) continue;
                    var num = (int) (player.votedFor + 1);
                    if (num < 0 || num >= array.Length) continue;
                    array[num] += 1;
                }

                foreach (var role in Role.GetRoles(RoleEnum.Mayor))
                {
                    foreach (var number in ((Mayor) role).ExtraVotes)
                    {
                        array[number] += 1;
                    }
                }



                __result = array;
                return false;
            }
        }



        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        public static class CastVote
        {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId,
                [HarmonyArgument(1)] sbyte suspectPlayerId)
            {

                var player = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == srcPlayerId);
                if (!player.Is(RoleEnum.Mayor)) return true;

                var role = Role.GetRole<Mayor>(player);

                var num = AreaIndexOf(__instance, (sbyte) srcPlayerId);
                var area = __instance.playerStates[num];

                if (area.isDead) return false;
                if (PlayerControl.LocalPlayer.PlayerId == srcPlayerId ||
                    AmongUsClient.Instance.GameMode != GameModes.LocalGame)
                {
                    SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f);
                }

                var isFirstVote = SetVote(area, suspectPlayerId, role);
                __instance.Cast<InnerNetObject>().SetDirtyBit(1U << num);
                __instance.CheckForEndVoting();
                if (isFirstVote)
                {
                    PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
                }

                return false;

            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public static class VotingComplete
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (!AmongUsClient.Instance.AmHost) return true;
                foreach (var role in Role.GetRoles(RoleEnum.Mayor))
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.SetExtraVotes, SendOption.Reliable, -1);
                    writer.Write(role.Player.PlayerId);
                    writer.WriteBytesAndSize(((Mayor) role).ExtraVotes.ToArray());
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }

                return true;
            }
        }

        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void Prefix()
        {
            foreach (var role in Role.GetRoles(RoleEnum.Mayor))
            {
                var mayor = (Mayor) role;
                mayor.ExtraVotes.Clear();
                mayor.VoteBank++;
            }
            
        }
        
        
        
        private static void Vote(MeetingHud __instance, PlayerVoteArea area2, int num,
           Component origin)
        {
            ////System.Console.WriteLine(PlayerControl.GameOptions.AnonymousVotes);
            var playerById = GameData.Instance.GetPlayerById((byte) area2.TargetPlayerId);
            var renderer =
                UnityEngine.Object.Instantiate(__instance.PlayerVotePrefab);
            if (PlayerControl.GameOptions.AnonymousVotes) //Should be AnonymousVotes but weird
            {
                //System.Console.WriteLine("ANONS");
                PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, renderer);
            }
            else
            {
                //System.Console.WriteLine("NONANONS");
                PlayerControl.SetPlayerMaterialColors(playerById.ColorId, renderer);
            }

            var transform = renderer.transform;
            transform.SetParent(origin.transform);
            transform.localPosition = __instance.CounterOrigin +
                                      new Vector3(__instance.CounterOffsets.x * num,
                                          0f, 0f);
            transform.localScale = Vector3.zero;
            __instance.StartCoroutine(Effects.Bloop(num * 0.5f, transform, 1f, 0.5f));
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        public static class PopulateResults {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte[] statess)
            {
                var joined = string.Join(",", statess);
                var arr = joined.Split(',');
                var states = arr.Select(byte.Parse).ToArray();

                var allnums = new int[__instance.playerStates.Length];


                __instance.TitleText.Text = UnityEngine.Object.FindObjectOfType<TranslationController>()
                    .GetString(StringNames.MeetingVotingResults, Array.Empty<Il2CppSystem.Object>());
                var num = 0;
                for (var i = 0; i < __instance.playerStates.Length; i++)
                {
                    var area = __instance.playerStates[i];
                    area.ClearForResults();
                    var num2 = 0;
                    for (var j = 0; j < __instance.playerStates.Length; j++)
                    {
                        var area2 = __instance.playerStates[j];
                        var self = states[(int) area2.TargetPlayerId];
                        if ((self & 128) > 0) continue;
                        var votedFor = (int) PlayerVoteArea.GetVotedFor(self);
                        if (votedFor == area.TargetPlayerId)
                        {
                            Vote(__instance, area2, num2, area);
                            num2++;
                        }
                        else if (i == 0 && votedFor == -1)
                        {
                            Vote(__instance, area2, num, __instance.SkippedVoting);
                            num++;
                        }
                    }

                    allnums[i] = num2;
                }

                foreach (var role in Role.GetRoles(RoleEnum.Mayor))
                {
                    var mayor = (Mayor) role;
                    foreach (var extraVote in mayor.ExtraVotes)
                    {
                        var area2 = __instance.playerStates.First(pv => pv.TargetPlayerId == role.Player.PlayerId);
                        var votedFor = (int) extraVote - 1;
                        if (votedFor == -1)
                        {
                            Vote(__instance, area2, num, __instance.SkippedVoting);
                            num++;
                        }
                        else
                        {
                            for (var i = 0; i < __instance.playerStates.Length; i++)
                            {
                                var area = __instance.playerStates[i];
                                if (votedFor != area.TargetPlayerId) continue;
                                Vote(__instance, area2, allnums[i], area);
                                allnums[i]++;
                            }
                        }

                    }

                }

                return false;
            }
        }

    }
}