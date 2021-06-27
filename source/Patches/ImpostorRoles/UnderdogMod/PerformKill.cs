using System.Linq;
using HarmonyLib;

namespace TownOfUs.ImpostorRoles.UnderdogMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public class PerformKill
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (__instance.Is(RoleEnum.Underdog))
                __instance.SetKillTimer(PlayerControl.GameOptions.KillCooldown * (LastImp() ? 0.5f : 1.5f));
        }

        internal static bool LastImp()
        {
            return PlayerControl.AllPlayerControls.ToArray().Count(x => x.Data.IsImpostor && !x.Data.IsDead) ==
                   1;
        }
    }
}