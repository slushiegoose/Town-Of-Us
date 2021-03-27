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

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))] // VotingComplete
        public static class VotingComplete
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

                var pool1 = Swap1.PlayerIcon.transform;
                var name1 = Swap1.NameText.transform;
                var mask1 = Swap1.transform.GetChild(4).GetChild(0);
                var pooldest1 = (Vector2) pool1.position;
                var namedest1 = (Vector2) name1.position;
                var maskdest1 = (Vector2) mask1.position;
                mask1.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                var pool2 = Swap2.PlayerIcon.transform;
                var name2 = Swap2.NameText.transform;
                var mask2 = Swap2.transform.GetChild(4).GetChild(0);
                var pooldest2 = (Vector2) pool2.position;
                var namedest2 = (Vector2) name2.position;
                var maskdest2 = (Vector2) mask2.position;
                mask2.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);


                Reactor.Coroutines.Start(Slide2D(pool1, pooldest1, pooldest2, 2f));
                Reactor.Coroutines.Start(Slide2D(pool2, pooldest2, pooldest1, 2f));
                Reactor.Coroutines.Start(Slide2D(name1, namedest1, namedest2, 2f));
                Reactor.Coroutines.Start(Slide2D(name2, namedest2, namedest1, 2f));
                Reactor.Coroutines.Start(Slide2D(mask1, maskdest1, maskdest2, 2f));
                Reactor.Coroutines.Start(Slide2D(mask2, maskdest2, maskdest1, 2f));
            }
        }

        private static IEnumerator Slide2D(Transform target, Vector2 source, Vector2 dest, float duration = 0.75f)
        {
            var temp = default(Vector3);
            temp.z = target.position.z;
            for (var time = 0f; time < duration; time += Time.deltaTime)
            {
                var t = time / duration;
                temp.x = Mathf.SmoothStep(source.x, dest.x, t);
                temp.y = Mathf.SmoothStep(source.y, dest.y, t);
                target.position = temp;
                yield return null;
            }

            temp.x = dest.x;
            temp.y = dest.y;
            target.position = temp;
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