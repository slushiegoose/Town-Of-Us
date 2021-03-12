using HarmonyLib;

namespace TownOfUs.MafiaMod.Mafioso
{
    [HarmonyPatch(typeof(UseButtonManager))]
    public class UseButton
    {

        [HarmonyPatch(nameof(UseButtonManager.SetTarget))]
        public static void Postfix(UseButtonManager __instance)
        {
            if (!(PlayerControl.AllPlayerControls.Count > 1 && PlayerControl.LocalPlayer.isMafioso() &&
                  !Utils.Godfather.Data.IsDead)) return;
            if (__instance.UseButton.sprite != __instance.SabotageImage) return;
            __instance.UseButton.sprite = __instance.UseImage;
            __instance.UseButton.color = UseButtonManager.DisabledColor;
        }

        [HarmonyPatch(nameof(UseButtonManager.DoClick))]
        public static bool Prefix(UseButtonManager __instance)
        {
            if (!(PlayerControl.AllPlayerControls.Count > 1 && PlayerControl.LocalPlayer.isMafioso() &&
                  !Utils.Godfather.Data.IsDead)) return true;
            var data = PlayerControl.LocalPlayer.Data;
            if (__instance.Field_7 != null)
            {
                return true;
            }
            return data == null || !data.IsImpostor;
        }
    }
}