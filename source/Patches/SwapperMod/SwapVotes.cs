using System.Collections;
using System.Linq;
using HarmonyLib;
using Reactor;
using UnityEngine;

namespace TownOfUs.SwapperMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class SwapVotes
    {
        public static PlayerVoteArea Swap1;
        public static PlayerVoteArea Swap2;

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))] // BBFDNCCEJHI
        public static class BBFDNCCEJHI
        {
            public static void Postfix(MeetingHud __instance)
            {
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage(Swap1 == null ? "null" : Swap1.ToString());
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage(Swap2 == null ? "null" : Swap2.ToString());

                if (!(Swap1 != null & Swap2 != null)) return;

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper))
                {
                    var swapper = Roles.Role.GetRole<Roles.Swapper>(PlayerControl.LocalPlayer);
                    foreach (var button in swapper.Buttons.Where(button => button != null))
                    {
                        button.SetActive(false);
                    }
                }

                SwapPositions(Swap1.PlayerIcon.transform, Swap2.PlayerIcon.transform);
                SwapPositions(Swap1.NameText.transform, Swap2.NameText.transform);
                SwapPositions(Swap1.transform.GetChild(4).GetChild(0), Swap2.transform.GetChild(4).GetChild(0));
            }
        }

        private static void SwapPositions(Transform a, Transform b, float duration = 5f)
        {
            Coroutines.Start(Slide2D(a, b.position, duration));
            Coroutines.Start(Slide2D(b, a.position, duration));
        }

        private static IEnumerator Slide2D(Transform target, Vector3 dest, float duration = 5f)
        {
            var currentPos = new Vector3(target.position.x, target.position.y, target.position.z);

            for (var time = 0f; time < duration; time += Time.deltaTime)
            {
                var t = time / duration;
                currentPos.x = Mathf.SmoothStep(currentPos.x, dest.x, t);
                currentPos.y = Mathf.SmoothStep(currentPos.y, dest.y, t);
                target.position = currentPos;
                yield return null;
            }

            target.position = dest;
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static class MeetingHud_Start
        {
            public static void Postfix(MeetingHud __instance)
            {

                Swap1 = null;
                Swap2 = null;

            }
        }
    }
}