using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.UndertakerMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class DragBody
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (!__instance.Is(RoleEnum.Undertaker)) return;
            var role = Role.GetRole<Undertaker>(__instance);
            var body = role.CurrentlyDragging;
            if (body == null) return;
            if (__instance.Data.IsDead)
            {
                role.CurrentlyDragging = null;
                body.bodyRenderer.material.SetFloat("_Outline", 0f);
                return;
            }
            var currentPosition = __instance.GetTruePosition();
            var velocity = __instance.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
            var newPos = ((Vector2) __instance.transform.position) - (velocity / 3) + body.myCollider.offset;
            if (!PhysicsHelpers.AnythingBetween(
                currentPosition,
                newPos,
                Constants.ShipAndObjectsMask,
                false
            )) body.transform.position = newPos;
            if (!__instance.AmOwner) return;
            var material = body.bodyRenderer.material;
            material.SetColor("_OutlineColor", Color.green);
            material.SetFloat("_Outline", 1f);
        }
    }
}
