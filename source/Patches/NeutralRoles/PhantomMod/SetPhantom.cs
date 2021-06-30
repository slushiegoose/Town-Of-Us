using System;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public class SetPhantom
    {
        public static PlayerControl WillBePhantom;
        public static Vector2 StartPosition;
        public static void Postfix(ExileController __instance)
        {
            var exiled = __instance.exiled?.Object;
            var localPlayer = PlayerControl.LocalPlayer;
            if (!localPlayer.Data.IsDead && exiled != localPlayer) return;
            if (exiled == localPlayer && localPlayer.Is(RoleEnum.Jester)) return;
            if (localPlayer != WillBePhantom) return;

            if (!localPlayer.Is(RoleEnum.Phantom))
            {
                Role.RoleDictionary.Remove(localPlayer.PlayerId);
                var role = new Phantom(localPlayer);
                role.RegenTask();
                Lights.SetLights();

                RemoveTasks(PlayerControl.LocalPlayer);
                localPlayer.MyPhysics.ResetMoveState();

                localPlayer.gameObject.layer = LayerMask.NameToLayer("Players");

                var writer = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId,
                    (byte) CustomRPC.PhantomDied, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            if (Role.GetRole<Phantom>(localPlayer).Caught) return;
            var startingVent =
                ShipStatus.Instance.AllVents[Random.RandomRangeInt(0, ShipStatus.Instance.AllVents.Count)];

            localPlayer.NetTransform.RpcSnapTo(startingVent.transform.position + startingVent.Offset);
            localPlayer.MyPhysics.RpcEnterVent(startingVent.Id);
        }

        public static void RemoveTasks(PlayerControl player) =>
            player.SetTasks(player.Data.Tasks);

        public static void AddCollider(Phantom role)
        {
            var player = role.Player;
            var collider2d = player.gameObject.AddComponent<BoxCollider2D>();
            collider2d.isTrigger = true;
            var button = player.gameObject.AddComponent<PassiveButton>();
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnMouseOut = new Button.ButtonClickedEvent();
            button.OnMouseOver = new Button.ButtonClickedEvent();

            button.OnClick.AddListener((Action) (() =>
            {
                if (MeetingHud.Instance) return;
                var localPlayer = PlayerControl.LocalPlayer;
                if (!localPlayer.CanMove) return;
                var localData = localPlayer.Data;
                if (localData.IsDead) return;
                var player = role.Player;
                if (
                    Utils.getDistBetweenPlayers(player, localPlayer) > 
                    (ShipStatus.Instance.MaxLightRadius * PlayerControl.GameOptions.CrewLightMod)
                ) return;
                role.Caught = true;
                var writer = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId,
                    (byte) CustomRPC.CatchPhantom, SendOption.Reliable, -1);
                writer.Write(player.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }));
        }
    }
}
