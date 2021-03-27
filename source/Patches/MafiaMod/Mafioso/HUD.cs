using HarmonyLib;

namespace TownOfUs.MafiaMod.Mafioso
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HUD
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || !PlayerControl.LocalPlayer.isMafioso()) return;
            var killButton = __instance.KillButton;
            if (!Utils.Godfather.Data.IsDead)
            {
                killButton.gameObject.SetActive(false);
                killButton.isActive = false;
            }
            else if (!Utils.Mafioso.Data.IsDead)
            {
                killButton.gameObject.SetActive(true);
                killButton.isActive = true;
            }
        }
    }
}