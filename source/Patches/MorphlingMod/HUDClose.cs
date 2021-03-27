using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.MorphlingMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Morphling))
            {
                var role = Roles.Role.GetRole<Roles.Morphling>(PlayerControl.LocalPlayer);
                role.MorphButton.renderer.sprite = TownOfUs.SampleSprite;
                role.SampledPlayer = null;
                role.LastMorphed = DateTime.UtcNow;
                
            }

        }
    }
}