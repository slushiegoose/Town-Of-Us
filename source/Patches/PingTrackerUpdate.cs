using System.Linq;
using HarmonyLib;
using TownOfUs.CustomHats;

namespace TownOfUs {

    //[HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTrackerUpdate
    {

        private static string GenerateHatText(HatCreation.HatData data)
        {
            if (data.name.Contains("trans"))
            {
                return
                    "\n[5BCEFAFF]trans [F5A9B8FF]hat []by The[F5A9B8FF]Last[5BCEFAFF]Shaymin";
            }

            if (data.name.Contains("asexual"))
            {
                return
                    "\n[454545FF]asexual [A3A3A3FF]hat by []TheLast[8D608BFF]Shaymin";
            }

            if (data.name.Contains("bisexual"))
            {
                return
                    "\n[FF87C1FF]bisexual hat [8D608BFF]by [4381FFFF]TheLastShaymin";
            }

            if (data.name.Contains("gay"))
            {
                return
                    "\n[FF5555FF]gay [FFAA55FF]hat [FFFF55FF]by [55FF55FF]The[5555FFFF]Last[FF55FFFF]Shaymin";
            }

            if (data.name.Contains("nonbinary"))
            {
                return
                    "\n[FFFF00FF]non-binary []hat by [8D608BFF]TheLast[454545FF]Shaymin";
            }

            if (data.name.Contains("pansexual"))
            {
                return
                    "\n[FF54A6FF]pansexual [FFD800FF]hat by [21B1FFFF]TheLastShaymin";
            }
            
            return $"\n{data.name} hat by {data.author}";
        }
        
        
        public static void Postfix(PingTracker __instance)
        {
            //__instance.text.Text += "\n> [FFBFCCFF]T[FF80D5FF]o[FFCC00FF]w[704FA8FF]n[FF0000FF] of[CC4D00FF] Us [FFFFFFFF] <\nBy [00FF00FF]slushiegoose[FFFFFFFF]";
            __instance.text.Text += "\n[00FF00FF]TownOfUs\nv2.0.0\nslushiegoose ft. edisonparklive[]";
            if (!MeetingHud.Instance)
            {
                __instance.text.Text += "\nButton Art by PhasmoFireGod";
                if (HatCreation.IdToData.ContainsKey(PlayerControl.LocalPlayer.Data.HatId))
                {
                    var data = HatCreation.IdToData[PlayerControl.LocalPlayer.Data.HatId];


                    __instance.text.Text += GenerateHatText(data);
                }
            }
        }
        
    }
}