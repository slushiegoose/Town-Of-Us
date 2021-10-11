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
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class RecordRewind
    {
        public static bool Rewinding = false;
        public static List<PointInTime> RewindPoints = new List<PointInTime>();
        public static float TimeLeft = float.MinValue;
        public static float RecordTime => CustomGameOptions.RewindDuration;

        private static float DeadTime;
        private static bool IsDead;

        public static void Record()
        {
            if (RewindPoints.Count > Mathf.Round(RecordTime / Time.deltaTime)) RewindPoints.RemoveAt(RewindPoints.Count - 1);

            var localPlayer = PlayerControl.LocalPlayer;

            if (localPlayer == null) return;

            Vector3 position;
            Vector2 velocity;
            if (!localPlayer.moveable && RewindPoints.Count > 0)
            {
                position = RewindPoints[0].Position;
                velocity = Vector2.zero;
            }
            else
            {
                position = localPlayer.transform.position;
                velocity = localPlayer.gameObject.GetComponent<Rigidbody2D>().velocity;
            }


            RewindPoints.Insert(0, new PointInTime {
                Position = position,
                Velocity = velocity,
                Unix = Time.time
            });

            if (localPlayer.Data.IsDead && !IsDead)
            {
                IsDead = true;
                DeadTime =
                    TempData.LastDeathReason == DeathReason.Exile ||
                    localPlayer.Is(RoleEnum.Altruist)
                        ? 0
                        : Time.time;
            }
            else if (!localPlayer.Data.IsDead && IsDead)
            {
                IsDead = false;
                DeadTime = 0;
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

            if (RewindPoints.Count > 2)
            {
                RewindPoints.RemoveAt(0);
                RewindPoints.RemoveAt(0);

                var localPlayer = PlayerControl.LocalPlayer;
                if (localPlayer.inVent)
                {
                    localPlayer.MyPhysics.RpcExitVent(Vent.currentVent.Id);
                    localPlayer.MyPhysics.ExitAllVents();
                }

                if (!localPlayer.Collider.enabled)
                {
                    localPlayer.MyPhysics.ResetMoveState();
                    localPlayer.Collider.enabled = true;
                    localPlayer.NetTransform.enabled = true;


                    var writer = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId,
                        (byte)CustomRPC.FixAnimation, SendOption.Reliable, -1);
                    writer.Write(localPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }


                var currentPoint = RewindPoints[0];

                localPlayer.transform.position = currentPoint.Position;
                localPlayer.gameObject.GetComponent<Rigidbody2D>().velocity =
                    currentPoint.Velocity * 3;

                if (IsDead && currentPoint.Unix < DeadTime && localPlayer.Data.IsDead &&
                    CustomGameOptions.RewindRevive)
                {
                    var player = PlayerControl.LocalPlayer;

                    ReviveBody(player);
                    player.myTasks.RemoveAt(0);

                    DeadTime = 0;
                    IsDead = false;

                    var write = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId,
                        (byte)CustomRPC.RewindRevive, SendOption.Reliable, -1);
                    write.Write(localPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(write);
                }

                RewindPoints.RemoveAt(0);
            }
            else
            {
                StartStop.StopRewind();
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

        public static void Postfix(PlayerControl __instance)
        {
            if (!__instance.AmOwner) return;

            if (Rewinding)
            {
                Rewind();
                TimeLeft = Mathf.Clamp(TimeLeft - Time.fixedDeltaTime, 0f, RecordTime);
                if (TimeLeft == 0f)
                    StartStop.StopRewind();
            }
            else
                Record();
        }
    }
}
