using System;
using HarmonyLib;

namespace TownOfUs.MinerMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Miner))
            {
                var role = Roles.Role.GetRole<Roles.Miner>(PlayerControl.LocalPlayer);
                role.LastMined = DateTime.UtcNow;
                
            }

        }
    }
}