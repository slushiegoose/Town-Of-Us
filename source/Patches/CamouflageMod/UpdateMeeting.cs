using HarmonyLib;
using UnityEngine;

namespace TownOfUs.CamouflageMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public class UpdateMeeting
    {
        private static void Postfix(MeetingHud __instance)
        {
            if (CamouflageMod.CamouflageUnCamouflage.CommsEnabled && CustomGameOptions.MeetingColourblind)
            {
                foreach (var state in __instance.playerStates)
                {
                    if(!PlayerControl.LocalPlayer.Is(RoleEnum.Seer))
                    {
                        state.NameText.Text = "";

                    };
                    PlayerControl.SetPlayerMaterialColors(Color.grey, state.PlayerIcon.Body);
                    state.PlayerIcon.HatSlot.SetHat(0, 0);
                    SkinData skinById = DestroyableSingleton<HatManager>.Instance.AllSkins[0];
                    state.PlayerIcon.SkinSlot.sprite = skinById.IdleFrame;
                }
            }
        }
    }
}