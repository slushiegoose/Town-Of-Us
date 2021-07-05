using HarmonyLib;
using Hazel;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    public static class RemoveShield
    {
        public static void Postfix(PlayerControl __instance)
        {
            var medic = Role.GetRole<Medic>();
            if (medic?.ShieldedPlayer == null) return;
            if (medic.Player == __instance || medic.ShieldedPlayer == __instance)
                StopKill.BreakShield(medic, medic.ShieldedPlayer, true);
        }
    }

    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class StopKill
    {
        public static void BreakShield(Medic medic, PlayerControl player, bool breakShield)
        {
            var notification = CustomGameOptions.NotificationShield;
            var medicPlayer = medic.Player;

            if (
                (notification == NotificationOptions.Everyone) ||
                (player.AmOwner && notification == NotificationOptions.Shielded) ||
                (medicPlayer.AmOwner && notification == NotificationOptions.Medic)
            ) Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            if (!breakShield) return;

            if (medic.ShieldedPlayer == player)
                medic.ShieldedPlayer = null;

            Role.NamePatch.UpdateDisplay(player);
        }

        [HarmonyPriority(Priority.First)]
        public static bool Prefix(KillButtonManager __instance)
        {
            if (
                __instance != DestroyableSingleton<HudManager>.Instance.KillButton ||
                !PlayerControl.LocalPlayer.Data.IsImpostor
            ) return true;
            var target = __instance.CurrentTarget;
            if (target == null || !target.IsShielded()) return true;

            if (__instance.isActiveAndEnabled && !__instance.isCoolingDown)
            {
                Utils.RpcBreakShield(target);
            }

            return false;
        }
    }
}
