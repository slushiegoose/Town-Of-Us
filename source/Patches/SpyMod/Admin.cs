using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SpyMod
{

    public class Admin
    {
        public static Dictionary<SystemTypes, List<byte>> AllRooms = new Dictionary<SystemTypes, List<byte>>();

        [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
        public static class MapCountOverlay_Update
        {
            public static bool Prefix(MapCountOverlay __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Spy)) return true;
                __instance.timer += Time.deltaTime;
                if (__instance.timer < 0.1f)
                {
                    return false;
                }

                __instance.timer = 0f;
                AllRooms = new Dictionary<SystemTypes, List<byte>>();
                var task = PlayerControl.LocalPlayer.myTasks.ToArray()
                    .FirstOrDefault(x => x.TaskType == TaskTypes.FixComms);
                var flag = task != null;
                if (!__instance.isSab && flag)
                {
                    __instance.isSab = true;
                    __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                    __instance.SabotageText.gameObject.SetActive(true);
                    return false;
                }
                if (__instance.isSab && !flag)
                {
                    __instance.isSab = false;
                    __instance.BackgroundColor.SetColor(Color.green);
                    __instance.SabotageText.gameObject.SetActive(false);
                }
                for (int i = 0; i < __instance.CountAreas.Length; i++)
                {
                    CounterArea counterArea = __instance.CountAreas[i];
                    var colourList = new List<byte>();
                    AllRooms.Add(counterArea.RoomType, colourList);

                    if (!PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(PlayerControl.LocalPlayer))
                    {
                        PlainShipRoom plainShipRoom;
                        if (ShipStatus.Instance.FastRooms.ContainsKey(counterArea.RoomType))
                        {
                            plainShipRoom = ShipStatus.Instance.FastRooms[counterArea.RoomType];
                            if (!plainShipRoom.roomArea) continue;

                            int num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                            int num2 = num;
                            for (int j = 0; j < num; j++)
                            {
                                Collider2D collider2D = __instance.buffer[j];
                                if (!(collider2D.tag == "DeadBody"))
                                {
                                    PlayerControl component = collider2D.GetComponent<PlayerControl>();
                                    if (!component || component.Data == null || component.Data.Disconnected ||
                                        component.Data.IsDead)
                                    {
                                        num2--;
                                    }
                                    else
                                    {
                                        colourList.Add(component.Data.ColorId);
                                    }
                                }
                            }

                            counterArea.UpdateCount(num2);
                        }
                        else
                        {
                            Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
                        }
                    }
                    else
                    {
                        counterArea.UpdateCount(0);
                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(CounterArea), nameof(CounterArea.UpdateCount))]
        public static class CounterArea_UpdateCount
        {
            public static void Postfix(CounterArea __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Spy)) return;
                if (!AllRooms.ContainsKey(__instance.RoomType)) return;
                var list = AllRooms[__instance.RoomType];
                for (int i = 0; i < __instance.myIcons.Count; i++)
                {
                    var poolable = __instance.myIcons[i];
                    var rend = poolable.GetComponent<SpriteRenderer>();
                    if (rend != null && list.Count > i)
                    {
    
                        PlayerControl.SetPlayerMaterialColors(list[i], rend);
                    }
                }
            }
        }
    }
}