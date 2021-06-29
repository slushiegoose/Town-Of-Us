using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(PlatformConsole), nameof(PlatformConsole.CanUse))]
    public class PlatformConsoleUse
    {
        public static bool Prefix(PlatformConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            [HarmonyArgument(1)] ref bool canUse,
            [HarmonyArgument(2)] ref bool couldUse,
            ref float __result)
        {
            var playerControl = playerInfo.Object;

            if (playerControl.Is(RoleEnum.Phantom) && Role.GetRole<Phantom>(playerControl).Caught)
            {
                __result = float.MaxValue;
                return canUse = couldUse = false;
            }
            
            return true;
        }
    }
}