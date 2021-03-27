// This doesn't work in the new mappings cos of the weird override in classes so as soon as this works i'll add it back

/*using System.Reflection;
using HarmonyLib;
using Reactor.Extensions;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(KillOverlay.Nested_4), nameof(KillOverlay.Nested_4.MoveNext))]
    public class Flamed
    {
        public static MethodBase TargetMethod()
        {
            typeof(KillOverlay).GetNestedType("NLJFBGBIAFG").GetMethod("MoveNext");
        }
        public static void Prefix(KillOverlay.Nested_4 __instance)
        {
            var prefab = __instance.killAnimPrefab;
            var renderer = __instance.__this.flameParent.transform.GetChild(0).gameObject
                .GetComponent<SpriteRenderer>();
            if (prefab == __instance.__this.ReportOverlay || prefab == __instance.__this.EmergencyOverlay)
            {
                renderer.color = Color.white;
                renderer.sprite = TownOfUs.NormalKill;
            }
            else
            {
                var option = GetOption(__instance.killer, __instance.victim);
                switch (option)
                {
                    case MurderEnum.Normal:
                        renderer.color = Color.white;
                        renderer.sprite = TownOfUs.NormalKill;
                        break;
                    case MurderEnum.Sheriff:
                        renderer.color = Color.yellow;
                        renderer.sprite = TownOfUs.GreyscaleKill;
                        break;
                    case MurderEnum.Shifter:
                        renderer.sprite = TownOfUs.GreyscaleKill;
                        renderer.color = Color.white;
                        break;
                    case MurderEnum.Lover:
                        renderer.sprite = TownOfUs.GreyscaleKill;
                        renderer.color = new Color(1f, 0.4f, 0.8f);
                        break;
                    case MurderEnum.Glitch:
                        renderer.sprite = TownOfUs.GreyscaleKill;
                        renderer.color = Color.green;
                        break;
                }
            }
        }
        

        private static MurderEnum GetOption(GameData.PlayerInfo killer, GameData.PlayerInfo victim)
        {
            if (victim.Object.Is(RoleEnum.Shifter) && (killer.Object.Is(RoleEnum.Shifter)))
                return MurderEnum.Shifter;
            if (killer.Object.Is(RoleEnum.Sheriff)) return MurderEnum.Sheriff;
            if (killer.Object.isLover() && victim.Object.isLover() && killer.PlayerId == victim.PlayerId) return MurderEnum.Lover;
            if (killer.Object.Is(RoleEnum.Glitch)) return MurderEnum.Glitch;
            return MurderEnum.Normal;
            }
    }
}*/