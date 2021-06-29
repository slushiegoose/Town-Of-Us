using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(OpenDoorConsole), nameof(OpenDoorConsole.CanUse))]
    public class OpenDoorConsoleUse
    {
        public static bool Prefix(OpenDoorConsole __instance, 
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

    [HarmonyPatch(typeof(DoorConsole), nameof(DoorConsole.CanUse))]
    public class DoorConsoleUse
    {
        public static bool Prefix(DoorConsole __instance,
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


    [HarmonyPatch(typeof(DeconControl), nameof(DeconControl.CanUse))]
    public class DeconControlUse
    {
        public static bool Prefix(DeconControl __instance, 
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