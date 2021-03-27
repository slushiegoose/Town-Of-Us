using HarmonyLib;

namespace TownOfUs.MafiaMod.Mafioso
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HUD
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || !PlayerControl.LocalPlayer.Is(RoleEnum.Mafioso)) return;
            var role = Roles.Role.GetRole<Roles.Mafioso>(PlayerControl.LocalPlayer);
            var killButton = __instance.KillButton;
            //System.Console.WriteLine(role.Godfather.Player.Data.IsDead ? "ISDEAD" : "ISNOTDEAD");
            if (!role.Godfather.Player.Data.IsDead)
            {
                killButton.gameObject.SetActive(false);
                killButton.isActive = false;
            }
            else if (!role.Player.Data.IsDead)
            {
                killButton.gameObject.SetActive(true);
                killButton.isActive = true;
            }
        }
    }
}