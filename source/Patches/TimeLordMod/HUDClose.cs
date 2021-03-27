using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.TimeLordMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HUDClose
    {
        
        public static void Postfix(HudManager __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.TimeLord))
            {
                var TimeLord = (Roles.TimeLord) role;
                TimeLord.FinishRewind = DateTime.UtcNow;
                TimeLord.StartRewind = DateTime.UtcNow;
                TimeLord.FinishRewind = TimeLord.FinishRewind.AddSeconds(-10.0);
                TimeLord.StartRewind = TimeLord.StartRewind.AddSeconds(-20.0);
            }
                
        }
    }
}