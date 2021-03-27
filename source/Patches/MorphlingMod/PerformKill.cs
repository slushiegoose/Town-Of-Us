using System;
using HarmonyLib;
using Hazel;
using UnityEngine;

namespace TownOfUs.MorphlingMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKill
    {
        public static Sprite SampleSprite => TownOfUs.SampleSprite;
        public static Sprite MorphSprite => TownOfUs.MorphSprite;
        
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Morphling);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var role = Roles.Role.GetRole<Roles.Morphling>(PlayerControl.LocalPlayer);
            var target = DestroyableSingleton<HudManager>.Instance.KillButton.CurrentTarget;
            if (__instance == role.MorphButton)
            {
                
                if (!__instance.isActiveAndEnabled) return false;
                if (role.MorphButton.renderer.sprite == SampleSprite)
                {
                    if (target == null) return false;
                    role.SampledPlayer = target;
                    role.MorphButton.renderer.sprite = MorphSprite;
                    role.MorphButton.SetTarget(null);
                    if (role.MorphTimer() < 5f)
                    {
                        role.LastMorphed = DateTime.UtcNow.AddSeconds(5 - CustomGameOptions.MorphlingCd);
                    }
                }
                else
                {
                    if (__instance.isCoolingDown) return false;
                    if (role.MorphTimer() != 0) return false;
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.Morph,
                        SendOption.Reliable, -1);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(role.SampledPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    role.TimeRemaining = CustomGameOptions.MorphlingDuration;
                    role.MorphedPlayer = role.SampledPlayer;
                    Utils.Morph(role.Player, role.SampledPlayer, true);
                }

                return false;
            }

            return !target.Data.IsImpostor;
        }
    }
}