using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.TimeLordMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class RecordRewind
    {
        private static float recordTime => CustomGameOptions.RewindDuration;
        public static bool rewinding = false;
        public static TimeLord whoIsRewinding;
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
                deadTime = TempData.LastDeathReason == DeathReason.Exile
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
            if (Minigame.Instance) {
                try {
                    Minigame.Instance.Close();
                } catch {}
            }
            //System.Console.WriteLine("Rewinding...");
            //System.Console.Write(points.Count);
            if (points.Count > 2)
            {
                //PlayerControl.LocalPlayer.Physics.ExitAllVents
                if (!PlayerControl.LocalPlayer.inVent)
                {
                    points.RemoveAt(0);
                    points.RemoveAt(0);
                    var currentPoint = points[0];

                    PlayerControl.LocalPlayer.transform.position = currentPoint.position;
                    PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity = currentPoint.velocity * 3;

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
            
            else StartStop.StopRewind(whoIsRewinding);
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
            if (rewinding)
            {
                Rewind();
            } else Record();

            foreach (var role in Roles.Role.GetRoles(RoleEnum.TimeLord))
            {
                var TimeLord = (Roles.TimeLord) role;
                if ((DateTime.UtcNow - TimeLord.StartRewind).TotalMilliseconds >
                    CustomGameOptions.RewindDuration * 1000f && TimeLord.FinishRewind < TimeLord.StartRewind)
                {
                    StartStop.StopRewind(TimeLord);
                }
            }
        }
    
    }
}