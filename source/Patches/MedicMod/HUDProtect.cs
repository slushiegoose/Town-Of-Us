using HarmonyLib;

namespace TownOfUs.MedicMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class HUDRewind
    {
        
        public static void Postfix(PlayerControl __instance)
        {
            UpdateProtectButton(__instance);
        }

        public static void UpdateProtectButton(PlayerControl __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Medic)) return;
            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;
            var protectButton = DestroyableSingleton<HudManager>.Instance.KillButton;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];


            var role = Roles.Role.GetRole<Roles.Medic>(PlayerControl.LocalPlayer);



            if (isDead)
            {
                protectButton.gameObject.SetActive(false);
                protectButton.isActive = false;
            }
            else
            {
                protectButton.gameObject.SetActive(true);
                protectButton.isActive = true;
                protectButton.SetCoolDown(0f, 1f);
                role.ClosestPlayer = Utils.getClosestPlayer(PlayerControl.LocalPlayer);
                var distBetweenPlayers = Utils.getDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
                var flag9 = distBetweenPlayers < maxDistance;
                if (flag9 && __instance.enabled && !role.UsedAbility)
                {
                    protectButton.SetTarget(role.ClosestPlayer);
                }
            }
        }
    }
}