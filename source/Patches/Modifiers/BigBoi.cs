using HarmonyLib;

namespace TownOfUs.Modifiers
{
    public class BigBoi
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls.ToArray())
            {
                if (player.Is(ModifierEnum.BigBoi))
                {
                    player.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
                    break;
                }
            }
            /*var bodies = Object.FindObjectsOfType<DeadBody>();
            foreach (var body in bodies)
            {
                System.Console.WriteLine(body.ParentId);
                var player = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == body.ParentId);
                if (player != null)
                {
                    if (player.Is(ModifierEnum.BigBoi))
                    {
                        body.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                    }
                }
            }*/
        }

        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        public static class PlayerPhysics_FixedUpdate
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                if (__instance.myPlayer.Is(ModifierEnum.BigBoi))
                    if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
                        __instance.body.velocity = __instance.body.velocity / 2;
            }
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        public static class CustomNetworkTransform_FixedUpdate
        {
            public static void Postfix(CustomNetworkTransform __instance)
            {
                if (!__instance.AmOwner)
                    if (__instance.interpolateMovement != 0f)
                        if (__instance.gameObject.GetComponent<PlayerControl>().Is(ModifierEnum.BigBoi))
                            __instance.body.velocity = __instance.body.velocity / 2;
            }
        }
    }
}