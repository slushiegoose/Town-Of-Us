using HarmonyLib;
using Reactor.Extensions;

namespace TownOfUs.AltruistMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class UpdateArrows
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (Coroutine.Arrow != null)
            {
                if (LobbyBehaviour.Instance || MeetingHud.Instance || PlayerControl.LocalPlayer.Data.IsDead ||
                    Coroutine.Target.Data.IsDead)
                {
                    Coroutine.Arrow.gameObject.Destroy();
                    Coroutine.Target = null;
                    return;
                }

                Coroutine.Arrow.target = Coroutine.Target.transform.position;

            }
        }
        
    }
}