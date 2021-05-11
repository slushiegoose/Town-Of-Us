using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    public class AddAbstain
    {

        private static Sprite Abstain => TownOfUs.Abstain;
        public static void UpdateButton(Mayor role, MeetingHud __instance)
        {
            var skip = __instance.SkipVoteButton;
            role.Abstain.gameObject.SetActive(skip.gameObject.active && !role.VotedOnce);
            role.Abstain.voteComplete = skip.voteComplete;
            role.Abstain.GetComponent<SpriteRenderer>().enabled = skip.GetComponent<SpriteRenderer>().enabled;
            role.Abstain.GetComponent<SpriteRenderer>().sprite = Abstain;
            
        }
        
        
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public class MeetingHudStart
        {
            

            public static void GenButton(Mayor role, MeetingHud __instance)
            {
                var skip = __instance.SkipVoteButton;
                role.Abstain = Object.Instantiate(skip, skip.transform.parent);
                role.Abstain.Parent = __instance;
                role.Abstain.SetTargetPlayerId(11);
                role.Abstain.transform.localPosition = skip.transform.localPosition +
                                                       new Vector3(0f, -0.17f, 0f);
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
                if (__0 != 11)
                {
                    mayorRole.Abstain.ClearButtons();
                }

                UpdateButton(mayorRole, __instance);
            }
        }
        
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public class MeetingHudBBFDNCCEJHI
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
                        if (__instance.discussionTimer < (float) PlayerControl.GameOptions.DiscussionTime)
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