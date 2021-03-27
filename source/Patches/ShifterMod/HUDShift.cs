using HarmonyLib;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(PlayerControl))]
    public class HUDShift
    {

        [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
        public static void Postfix(PlayerControl __instance)
        {
            UpdateShiftButton(__instance);
        }

        public static void UpdateShiftButton(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.isShifter()) return;
            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var shiftButton = DestroyableSingleton<HudManager>.Instance.KillButton;

            
            
            
            if (isDead)
            {
                shiftButton.gameObject.SetActive(false);
                shiftButton.isActive = false;
            }
            else
            {
                shiftButton.gameObject.SetActive(true);
                shiftButton.isActive = true;
                shiftButton.SetCoolDown(Methods.ShifterShiftTimer(), CustomGameOptions.ShifterCd);
                Methods.ClosestPlayer = Methods.getClosestPlayer(PlayerControl.LocalPlayer);
                var distBetweenPlayers = Methods.getDistBetweenPlayers(PlayerControl.LocalPlayer, Methods.ClosestPlayer);
                var flag9 = distBetweenPlayers < maxDistance;
                if (flag9)
                {
                    shiftButton.SetTarget(Methods.ClosestPlayer);
                }

            }
        }
    }
}