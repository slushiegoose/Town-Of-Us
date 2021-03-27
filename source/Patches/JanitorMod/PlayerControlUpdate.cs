using HarmonyLib;
using UnityEngine;

namespace TownOfUs.JanitorMod
{ 
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class PlayerControlUpdate
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!__instance.AmOwner) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Janitor)) return;
            if (CustomGameOptions.JanitorKill && Utils.IsLastImp(PlayerControl.LocalPlayer)) return;
            var data = __instance.Data;
            var isDead = data.IsDead;
            var truePosition = __instance.GetTruePosition();
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var flag = (PlayerControl.GameOptions.GhostsDoTasks || !data.IsDead) &&
                       (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver) && __instance.CanMove;
            var allocs = Physics2D.OverlapCircleAll(truePosition, maxDistance, 
                Constants.PlayersOnlyMask);
            var killButton = DestroyableSingleton<HudManager>.Instance.KillButton;
            DeadBody closestBody = null;
            var closestDistance = float.MaxValue;
            
            foreach (var collider2D in allocs)
            {
                if (!flag || isDead || collider2D.tag != "DeadBody") continue;
                var component = collider2D.GetComponent<DeadBody>();
                if (!(Vector2.Distance(truePosition, component.TruePosition) <=
                      maxDistance) || PhysicsHelpers.AnythingBetween(truePosition,
                    component.TruePosition, Constants.ShipAndObjectsMask, false)) continue;

                var distance = Vector2.Distance(truePosition, component.TruePosition);
                if (!(distance < closestDistance)) continue;
                closestBody = component;
                closestDistance = distance;

            }

            var role = Roles.Role.GetRole<Roles.Janitor>(PlayerControl.LocalPlayer);
            KillButtonTarget.SetTarget(killButton, closestBody, role);
            
            if (isDead)
            {
                killButton.gameObject.SetActive(false);
                killButton.isActive = false;
            }
            else
            {
                killButton.gameObject.SetActive(true);
                killButton.isActive = true;
                killButton.SetCoolDown(role.JanitorTimer(), CustomGameOptions.JanitorCleanCd);
            }
        }

    }
}