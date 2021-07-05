using HarmonyLib;
using TownOfUs.CrewmateRoles.AltruistMod;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.LoversMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    public static class Die
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (!CustomGameOptions.BothLoversDie || !__instance.IsLover()) return;

            var otherLover = Role.GetRole<Lover>(__instance).OtherLover.Player;
            if (otherLover.Data.IsDead) return;

            Utils.MurderPlayer(otherLover, otherLover);
        }
    }
}
