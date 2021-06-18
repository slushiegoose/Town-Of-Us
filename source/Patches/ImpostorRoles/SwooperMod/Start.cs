using System;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.ImpostorRoles.SwooperMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Role.GetRoles(RoleEnum.Swooper))
            {
                var miner = (Swooper) role;
                miner.LastSwooped = DateTime.UtcNow;
                miner.LastSwooped = miner.LastSwooped.AddSeconds(-10f);
            }
        }
    }
}