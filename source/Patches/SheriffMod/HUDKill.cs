using HarmonyLib;

namespace TownOfUs.SheriffMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HUDKill
    {

        private static KillButtonManager KillButton;
        
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
            var flag7 = PlayerControl.AllPlayerControls.Count > 1;
            if (!flag7) return;
            var flag8 = PlayerControl.LocalPlayer.Is(RoleEnum.Sheriff);
            if (flag8)
            {
                var role = Roles.Role.GetRole<Roles.Sheriff>(PlayerControl.LocalPlayer);
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
                    KillButton.SetCoolDown(role.SheriffKillTimer(), PlayerControl.GameOptions.KillCooldown + 15f);
                    role.ClosestPlayer = Utils.getClosestPlayer(PlayerControl.LocalPlayer);
                    var distBetweenPlayers = Utils.getDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
                    var flag9 = distBetweenPlayers < GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
                    if (flag9 && KillButton.enabled)
                    {
                        KillButton.SetTarget(role.ClosestPlayer);
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