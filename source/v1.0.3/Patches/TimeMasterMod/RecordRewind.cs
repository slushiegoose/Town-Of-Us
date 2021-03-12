using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.TimeMasterMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class RecordRewind
    {
        private static float recordTime => CustomGameOptions.RewindDuration;
        public static bool rewinding = false;
        public static List<PointInTime> points = new List<PointInTime>();
        private static float deadTime;
        private static bool isDead = false;

        public static void Record()
        {
            if (points.Count > Mathf.Round(recordTime / Time.deltaTime))
            {
                points.RemoveAt(points.Count - 1);
            }

            if (PlayerControl.LocalPlayer == null) return;
            
            points.Insert(0, new PointInTime(
                PlayerControl.LocalPlayer.transform.position,
                PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity,
                Time.time
            ));

            if (PlayerControl.LocalPlayer.Data.IsDead && !isDead)
            {
                isDead = true;
                deadTime = TempData.LastDeathReason == DeathReason.Exile && !CustomGameOptions.ReviveExiled
                    ? 0
                    : Time.time;
            }
            else if (!PlayerControl.LocalPlayer.Data.IsDead && isDead)
            {
                isDead = false;
                deadTime = 0;
            }

        }

        public static void Rewind()
        {
            System.Console.WriteLine("Rewinding...");
            System.Console.Write(points.Count);
            if (points.Count > 2)
            {
                //PlayerControl.LocalPlayer.Physics.ExitAllVents
                if (!PlayerControl.LocalPlayer.inVent)
                {
                    points.RemoveAt(0);
                    points.RemoveAt(0);
                    var currentPoint = points[0];

                    PlayerControl.LocalPlayer.transform.position = currentPoint.position;
                    PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity = currentPoint.velocity;

                    if (isDead && currentPoint.unix < deadTime && PlayerControl.LocalPlayer.Data.IsDead &&
                        CustomGameOptions.RewindRevive)
                    {
                        var player = PlayerControl.LocalPlayer;

                        ReviveBody(player);
                        player.myTasks.RemoveAt(0);

                        deadTime = 0;
                        isDead = false;

                        var write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte) CustomRPC.RewindRevive, SendOption.Reliable, -1);
                        write.Write(PlayerControl.LocalPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(write);
                    }
                }
                points.RemoveAt(0);
            }
            
            else StartStop.StopRewind();
        }

        public static void ReviveBody(PlayerControl player)
        {
            player.Revive();
            var body = Object.FindObjectsOfType<DeadBody>()
                .FirstOrDefault(b => b.ParentId == player.PlayerId);

            if (body != null)
                Object.Destroy(body.gameObject);
        }

        public static void Postfix()
        {
            if (Utils.TimeMaster == null) return;
            if (rewinding)
            {
                Rewind();
            } else Record();

            if ((DateTime.UtcNow - Methods.StartRewind).TotalMilliseconds > CustomGameOptions.RewindDuration * 1000f && Methods.FinishRewind < Methods.StartRewind)
            {
                StartStop.StopRewind();
            }
        }
    
    }
}