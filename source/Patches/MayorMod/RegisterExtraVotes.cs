using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class RegisterExtraVotes
    {
        [HarmonyPatch(nameof(MeetingHud.Confirm))]
        public static bool Prefix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.isMayor()) return true;
            if (__instance.state != MeetingHud.VoteStates.Voted) return true;
            __instance.state = MeetingHud.VoteStates.NotVoted;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(MeetingHud.Confirm))]
        public static void Postfix1(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.isMayor()) return;
            if (AllowExtraVotes.VoteBank > 0)
            {
                __instance.SkipVoteButton.gameObject.SetActive(true);
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

        private static bool SetVote(PlayerVoteArea area, sbyte suspectPlayerId)
        {
            if (area.didVote)
            {
                AllowExtraVotes.ExtraVotes.Add((byte)(suspectPlayerId + 1));
                if (!PlayerControl.LocalPlayer.isMayor())
                {
                    AllowExtraVotes.VoteBank--;
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
            if (!PlayerControl.LocalPlayer.isMayor()) return;
            __instance.TimerText.Text = "Can Vote: " + AllowExtraVotes.VoteBank + " time(s) | " + __instance.TimerText.Text;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch("CCEPEINGBCN")] //CalculateVotes
        public static bool Prefix2(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
        {
            if (Utils.Mayor == null) return true;
            var array = new byte[PlayerControl.AllPlayerControls.Count + 1];
            foreach (var player in __instance.playerStates)
            {
                if (!player.didVote) continue;
                var num = (int) (player.votedFor + 1);
                if (num < 0 || num >= array.Length) continue;
                array[num] += 1;
            }

            foreach (var number in AllowExtraVotes.ExtraVotes)
            {
                array[number] += 1;
            }
           

            __result = array;
            return false;
        }
        
        
        
        [HarmonyPatch(nameof(MeetingHud.CastVote))]
        public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId,
            [HarmonyArgument(1)] sbyte suspectPlayerId)
        {
            if (Utils.Mayor == null || srcPlayerId != Utils.Mayor.PlayerId) return true;
            
            var num = AreaIndexOf(__instance, (sbyte)srcPlayerId);
            var area = __instance.playerStates[num];
            
            if (area.isDead) return false;
            if (PlayerControl.LocalPlayer.PlayerId == srcPlayerId || AmongUsClient.Instance.GameMode != GameModes.LocalGame)
            {
                SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f);
            }

            var isFirstVote = SetVote(area, suspectPlayerId);
            __instance.Cast<InnerNetObject>().Method_8(1U << num);
            __instance.CheckForEndVoting();
            if (isFirstVote)
            {
                PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
            }

            return false;

        }
        
        [HarmonyPrefix]
        [HarmonyPatch("KGIPOIOFBJH")] //VotingComplete
        public static bool Prefix3(MeetingHud __instance)
        {
            if (!AmongUsClient.Instance.AmHost || Utils.Mayor == null) return true;
            
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SetExtraVotes, SendOption.Reliable, -1);
            writer.WriteBytesAndSize(AllowExtraVotes.ExtraVotes.ToArray());
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            return true;
        }

        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void Prefix()
        {
            AllowExtraVotes.ExtraVotes.Clear();
            AllowExtraVotes.VoteBank++;
        }
        
        
        
        private static void Vote(MeetingHud __instance, PlayerVoteArea area2, int num,
           Component origin)
        {
            //System.Console.WriteLine(PlayerControl.GameOptions.AnonymousVotes);
            var playerById = GameData.Instance.GetPlayerById((byte) area2.TargetPlayerId);
            var renderer =
                UnityEngine.Object.Instantiate(__instance.PlayerVotePrefab);
            if (PlayerControl.GameOptions.VisualTasks) //Should be AnonymousVotes but weird
            {
                System.Console.WriteLine("ANONS");
                PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, renderer);
            }
            else
            {
                System.Console.WriteLine("NONANONS");
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

        [HarmonyPatch("FBNKEAEKKJE")] //PopulateResults
        public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte[] statess)
        {
            var joined = string.Join(",", statess);
            var arr = joined.Split(',');
            var states = arr.Select(byte.Parse).ToArray();

            var allnums = new int[__instance.playerStates.Length];
            
            
            if (Utils.Mayor == null) return true;
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
            foreach (var extraVote in AllowExtraVotes.ExtraVotes)
            {
                var area2 = __instance.playerStates.First((PlayerVoteArea pv) =>
                    pv.TargetPlayerId == Utils.Mayor.PlayerId);
                var votedFor = (int)extraVote - 1;
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
            
            return false;
        }
        
    }
}