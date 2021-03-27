using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(HudManager))]
    public class HUDKill
    {

        private static KillButtonManager KillButton;
        
        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            UpdateKillButton(__instance);
        }

        private static void UpdateKillButton(HudManager __instance)
        {
            KillButton = __instance.KillButton;
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            var flag7 = PlayerControl.AllPlayerControls.Count > 1 & Utils.Sheriff != null;
            if (!flag7) return;
            var flag8 = PlayerControl.LocalPlayer.isSheriff();
            if (flag8)
            {
                var isDead = PlayerControl.LocalPlayer.Data.IsDead;
                if (isDead)
                {
                    KillButton.gameObject.SetActive(false);
                    KillButton.isActive = false;
                }
                else
                {
                    KillButton.gameObject.SetActive(true);
                    KillButton.isActive = true;
                    KillButton.SetCoolDown(Methods.SheriffKillTimer(), PlayerControl.GameOptions.KillCooldown + 15f);
                    Methods.ClosestPlayer = Methods.getClosestPlayer(PlayerControl.LocalPlayer);
                    var distBetweenPlayers = Methods.getDistBetweenPlayers(PlayerControl.LocalPlayer, Methods.ClosestPlayer);
                    var flag9 = distBetweenPlayers < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
                    if (flag9)
                    {
                        KillButton.SetTarget(Methods.ClosestPlayer);
                    }
                }
            }
            else
            {
                var isImpostor = PlayerControl.LocalPlayer.Data.IsImpostor;
                if (!isImpostor) return;
                var isDead2 = PlayerControl.LocalPlayer.Data.IsDead;
                if (isDead2)
                {
                    KillButton.gameObject.SetActive(false);
                    KillButton.isActive = false;
                }
                else
                {
                    __instance.KillButton.gameObject.SetActive(true);
                    __instance.KillButton.isActive = true;
                }
            }

        }
    }
}