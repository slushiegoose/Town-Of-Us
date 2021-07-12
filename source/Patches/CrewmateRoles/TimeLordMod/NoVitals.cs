using HarmonyLib;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.TimeLordMod
{
    [HarmonyPatch(typeof(SystemConsole), nameof(SystemConsole.CanUse))]
    public class NoVitals
    {
        public static bool Prefix(
            SystemConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo player/*,
            [HarmonyArgument(1)] ref bool canUse,
            [HarmonyArgument(2)] ref bool couldUse*/)
        {
            return
                CustomGameOptions.TimeLordVitals ||
                __instance.UseIcon != ImageNames.VitalsButton ||
                !player.Object.Is(RoleEnum.TimeLord);
            /*
            if (
                !CustomGameOptions.TimeLordVitals &&
                __instance.UseIcon == ImageNames.VitalsButton
            )
            {
                return !player.Object.Is(RoleEnum.TimeLord);
            }
            return true;*/
        }
    }
}
