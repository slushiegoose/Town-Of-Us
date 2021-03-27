
using System;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.MinerMod
{
    
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class Start
    {
        public static void Postfix(ShipStatus __instance)
        {
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Miner))
            {
                var miner = (Roles.Miner) role;
                miner.LastMined = DateTime.UtcNow;
                miner.LastMined = miner.LastMined.AddSeconds(-10f);
                var vents = Object.FindObjectsOfType<Vent>();
                miner.VentSize = Vector2.Scale(vents[0].GetComponent<BoxCollider2D>().size, vents[0].transform.localScale) * 0.75f;
            }
        }
    }
}