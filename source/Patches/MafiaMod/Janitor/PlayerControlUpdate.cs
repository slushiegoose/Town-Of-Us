using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{ 
    [HarmonyPatch(typeof(PlayerControl))]
    public class PlayerControlUpdate
    {

        private static DeadBody closestBody;
        private static float closestDistance;
        
        [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!__instance.AmOwner) return;
            if (!PlayerControl.LocalPlayer.isJanitor()) return;
            var data = __instance.Data;
            var isDead = data.IsDead;
            var truePosition = __instance.GetTruePosition();
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var flag = (PlayerControl.GameOptions.GhostsDoTasks || !data.IsDead) &&
                       (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver) && __instance.CanMove;
            var allocs = Physics2D.OverlapCircleAll(truePosition, maxDistance, 
                Constants.PlayersOnlyMask);
            var killButton = DestroyableSingleton<HudManager>.Instance.KillButton;
            closestBody = null;
            closestDistance = float.MaxValue;
            
            foreach (var collider2D in allocs)
            {
                if (!flag || isDead || collider2D.tag != "DeadBody") continue;
                var component = collider2D.GetComponent<DeadBody>();
                if (!(Vector2.Distance(truePosition, component.TruePosition) <=
                      maxDistance) || PhysicsHelpers.Method_2(truePosition,
                    component.TruePosition, Constants.ShipAndObjectsMask, false)) continue;

                var distance = Vector2.Distance(truePosition, component.TruePosition);
                if (!(distance < closestDistance)) continue;
                closestBody = component;
                closestDistance = distance;

            }
            KillButtonTarget.SetTarget(killButton, closestBody);
            
            if (isDead)
            {
                killButton.gameObject.SetActive(false);
                killButton.isActive = false;
            }
            else
            {
                killButton.gameObject.SetActive(true);
                killButton.isActive = true;
                killButton.SetCoolDown(PerformKillButton.JanitorTimer(), CustomGameOptions.JanitorCleanCd);
            }
        }

    }
}