using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.ImpostorRoles.MinerMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Miner);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            var role = Role.GetRole<Miner>(PlayerControl.LocalPlayer);
            if (__instance == role.MineButton)
            {
                if (__instance.isCoolingDown) return false;
                if (!__instance.isActiveAndEnabled) return false;
                if (!role.CanPlace) return false;
                if (role.MineTimer() != 0) return false;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.Mine, SendOption.Reliable, -1);
                var position = PlayerControl.LocalPlayer.transform.position;
                var id = GetAvailableId();
                writer.Write(id);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(position);
                writer.Write(0.01f);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                SpawnVent(id, role, position, 0.01f);
                return false;
            }

            return true;
        }


        public static void SpawnVent(int ventId, Miner role, Vector2 position, float zAxis)
        {
            var ventPrefab = Object.FindObjectOfType<Vent>();
            var vent = Object.Instantiate(ventPrefab, ventPrefab.transform.parent);
            vent.Id = ventId;
            vent.transform.position = new Vector3(position.x, position.y, zAxis);

            if (role.Vents.Count > 0)
            {
                var leftVent = role.Vents[^1];
                vent.Left = leftVent;
                leftVent.Right = vent;
            }
            else
            {
                vent.Left = null;
            }

            vent.Right = null;
            vent.Center = null;

            var allVents = ShipStatus.Instance.AllVents.ToList();
            allVents.Add(vent);
            ShipStatus.Instance.AllVents = allVents.ToArray();

            role.Vents.Add(vent);
            role.LastMined = DateTime.UtcNow;
        }

        public static int GetAvailableId()
        {
            var id = 0;

            while (true)
            {
                if (ShipStatus.Instance.AllVents.All(v => v.Id != id)) return id;
                id++;
            }
        }
    }
}