using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch(typeof(AirshipStatus), nameof(AirshipStatus.OnEnable))]
    public class DumbAirshipComms
    {
        public static void Postfix(AirshipStatus __instance)
        {
            var gObj = __instance.gameObject.transform.GetChild(4).GetChild(1);
            var console = gObj.GetComponent<Console>();
            console.AllowImpostor = true;
            console.GhostsIgnored = true;
        }
    }
}