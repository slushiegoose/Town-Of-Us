using System.Collections;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MedicMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class StopKill
    {

        public static IEnumerator FlashCoroutine()
        {
            var color = Color.cyan;
            color.a = 0.3f;
            var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
            fullscreen.enabled = true;
            fullscreen.color = color;
            yield return new WaitForSeconds(1f);
            fullscreen.enabled = false;
        }
        
        public static void BreakShield(byte playerId, bool flag)
        {
            if (PlayerControl.LocalPlayer.PlayerId == playerId)
            {
                Reactor.Coroutines.Start(FlashCoroutine());
            }

            if (!flag)
                return;

            var player = Utils.PlayerById(playerId);
            foreach (var role in Roles.Role.GetRoles(RoleEnum.Medic))
            {
                if (((Roles.Medic) role).ShieldedPlayer.PlayerId == playerId)
                {
                    ((Roles.Medic) role).ShieldedPlayer = null;
                }
            }
            player.myRend.material.SetColor("_VisorColor", Palette.VisorColor);
            player.myRend.material.SetFloat("_Outline", 0f);
            //System.Console.WriteLine("Broke " + player.name + "'s shield");

            
        }
        
        [HarmonyPriority(Priority.First)]
        public static bool Prefix(KillButtonManager __instance)
        {
            var target = __instance.CurrentTarget;
            if (target == null) return true;
            if(target.isShielded()) {
                if (__instance.isActiveAndEnabled && !__instance.isCoolingDown &&
                    CustomGameOptions.PlayerMurderIndicator)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.AttemptSound, Hazel.SendOption.None, -1);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    BreakShield(target.PlayerId, false);
                }

                return false;
            }
            

            return true;
        }
    }
}