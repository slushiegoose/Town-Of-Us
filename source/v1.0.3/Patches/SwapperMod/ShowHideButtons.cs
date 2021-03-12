using System.Linq;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class ShowHideButtons
    {
        [HarmonyPatch(nameof(MeetingHud.Confirm))]
        public static bool Prefix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.isSwapper()) return true;
            foreach (var button in AddButton.Buttons.Where(button => button != null))
            {
                if (button.GetComponent<SpriteRenderer>().sprite == AddButton.DisabledSprite)
                {
                    button.SetActive(false);
                }

                button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
            }
            
            if (AddButton.ListOfActives.Count(x => x) == 2)
            {
                var toSet1 = true;
                for (var i = 0; i < AddButton.ListOfActives.Count; i++)
                {
                    if (!AddButton.ListOfActives[i]) continue;
                    
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

        [HarmonyPostfix]
        [HarmonyPatch("CCEPEINGBCN")] //CalculateVotes
        public static void Postfix2(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
        {
            if (Utils.Swapper == null) return;
            if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null) return;

            var array = new byte[PlayerControl.AllPlayerControls.Count + 1];
            for(var i=0; i<array.Length; i++)
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