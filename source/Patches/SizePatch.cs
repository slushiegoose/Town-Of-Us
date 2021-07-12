using HarmonyLib;
using TownOfUs.Extensions;

namespace TownOfUs
{
    [HarmonyPatch]
    public static class SizePatch
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls.ToArray())
            {
                player.transform.localScale = player.GetAppearance().SizeFactor;
            }

            // This was previously commented out, so I converted it and left it disabled.
            //var playerBindings = PlayerControl.AllPlayerControls.ToArray().ToDictionary(player => player.PlayerId);
            //var bodies = Object.FindObjectsOfType<DeadBody>();
            //foreach (var body in bodies)
            //{
            //    body.transform.localScale = playerBindings[body.ParentId].GetAppearance().SizeFactor;
            //}
        }
    }
}
