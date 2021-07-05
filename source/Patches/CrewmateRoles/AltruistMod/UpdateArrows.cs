using HarmonyLib;
using Reactor.Extensions;

namespace TownOfUs.CrewmateRoles.AltruistMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class UpdateArrows
    {
        public static void Postfix()
        {
            if (AltruistCoroutine.Arrow == null) return;

            if (
                LobbyBehaviour.Instance ||
                MeetingHud.Instance ||
                PlayerControl.LocalPlayer.Data.IsDead ||
                AltruistCoroutine.Target.Data.IsDead)
            {
                AltruistCoroutine.Arrow.gameObject.Destroy();
                AltruistCoroutine.Target = null;
            }
            else
                AltruistCoroutine.Arrow.target = AltruistCoroutine.Target.transform.position;
        }
    }
}
