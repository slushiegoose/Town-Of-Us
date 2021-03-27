using System;
using HarmonyLib;

namespace TownOfUs.SwooperMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swooper))
            {
                var role = Roles.Role.GetRole<Roles.Swooper>(PlayerControl.LocalPlayer);
                role.LastSwooped = DateTime.UtcNow;
                
            }

        }
    }
}