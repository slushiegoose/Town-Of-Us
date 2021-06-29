using System;
using HarmonyLib;
using TownOfUs.Roles;
using Object = UnityEngine.Object;

namespace TownOfUs.ImpostorRoles.UndertakerMod
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Undertaker))
            {
                var role = Role.GetRole<Undertaker>(PlayerControl.LocalPlayer);
                role.DragDropButton.renderer.sprite = TownOfUs.DragSprite;
                role.CurrentlyDragging = null;
                role.LastDragged = DateTime.UtcNow;
            }
        }
    }
}
