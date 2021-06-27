using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
    public class CompleteTask
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (!__instance.Is(RoleEnum.Phantom)) return;
            var role = Role.GetRole<Phantom>(__instance);

            var taskinfos = __instance.Data.Tasks.ToArray();

            var tasksLeft = taskinfos.Count(x => !x.Complete);

            if (tasksLeft == 0 && !role.Caught)
            {
                role.CompletedTasks = true;
                if (AmongUsClient.Instance.AmHost)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.PhantomWin, SendOption.Reliable, -1);
                    writer.Write(role.Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    Utils.EndGame();
                }
            }
        }
    }
}