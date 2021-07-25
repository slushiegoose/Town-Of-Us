using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.CrewmateRoles.TimeLordMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class RecordRewind
    {
        public static bool rewinding = false;
        public static TimeLord whoIsRewinding;
        public static List<PointInTime> points = new List<PointInTime>();
        private static float deadTime;
        private static bool isDead;
        private static float recordTime => CustomGameOptions.RewindDuration;

        public static void Record()
        {
            if (points.Count > Mathf.Round(recordTime / Time.deltaTime)) points.RemoveAt(points.Count - 1);

            if (PlayerControl.LocalPlayer == null) return;

            Vector3 position;
            Vector2 velocity;
            if (!PlayerControl.LocalPlayer.moveable && points.Count > 0)
            {
                position = points[0].position;
                velocity = Vector2.zero;
            }
            else
            {
                position = PlayerControl.LocalPlayer.transform.position;
                velocity = PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity;
            }


            points.Insert(0, new PointInTime(
                position,
                velocity,
                Time.time
            ));

            if (PlayerControl.LocalPlayer.Data.IsDead && !isDead)
            {
                isDead = true;
                deadTime = TempData.LastDeathReason == DeathReason.Exile ||
                           PlayerControl.LocalPlayer.Is(RoleEnum.Altruist)
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
            if (Minigame.Instance)
                try
                {
                    Minigame.Instance.Close();
                }
                catch
                {
                }
            //System.Console.WriteLine("Rewinding...");
            //System.Console.Write(points.Count);

            if (points.Count > 2)
            {
                points.RemoveAt(0);
                points.RemoveAt(0);
                if (PlayerControl.LocalPlayer.inVent)
                {
                    PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(Vent.currentVent.Id);
                    PlayerControl.LocalPlayer.MyPhysics.ExitAllVents();
                }
                
                if (!PlayerControl.LocalPlayer.inVent)
                {
                    if (!PlayerControl.LocalPlayer.Collider.enabled)
                    {
                        PlayerControl.LocalPlayer.MyPhysics.ResetMoveState();
                        PlayerControl.LocalPlayer.Collider.enabled = true;
                        PlayerControl.LocalPlayer.NetTransform.enabled = true;


                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte) CustomRPC.FixAnimation, SendOption.Reliable, -1);
                        writer.Write(PlayerControl.LocalPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }


                    var currentPoint = points[0];

                    PlayerControl.LocalPlayer.transform.position = currentPoint.position;
                    PlayerControl.LocalPlayer.gameObject.GetComponent<Rigidbody2D>().velocity =
                        currentPoint.velocity * 3;

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

            else
            {
                StartStop.StopRewind(whoIsRewinding);
            }
        }

        public static void ReviveBody(PlayerControl player)
        {
            player.Revive();
            Murder.KilledPlayers.Remove(
                Murder.KilledPlayers.FirstOrDefault(x => x.PlayerId == player.PlayerId));
            var body = Object.FindObjectsOfType<DeadBody>()
                .FirstOrDefault(b => b.ParentId == player.PlayerId);

            if (body != null)
                Object.Destroy(body.gameObject);
        }

        public static void Postfix()
        {
            if (rewinding)
                Rewind();
            else Record();

            foreach (var role in Role.GetRoles(RoleEnum.TimeLord))
            {
                var TimeLord = (TimeLord) role;
                if ((DateTime.UtcNow - TimeLord.StartRewind).TotalMilliseconds >
                    CustomGameOptions.RewindDuration * 1000f && TimeLord.FinishRewind < TimeLord.StartRewind)
                    StartStop.StopRewind(TimeLord);
            }
        }
    }
}