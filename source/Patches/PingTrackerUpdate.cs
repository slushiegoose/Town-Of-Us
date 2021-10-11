﻿using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    //[HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTracker_Update
    {
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
                $"<color=#00FF00FF>TownOfUs v{TownOfUs.Version}</color>\n" +
                "Available on <color=#BEA4FFFF>Polus.gg</color>\n" +
                $"Ping: {AmongUsClient.Instance.Ping}ms\n" +
                (!MeetingHud.Instance
                    ? "<color=#00FF00FF>slushiegoose ft. edisonparklive</color>"
                    : "");
        }
    }
}
