using HarmonyLib;

namespace TownOfUs.MafiaMod.Mafioso
{
    [HarmonyPatch(typeof(UseButtonManager))]
    public class UseButton
    {

        [HarmonyPatch(typeof(UseButtonManager), nameof(UseButtonManager.SetTarget))]
        public static class SetTarget
        {
            public static void Postfix(UseButtonManager __instance, [HarmonyArgument(0)] IUsable usable)
            {
                if (!(PlayerControl.AllPlayerControls.Count > 1 &&
                      PlayerControl.LocalPlayer.Is(RoleEnum.Mafioso))) return;

                var role = Roles.Role.GetRole<Roles.Mafioso>(PlayerControl.LocalPlayer);
                //System.Console.WriteLine(role.Godfather.Player.Data.IsDead ? "ISDEADUSE" : "ISNOTDEADUSE");
                if (role.Godfather.Player.Data.IsDead) return;
                /*if (__instance.UseButton.sprite != __instance.SabotageImage) return;
                __instance.UseButton.sprite = __instance.UseImage;
                __instance.UseButton.color = UseButtonManager.DisabledColor;*/
                if (usable != null) return;
                __instance.UseButton.sprite = TownOfUs.UseButton;
                __instance.UseButton.color = UseButtonManager.Field_0;
            }
        }

        [HarmonyPatch(typeof(UseButtonManager), nameof(UseButtonManager.DoClick))]
        public static class DoClick
        {
            public static bool Prefix(UseButtonManager __instance)
            {
                if (!(PlayerControl.AllPlayerControls.Count > 1 && PlayerControl.LocalPlayer.Is(RoleEnum.Mafioso)))
                    return true;

                var role = Roles.Role.GetRole<Roles.Mafioso>(PlayerControl.LocalPlayer);
                //System.Console.WriteLine(role.Godfather.Player.Data.IsDead ? "ISDEADCLICK" : "ISNOTDEADCLICK");
                if (role.Godfather.Player.Data.IsDead) return true;
                var data = PlayerControl.LocalPlayer.Data;
                if (__instance.Field_3 != null)
                {
                    return true;
                }

                return data == null || !data.IsImpostor;
            }
        }
    }
}