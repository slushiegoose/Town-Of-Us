using System;
using HarmonyLib;
using Hazel;
using Reactor.Extensions;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.UndertakerMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class DragBody
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (!__instance.Is(RoleEnum.Undertaker)) return;
            var role = Role.GetRole<Undertaker>(__instance);
            var body = role.CurrentlyDragging;
            if (body == null) return;


            if (__instance.Data.IsDead)
            {
                if (__instance.AmOwner)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.Drop, SendOption.Reliable, -1);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    var position = PlayerControl.LocalPlayer.GetTruePosition();
                    writer.Write(position);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    // body.transform.position = position;

                    role.CurrentlyDragging = null;
                    body.bodyRenderer.material.SetFloat("_Outline", 0f);
                    role.LastDragged = DateTime.UtcNow;

                }
                return;
            }

            var currentPosition = __instance.GetTruePosition();
            var velocity = __instance.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
            var newPos = ((Vector2) __instance.transform.position) - (velocity / 3) + body.myCollider.offset;
            if (PhysicsHelpers.AnythingBetween(
                currentPosition,
                newPos,
                Constants.ShipAndObjectsMask,
                false
            ))
            {
                body.transform.position = currentPosition;
            }
            else
            {
                body.transform.position = newPos;
            }

            if (!__instance.AmOwner) return;
            var material = body.bodyRenderer.material;
            material.SetColor("_OutlineColor", Color.green);
            material.SetFloat("_Outline", 1f);
            
        }
    }
}