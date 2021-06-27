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

            if (role.CurrentlyDragging != null)
            {
                var velocity = __instance.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;

                role.CurrentlyDragging.transform.position = __instance.transform.position - (Vector3) velocity / 3;


                if (__instance.AmOwner)
                {
                    var renderer = role.CurrentlyDragging.GetComponent<SpriteRenderer>();
                    renderer.material.SetColor("_OutlineColor", Color.green);
                    renderer.material.SetFloat("_Outline", 1f);
                    if (__instance.CanMove) __instance.killTimer += Time.fixedDeltaTime;
                }
            }
        }
    }
}