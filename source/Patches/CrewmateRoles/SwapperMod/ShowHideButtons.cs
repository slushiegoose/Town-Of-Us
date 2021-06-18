using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.CrewmateRoles.MayorMod;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace TownOfUs.CrewmateRoles.SwapperMod
{
    public class ShowHideButtons
    {
        public static Dictionary<byte, int> CalculateVotes(MeetingHud __instance)
        {
            var self = RegisterExtraVotes.CalculateAllVotes(__instance);
            if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null) return self;
            //

            PluginSingleton<TownOfUs>.Instance.Log.LogInfo($"Swap1 playerid = {SwapVotes.Swap1.TargetPlayerId}");
            var swap1 = 0;
            if (self.TryGetValue(SwapVotes.Swap1.TargetPlayerId, out var value)) swap1 = value;
            PluginSingleton<TownOfUs>.Instance.Log.LogInfo($"Swap1 player has votes = {swap1}");

            var swap2 = 0;
            PluginSingleton<TownOfUs>.Instance.Log.LogInfo($"Swap2 playerid = {SwapVotes.Swap2.TargetPlayerId}");
            if (self.TryGetValue(SwapVotes.Swap2.TargetPlayerId, out var value2)) swap2 = value2;
            PluginSingleton<TownOfUs>.Instance.Log.LogInfo($"Swap2 player has votes  = {swap2}");

            self[SwapVotes.Swap2.TargetPlayerId] = swap1;
            self[SwapVotes.Swap1.TargetPlayerId] = swap2;

            return self;
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Swapper)) return true;
                var swapper = Role.GetRole<Swapper>(PlayerControl.LocalPlayer);
                foreach (var button in swapper.Buttons.Where(button => button != null))
                {
                    if (button.GetComponent<SpriteRenderer>().sprite == AddButton.DisabledSprite)
                        button.SetActive(false);

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

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
        public static class CheckForEndVoting
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (__instance.playerStates.All(ps => ps.AmDead || ps.DidVote))
                {
                    var self = CalculateVotes(__instance);

                    var array = new Il2CppStructArray<MeetingHud.VoterState>(__instance.playerStates.Length);

                    var maxIdx = self.MaxPair(out var tie);

                    PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Meeting was a tie = {tie}");
                    var exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => v.PlayerId == maxIdx.Key);
                    for (var i = 0; i < __instance.playerStates.Length; i++)
                    {
                        var playerVoteArea = __instance.playerStates[i];
                        array[i] = new MeetingHud.VoterState
                        {
                            VoterId = playerVoteArea.TargetPlayerId,
                            VotedForId = playerVoteArea.VotedFor
                        };
                    }

                    __instance.exiledPlayer = tie ? null : exiled;
                    __instance.wasTie = tie;
                    __instance.RpcVotingComplete(array, __instance.exiledPlayer, tie);
                }

                return false;
            }
        }
    }
}