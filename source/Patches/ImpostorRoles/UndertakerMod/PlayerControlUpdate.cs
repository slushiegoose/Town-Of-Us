using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.UndertakerMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class PlayerControlUpdate
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Undertaker)) return;

            var role = Role.GetRole<Undertaker>(PlayerControl.LocalPlayer);
            if (role.DragDropButton == null)
            {
                role.DragDropButton = Object.Instantiate(__instance.KillButton, HudManager.Instance.transform);
                role.DragDropButton.renderer.enabled = true;
                role.DragDropButton.renderer.sprite = TownOfUs.DragSprite;
            }

            if (role.DragDropButton.renderer.sprite != TownOfUs.DragSprite &&
                role.DragDropButton.renderer.sprite != TownOfUs.DropSprite)
                role.DragDropButton.renderer.sprite = TownOfUs.DragSprite;

            if (role.DragDropButton.renderer.sprite == TownOfUs.DropSprite && role.CurrentlyDragging == null)
                role.DragDropButton.renderer.sprite = TownOfUs.DragSprite;

            role.DragDropButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance);
            var position = __instance.KillButton.transform.localPosition;
            role.DragDropButton.transform.localPosition = new Vector3(position.x,
                __instance.ReportButton.transform.localPosition.y, position.z);


            if (role.DragDropButton.renderer.sprite == TownOfUs.DragSprite)
            {
                var data = PlayerControl.LocalPlayer.Data;
                var isDead = data.IsDead;
                var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
                var flag = (PlayerControl.GameOptions.GhostsDoTasks || !data.IsDead) &&
                           (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver) &&
                           PlayerControl.LocalPlayer.CanMove;
                var allocs = Physics2D.OverlapCircleAll(truePosition, maxDistance,
                    LayerMask.GetMask(new[] {"Players", "Ghost"}));
                var killButton = role.DragDropButton;
                DeadBody closestBody = null;
                var closestDistance = float.MaxValue;

                foreach (var collider2D in allocs)
                {
                    if (!flag || isDead || collider2D.tag != "DeadBody") continue;
                    var component = collider2D.GetComponent<DeadBody>();
                    if (!(Vector2.Distance(truePosition, component.TruePosition) <=
                          maxDistance)) continue;

                    var distance = Vector2.Distance(truePosition, component.TruePosition);
                    if (!(distance < closestDistance)) continue;
                    closestBody = component;
                    closestDistance = distance;
                }


                KillButtonTarget.SetTarget(killButton, closestBody, role);
            }

            if (role.DragDropButton.renderer.sprite == TownOfUs.DragSprite)
            {
                role.DragDropButton.SetCoolDown(role.DragTimer(), CustomGameOptions.DragCd);
            }
            else
            {
                role.DragDropButton.SetCoolDown(0f, 1f);
                role.DragDropButton.renderer.color = Palette.EnabledColor;
                role.DragDropButton.renderer.material.SetFloat("_Desat", 0f);
            }
        }
    }
}