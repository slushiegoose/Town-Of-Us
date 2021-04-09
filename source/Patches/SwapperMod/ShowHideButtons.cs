using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using Reactor.Extensions;

namespace TownOfUs.SwapperMod
{
    public class ShowHideButtons
    {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) return true;
                var swapper = Roles.Role.GetRole<Roles.Swapper>(PlayerControl.LocalPlayer);
                foreach (var button in swapper.Buttons.Where(button => button != null))
                {
                    if (button.GetComponent<SpriteRenderer>().sprite == AddButton.DisabledSprite)
                    {
                        button.SetActive(false);
                    }

                    button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                }

                if (swapper.ListOfActives.Count(x => x) == 2)
                {
                    var toSet1 = true;
                    for (var i = 0; i < swapper.ListOfActives.Count; i++)
                    {
                        if (!swapper.ListOfActives[i]) continue;

                        if (toSet1)
                        {
                            SwapVotes.Swap1 = __instance.playerStates[i];
                            toSet1 = false;
                        }
                        else
                        {
                            SwapVotes.Swap2 = __instance.playerStates[i];
                        }
                    }
                }


                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null) return true;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetSwaps, SendOption.Reliable, -1);
                writer.Write(SwapVotes.Swap1.TargetPlayerId);
                writer.Write(SwapVotes.Swap2.TargetPlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                return true;

            }
        }

        public static byte[] CalculateVotes(MeetingHud __instance)
        {
            var self = MayorMod.RegisterExtraVotes.CalculateAllVotes(__instance);
            var array = new byte[Mathf.Max(PlayerControl.AllPlayerControls.Count + 1, 11)];
            for (var i = 0; i < array.Length; i++)
            {
                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null)
                {
                    array[i] = self[i];
                    continue;
                }
                    
                if (i == SwapVotes.Swap1.TargetPlayerId + 1)
                {
                    array[SwapVotes.Swap2.TargetPlayerId + 1] = self[i];
                }
                else if (i == SwapVotes.Swap2.TargetPlayerId + 1)
                {
                    array[SwapVotes.Swap1.TargetPlayerId + 1] = self[i];
                }
                else
                {
                    array[i] = self[i];
                }
            }

            return array;

        }

        public static void RpcVotingComplete(MeetingHud __instance, byte[] states, GameData.PlayerInfo exiled, bool tie)
        {
            if (AmongUsClient.Instance.AmClient)
            {
                __instance.VotingComplete(states, exiled, tie);
            }
            var messageWriter = AmongUsClient.Instance.StartRpc(__instance.NetId, 23, SendOption.Reliable);
            messageWriter.WriteBytesAndSize(states);
            messageWriter.Write(exiled?.PlayerId ?? byte.MaxValue);
            messageWriter.Write(tie);
            messageWriter.EndMessage();
        }
        

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
        public static class CheckForEndVoting
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (__instance.playerStates.All((ps) => ps.isDead || ps.didVote))
                {
                    var self = CalculateVotes(__instance);
                    Il2CppStructArray<byte> selfIl2 = self;
                    bool tie;


                    var maxIdx = Extensions.IndexOfMax(selfIl2, (Func<byte, int>) ((p) => (int) p), out tie) - 1;
                    var exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => (int)v.PlayerId == maxIdx);
                    var array = new byte[10];
                    foreach (var playerVoteArea in __instance.playerStates)
                    {
                        array[(int)playerVoteArea.TargetPlayerId] = playerVoteArea.GetState();
                    }
                    RpcVotingComplete(__instance, array, exiled, tie);
                }

                return false;
            }
        }
        



    }
}