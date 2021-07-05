using HarmonyLib;
using TMPro;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MayorMod
{
    public class AddAbstain
    {
        private static Sprite Abstain => TownOfUs.Abstain;

        public static void UpdateButton(Mayor role, MeetingHud __instance)
        {
            var skip = __instance.SkipVoteButton;
            var abstain = role.Abstain;
            abstain.gameObject.SetActive(skip.gameObject.active && !role.VotedOnce);
            if (role.VotedOnce) skip.gameObject.SetActive(false);
            abstain.voteComplete = skip.voteComplete;
            var renderer = abstain.GetComponent<SpriteRenderer>();
            renderer.enabled = skip.GetComponent<SpriteRenderer>().enabled;
            renderer.sprite = Abstain;
            abstain.skipVoteText.text = "Abstain";
        }


        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public class MeetingHudStart
        {
            public static void GenButton(Mayor role, MeetingHud __instance)
            {
                var skip = __instance.SkipVoteButton;
                var abstain = role.Abstain = Object.Instantiate(skip, skip.transform.parent);
                abstain.Parent = __instance;
                abstain.SetTargetPlayerId(251);
                abstain.transform.localPosition =
                    skip.transform.localPosition + new Vector3(0f, -0.17f, 0f);
                abstain.skipVoteText.gameObject.SetActive(false);
                skip.transform.localPosition += new Vector3(0f, 0.20f, 0f);
                UpdateButton(role, __instance);
            }

            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var mayorRole = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                GenButton(mayorRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ClearVote))]
        public class MeetingHudClearVote
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var mayorRole = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                UpdateButton(mayorRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public class MeetingHudConfirm
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var mayorRole = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                mayorRole.Abstain.ClearButtons();
                UpdateButton(mayorRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Select))]
        public class MeetingHudSelect
        {
            public static void Postfix(MeetingHud __instance, int __0)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var mayorRole = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                if (__0 != 251) mayorRole.Abstain.ClearButtons();

                UpdateButton(mayorRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public class MeetingHudVotingComplete
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var mayorRole = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                UpdateButton(mayorRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        public class MeetingHudUpdate
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) return;
                var mayorRole = Role.GetRole<Mayor>(PlayerControl.LocalPlayer);
                switch (__instance.state)
                {
                    case MeetingHud.VoteStates.Discussion:
                        if (__instance.discussionTimer < PlayerControl.GameOptions.DiscussionTime)
                        {
                            mayorRole.Abstain.SetDisabled();
                            break;
                        }


                        mayorRole.Abstain.SetEnabled();
                        break;
                }

                UpdateButton(mayorRole, __instance);
            }
        }
    }
}
