using System.Linq;
using HarmonyLib;
using Reactor.Extensions;
using TownOfUs.Extensions;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.SnitchMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class UpdateArrows
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (!__instance.AmOwner) return;
            var snitch = Role.GetRole<Snitch>();
            if (snitch == null) return;
            var snitchArrows = snitch.SnitchArrows;
            var impArrows = snitch.ImpArrows;
            if ((__instance.Data.IsDead || snitch.Player.Data.IsDead) && snitchArrows.Count > 0)
            {
                snitchArrows.DestroyAll();
                snitchArrows.Clear();
                impArrows.DestroyAll();
                impArrows.Clear();
                return;
            }

            foreach (var arrow in impArrows)
                arrow.target = snitch.Player.transform.position;

            foreach (var (arrow, target) in Utils.Zip(snitchArrows, snitch.SnitchTargets))
            {
                if (target.Data.IsDead)
                {
                    arrow.Destroy();
                    arrow.gameObject?.Destroy();
                }

                arrow.target = target.transform.position;
            }
        }
    }
}
