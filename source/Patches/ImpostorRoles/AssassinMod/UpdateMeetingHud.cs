using HarmonyLib;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class UpdateMeetingHud
    {
        private static Vector3 oldScale = Vector3.zero;
        private static Vector3 oldPosition = Vector3.zero;


        private static void UpdateMeeting(MeetingHud __instance, Assassin role)
        {
            for (var i = 0; i < __instance.playerStates.Count; i++)
            {
                var state = __instance.playerStates[i];

                var currentGuess = role.Guesses.ContainsKey(i) ? role.Guesses[i] : null;

                if (role.GuessedThisMeeting || currentGuess == null) continue;


                if (currentGuess == "None")
                    state.NameText.text += "\nGuess";
                else
                    state.NameText.text += "\n<color=#" + role.ColorMapping[currentGuess].ToHtmlStringRGBA() +
                                           $">{currentGuess}??</color>";

                state.NameText.transform.localPosition = new Vector3(0.6f, 0.03f, -0.1f);
                // if (state.NameText.text.Contains("\n"))
                // {
                //     var newScale = Vector3.one * 1.8f;
                //
                //     
                //     //TODO: Fix scale
                //     var trueScale = state.NameText.transform.localScale / 2;
                //
                //
                //     if (trueScale != newScale) oldScale = trueScale;
                //     var newPosition = new Vector3(1.43f, 0.055f, 0f);
                //
                //     var truePosition = state.NameText.transform.localPosition;
                //
                //     if (newPosition != truePosition) oldPosition = truePosition;
                //
                //     state.NameText.transform.localPosition = newPosition;
                //     state.NameText.transform.localScale = newScale;
                // }
                // else
                // {
                //     if (oldPosition != Vector3.zero) state.NameText.transform.localPosition = oldPosition;
                //     if (oldScale != Vector3.zero) state.NameText.transform.localScale = oldScale;
                // }
            }
        }

        public static void Postfix(HudManager __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Assassin)) return;
            if (MeetingHud.Instance)
                UpdateMeeting(MeetingHud.Instance, Role.GetRole<Assassin>(PlayerControl.LocalPlayer));
        }
    }
}