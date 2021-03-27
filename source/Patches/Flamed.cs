using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(KillOverlay.Nested_4), nameof(KillOverlay.Nested_4.MoveNext))]
    public class Flamed
    {
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
                }
            }
        }

        private static MurderEnum GetOption(GameData.PlayerInfo killer, GameData.PlayerInfo victim)
        {
            if (killer.Object.isSheriff()) return MurderEnum.Sheriff;
            if (victim.Object.isShifter() && (!killer.IsImpostor || killer.Object.isShifter())) return MurderEnum.Shifter;
            if (killer.Object.isLover() && victim.Object.isLover() && killer.PlayerId == victim.PlayerId) return MurderEnum.Lover;
            return MurderEnum.Normal;
        }
    }
}