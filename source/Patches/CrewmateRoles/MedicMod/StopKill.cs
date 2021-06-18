using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class StopKill
    {
        public static void BreakShield(byte medicId, byte playerId, bool flag)
        {
            if (PlayerControl.LocalPlayer.PlayerId == playerId &&
                CustomGameOptions.NotificationShield == NotificationOptions.Shielded)
                Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            if (PlayerControl.LocalPlayer.PlayerId == medicId &&
                CustomGameOptions.NotificationShield == NotificationOptions.Medic)
                Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            if (CustomGameOptions.NotificationShield == NotificationOptions.Everyone)
                Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            if (!flag)
                return;

            var player = Utils.PlayerById(playerId);
            foreach (var role in Role.GetRoles(RoleEnum.Medic))
                if (((Medic) role).ShieldedPlayer.PlayerId == playerId)
                {
                    ((Medic) role).ShieldedPlayer = null;
                    ((Medic) role).exShielded = player;
                    System.Console.WriteLine(player.name + " Is Ex-Shielded");
                }

            player.myRend.material.SetColor("_VisorColor", Palette.VisorColor);
            player.myRend.material.SetFloat("_Outline", 0f);
            //System.Console.WriteLine("Broke " + player.name + "'s shield");
        }

        [HarmonyPriority(Priority.First)]
        public static bool Prefix(KillButtonManager __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            if (!PlayerControl.LocalPlayer.Data.IsImpostor) return true;
            var target = __instance.CurrentTarget;
            if (target == null) return true;
            if (target.isShielded())
            {
                if (__instance.isActiveAndEnabled && !__instance.isCoolingDown)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer.Write(target.getMedic().Player.PlayerId);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    System.Console.WriteLine(CustomGameOptions.ShieldBreaks + "- shield break");
                    if (CustomGameOptions.ShieldBreaks)
                        PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);

                    BreakShield(target.getMedic().Player.PlayerId, target.PlayerId, CustomGameOptions.ShieldBreaks);
                }


                return false;
            }


            return true;
        }
    }
}