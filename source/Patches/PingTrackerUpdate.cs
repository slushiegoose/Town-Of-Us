using HarmonyLib;
using TownOfUs.CustomHats;
using UnityEngine;

namespace TownOfUs
{
    //[HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTracker_Update
    {
        private static string GenerateHatText()
        {
            HatCreation.HatData data;
            if (HatCreation.IdToData.ContainsKey(PlayerControl.LocalPlayer.Data.HatId))
                data = HatCreation.IdToData[PlayerControl.LocalPlayer.Data.HatId];
            else return "";

            /*if (data.name.Contains("trans"))
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
            }*/
            return data.author == "" ? $"\n{data.name} hat" : $"\n{data.name} hat by {data.author}";
        }

        [HarmonyPrefix]
        public static void Prefix(PingTracker __instance)
        {
            if (!__instance.GetComponentInChildren<SpriteRenderer>())
            {
                var spriteObject = new GameObject("Polus Sprite");
                spriteObject.AddComponent<SpriteRenderer>().sprite = TownOfUs.PolusSprite;
                spriteObject.transform.parent = __instance.transform;
                spriteObject.transform.localPosition = new Vector3(-1f, -0.3f, -1);
                spriteObject.transform.localScale *= 0.72f;
            }
        }

        [HarmonyPostfix]
        public static void Postfix(PingTracker __instance)
        {
            var position = __instance.GetComponent<AspectPosition>();
            position.DistanceFromEdge = new Vector3(3.1f, 0.1f, 0);
            position.AdjustPosition();

            __instance.text.text =
                "<color=#00FF00FF>TownOfUs v2.1.3</color>\n" +
                "Available on <color=#BEA4FFFF>Polus.gg</color>\n" +
                $"Ping: {AmongUsClient.Instance.Ping}ms\n"/* +
                (!MeetingHud.Instance
                    ? "<color=#00FF00FF>slushiegoose ft. edisonparklive</color>" +
                      GenerateHatText()
                    : "")*/;
        }
    }
}
