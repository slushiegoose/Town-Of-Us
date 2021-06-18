using System;
using System.Collections.Generic;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    public class DeadPlayer
    {
        public byte KillerId { get; set; }
        public byte PlayerId { get; set; }
        public DateTime KillTime { get; set; }
    }

    //body report class for when medic reports a body
    public class BodyReport
    {
        public PlayerControl Killer { get; set; }
        public PlayerControl Reporter { get; set; }
        public PlayerControl Body { get; set; }
        public float KillAge { get; set; }

        public static string ParseBodyReport(BodyReport br)
        {
            //System.Console.WriteLine(br.KillAge);
            if (br.KillAge > CustomGameOptions.MedicReportColorDuration * 1000)
                return
                    $"Body Report: The corpse is too old to gain information from. (Killed {Math.Round(br.KillAge / 1000)}s ago)";

            if (br.Killer.PlayerId == br.Body.PlayerId)
                return
                    $"Body Report: The kill appears to have been a suicide! (Killed {Math.Round(br.KillAge / 1000)}s ago)";

            if (br.KillAge < CustomGameOptions.MedicReportNameDuration * 1000)
                return
                    $"Body Report: The killer appears to be {br.Killer.Data.PlayerName}! (Killed {Math.Round(br.KillAge / 1000)}s ago)";

            var colors = new Dictionary<int, string>
            {
                {0, "darker"},
                {1, "darker"},
                {2, "darker"},
                {3, "lighter"},
                {4, "lighter"},
                {5, "lighter"},
                {6, "darker"},
                {7, "lighter"},
                {8, "darker"},
                {9, "darker"},
                {10, "lighter"},
                {11, "lighter"},
                {12, "darker"},
                {13, "darker"},
                {14, "lighter"},
                {15, "darker"},
                {16, "lighter"},
                {17, "lighter"},
                {18, "lighter"},
                {19, "lighter"},
                {20, "lighter"},
                {21, "lighter"}
            };
            var typeOfColor = colors[br.Killer.Data.ColorId];
            return
                $"Body Report: The killer appears to be a {typeOfColor} color. (Killed {Math.Round(br.KillAge / 1000)}s ago)";
        }
    }
}