using HarmonyLib;
using Hazel;
using UnityEngine;

namespace TownOfUs.CamouflageMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Camouflager);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var role = Roles.Role.GetRole<Roles.Camouflager>(PlayerControl.LocalPlayer);
            var target = DestroyableSingleton<HudManager>.Instance.KillButton.CurrentTarget;
            if (__instance == role.CamouflageButton)
            {
                if (__instance.isCoolingDown) return false;
                if (!__instance.isActiveAndEnabled) return false;
                if (role.CamouflageTimer() != 0) return false;
                
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.Camouflage,
                    SendOption.Reliable, -1);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                role.TimeRemaining = CustomGameOptions.CamouflagerDuration;
                Utils.Camouflage();

                return false;
            }

            return true;
        }
    }
}