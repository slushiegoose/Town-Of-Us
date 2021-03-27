using System.Linq;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

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

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CalculateVotes))] //CalculateVotes
        public static class CalculateVotes
        {
            public static void Postfix(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
            {
                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null) return;

                var array = new byte[Mathf.Max(PlayerControl.AllPlayerControls.Count + 1, 11)];
                for (var i = 0; i < array.Length; i++)
                {
                    if (i == SwapVotes.Swap1.TargetPlayerId + 1)
                    {
                        array[SwapVotes.Swap2.TargetPlayerId + 1] = __result[i];
                    }
                    else if (i == SwapVotes.Swap2.TargetPlayerId + 1)
                    {
                        array[SwapVotes.Swap1.TargetPlayerId + 1] = __result[i];
                    }
                    else
                    {
                        array[i] = __result[i];
                    }
                }

                __result = array;


            }
        }



    }
}