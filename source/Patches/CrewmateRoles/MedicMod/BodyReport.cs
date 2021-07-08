using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    internal class BodyReportPatch
    {
        private static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo info)
        {
            if (
                !CustomGameOptions.ShowReports ||
                info == null ||
                !__instance.AmOwner ||
                !__instance.Is(RoleEnum.Medic)
            ) return;
            var deadPlayer = Murder.KilledPlayers.FirstOrDefault(x => x.PlayerId == info.PlayerId);
            if (deadPlayer == null) return;

            var report = new BodyReport
            {
                Killer = Utils.PlayerById(deadPlayer.KillerId),
                Reporter = __instance,
                Body = Utils.PlayerById(deadPlayer.PlayerId),
                KillAge = (float) (DateTime.UtcNow - deadPlayer.KillTime).TotalMilliseconds
            };

            var reportMessage = BodyReport.ParseBodyReport(report);

            if (string.IsNullOrWhiteSpace(reportMessage))
                return;
            
            HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, reportMessage);
        }
    }
}
