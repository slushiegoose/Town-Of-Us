using System.Linq;
using HarmonyLib;
using InnerNet;

namespace TownOfUs.GlitchMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class Update
    {
        static void Postfix(HudManager __instance)
        {
            var glitch = Roles.Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {
                if (glitch != null)
                {
                    if (PlayerControl.LocalPlayer.Is(RoleEnum.Glitch))
                        Roles.Role.GetRole<Roles.Glitch>(PlayerControl.LocalPlayer).Update(__instance);
                }
            }
        }
    }
}
