using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.TimeLordMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.TimeLord))
            {
                var TimeLord = (Roles.TimeLord) role;
                TimeLord.FinishRewind = DateTime.UtcNow;
                TimeLord.StartRewind = DateTime.UtcNow;
                TimeLord.StartRewind = TimeLord.StartRewind.AddSeconds(-10.0);
            }
        }
    }
}