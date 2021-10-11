using Il2CppSystem.Collections;
using Il2CppSystem;
using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(PlayerVoteArea))]
    public class Allow
    {
        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        public static class Select
        {
            public static bool Prefix(PlayerVoteArea __instance)
            {
                if (
                    MeetingHud.Instance.state == MeetingHud.VoteStates.Animating ||
                    !PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)
                ) return true;
                var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (PlayerControl.LocalPlayer.Data.IsDead) return false;
                if (__instance.AmDead) return false;
                if (!role.CanVote || !__instance.Parent.Select(__instance.TargetPlayerId)) return false;
                __instance.Buttons.SetActive(true);
                var startPos = __instance.AnimateButtonsFromLeft ? 0.2f : 1.95f;
                __instance.StartCoroutine(Effects.All(new IEnumerator[]
                {
                    Effects.Lerp(0.25f, (Action<float>)delegate(float t)
                    {
                        __instance.CancelButton.transform.localPosition = Vector2.Lerp(Vector2.right * startPos, Vector2.right * 1.3f, Effects.ExpOut(t));
                    }),
                    Effects.Lerp(0.35f, (Action<float>)delegate(float t)
                    {
                        __instance.ConfirmButton.transform.localPosition = Vector2.Lerp(Vector2.right * startPos, Vector2.right * 0.65f, Effects.ExpOut(t));
                    })
                }));
                return false;
            }
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.VoteForMe))]
        public static class VoteForMe
        {
            public static bool Prefix(PlayerVoteArea __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return true;
                var role = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding ||
                    __instance.Parent.state == MeetingHud.VoteStates.Results)
                {
                    return false;
                }

                if (!role.CanVote) return false;
                if (__instance != role.Abstain)
                {
                    role.VoteBank--;
                    role.VotedOnce = true;
                }
                else
                {
                    role.SelfVote = true;
                }
                __instance.Parent.Confirm(__instance.TargetPlayerId);
                return false;
            }
        }
    }
}
