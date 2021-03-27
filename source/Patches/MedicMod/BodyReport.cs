using System;
using System.Linq;
using HarmonyLib;

namespace TownOfUs.MedicMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    class BodyReportPatch
    {
        static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo info)
        {
            //System.Console.WriteLine("Report Body!");
            var matches = Murder.KilledPlayers.Where(x => x.PlayerId == info.PlayerId).ToArray();
            DeadPlayer killer = null;

            if (matches.Length > 0)
            {
                //System.Console.WriteLine("RBOOF");
                killer = matches[0];
            }

            if (killer == null)
            {
                //System.Console.WriteLine("RBTWOOF");
                return;
            }

            var isMedicAlive = __instance.Is(RoleEnum.Medic);
            var areReportsEnabled = CustomGameOptions.ShowReports;

            if (!isMedicAlive || !areReportsEnabled)
                return;

            var isUserMedic = PlayerControl.LocalPlayer.Is(RoleEnum.Medic);
            if (!isUserMedic)
                return;
            //System.Console.WriteLine("RBTHREEF");
            var br = new BodyReport
            {
                Killer = Utils.PlayerById(killer.KillerId),
                Reporter = __instance,
                KillAge = (float) (DateTime.UtcNow - killer.KillTime).TotalMilliseconds,
            };
            
            //System.Console.WriteLine("FIVEF");

            var reportMsg = BodyReport.ParseBodyReport(br);
            
            //System.Console.WriteLine("SIXTHF");

            if (string.IsNullOrWhiteSpace(reportMsg))
                return;
            
            //System.Console.WriteLine("SEFENFTH");

            if (DestroyableSingleton<HudManager>.Instance)
            {
                // Send the message through chat only visible to the medic
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, reportMsg);
            }
        }
    }
}