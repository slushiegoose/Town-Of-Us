using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    public static class LowLights
    {
        public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo player,
            ref float __result)
        {
            if (player == null || player.IsDead)
            {
                __result = __instance.MaxLightRadius;
                return false;
            }
            var switchSystem = __instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
            if (player.IsImpostor || player._object.Is(RoleEnum.Glitch))
            {
                __result = __instance.MaxLightRadius * PlayerControl.GameOptions.ImpostorLightMod;
                return false;
            }
            var t = switchSystem.Value / 255f;
            if (player._object.Is(ModifierEnum.Torch))
            {
                t = 1;
            }
            __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, t) * PlayerControl.GameOptions.CrewLightMod;
            return false;

        }
    }
}