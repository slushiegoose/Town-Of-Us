using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.IsUiControllerActive), MethodType.Getter)]
    public static class MovementPatch
    {
        public static bool Prefix(out bool __result) => __result = false;
    }
}
