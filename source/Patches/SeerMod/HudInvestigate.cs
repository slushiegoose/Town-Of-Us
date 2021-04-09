using System.Linq;
using HarmonyLib;

namespace TownOfUs.SeerMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class HudInvestigate
    {
        public static void Postfix(PlayerControl __instance)
        {
            UpdateInvButton(__instance);
        }

        public static void UpdateInvButton(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Seer)) return;
            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var investigateButton = DestroyableSingleton<HudManager>.Instance.KillButton;

            var role = Roles.Role.GetRole<Roles.Seer>(PlayerControl.LocalPlayer);
            
            
            if (isDead)
            {
                investigateButton.gameObject.SetActive(false);
                investigateButton.isActive = false;
            }
            else
            {
                investigateButton.gameObject.SetActive(!MeetingHud.Instance);
                investigateButton.isActive = !MeetingHud.Instance;
                investigateButton.SetCoolDown(role.SeerTimer(), CustomGameOptions.SeerCd);
                role.ClosestPlayer = Utils.getClosestPlayer(PlayerControl.LocalPlayer,
                    PlayerControl.AllPlayerControls.ToArray().Where(x => !role.Investigated.Contains(x.PlayerId)).ToList());
                var distBetweenPlayers = Utils.getDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
                var flag9 = distBetweenPlayers < maxDistance;
                if (flag9 && __instance.enabled)
                {
                    investigateButton.SetTarget(role.ClosestPlayer);
                }

            }
        }
    }
}