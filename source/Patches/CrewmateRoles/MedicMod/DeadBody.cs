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
                {0, "darker"},// red
                {1, "darker"},// blue
                {2, "darker"},// green
                {3, "lighter"},// pink
                {4, "lighter"},// orange
                {5, "lighter"},// yellow
                {6, "darker"},// black
                {7, "lighter"},// white
                {8, "darker"},// purple
                {9, "darker"},// brown
                {10, "lighter"},// cyan
                {11, "lighter"},// lime
                {12, "darker"},// maroon
                {13, "lighter"},// rose
                {14, "lighter"},// banana
                {15, "lighter"},// gray
                {16, "darker"},// tan
                {17, "lighter"},// coral
                {18, "darker"},// watermelon
                {19, "darker"},// chocolate
                {20, "lighter"},// sky blue
                {21, "darker"},// beige
                {22, "lighter"},// hot pink
                {23, "lighter"},// turquoise
                {24, "lighter"},// lilac
                {25, "lighter"},// rainbow
                {26, "lighter"},// azure
            };
            var typeOfColor = colors[br.Killer.Data.ColorId];
            return
                $"Body Report: The killer appears to be a {typeOfColor} color. (Killed {Math.Round(br.KillAge / 1000)}s ago)";
        }
    }
}
