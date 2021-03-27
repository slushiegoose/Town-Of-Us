using HarmonyLib;

namespace TownOfUs.GlitchMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    class PerformKill
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Glitch) && __instance.isActiveAndEnabled &&
                !__instance.isCoolingDown)
            {
                return Roles.Role.GetRole<Roles.Glitch>(PlayerControl.LocalPlayer).UseAbility(__instance);
            }

            return true;
        }
    }
}
