using System;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.SwooperMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Swooper))
            {
                var miner = (Roles.Swooper) role;
                miner.LastSwooped = DateTime.UtcNow;
                miner.LastSwooped = miner.LastSwooped.AddSeconds(-10f);
            }
        }
    }
}