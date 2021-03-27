using System.Linq;
using UnhollowerBaseLib;

namespace TownOfUs.LoversMod
{
    public class Methods
    {
        public static bool CheckNoImpsNoCrews()
        {
            var alives = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead).ToList();
            if (alives.Count == 2)
            {
                var jester = alives.Where(Utils.isJester).Any();
                var shifter = alives.Where(Utils.isShifter).Any();
                return jester && shifter;
            }

            return false;
        }

        public static bool FourPeopleLeft()
        {
            var players = PlayerControl.AllPlayerControls.ToArray();
            var alives = players.Where(x => !x.Data.IsDead);
            var lovers = players.Where(Utils.isLover);


            return lovers.Count() >= 2 && !Utils.Lover1.Data.IsDead && !Utils.Lover2.Data.IsDead &&
                   alives.Count() == 4 && Utils.LoverImpostor;
        }

        public static bool CheckLoversWin()
        {
            var players = PlayerControl.AllPlayerControls.ToArray();
            var alives = players.Where(x => !x.Data.IsDead).ToList();
            var lovers = players.Where(Utils.isLover);

            return lovers.Count() >= 2 && !Utils.Lover1.Data.IsDead && !Utils.Lover2.Data.IsDead &&
                   alives.Count == 3 | alives.Count == 2;
        }
        
        public static void LoversWin()
        {
            if(JesterMod.EndCriteria.JesterVotedOut)
            {
                return;
            }
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.Data.IsImpostor = player.isLover();
            }

            EndCriteria.LoveCoupleWins = true;
        }
        
        public static void NobodyWins()
        {
            if(JesterMod.EndCriteria.JesterVotedOut)
            {
                return;
            }
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.Data.IsImpostor = false;
            }

            EndCriteria.NobodyWins = true;
        }
    }
}