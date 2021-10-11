using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hazel;
using TownOfUs.Extensions;

namespace TownOfUs.Roles
{
    public class Miner : Impostor
    {
        private Vector2 _VentSize;
        public readonly List<Vent> Vents = new List<Vent>();
        // hacky wacky
        public Vector2 VentSize
        {
            get
            {
                if (_VentSize == null)
                {
                    var vent = Object.FindObjectOfType<Vent>();
                    _VentSize = Vector2.Scale(
                        vent.GetComponent<BoxCollider2D>().size,
                        vent.transform.localScale
                    ) * 0.9f;
                }
                return _VentSize;
            }
        }

        public Miner(PlayerControl player) : base(player)
        {
            ImpostorText = () => "From the top, make it drop, that's a vent";
            TaskText = () => ImpostorText();
            RoleType = RoleEnum.Miner;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlainAbilityData
                {
                    Callback = MineCallback,
                    MaxTimer = CustomGameOptions.MineCd,
                    Icon = TownOfUs.MineSprite,
                    Position = AbilityPositions.OverKillButton,
                    IsHighlighted = CanPlace
                });
            }
        }

        public bool CanPlace()
        {
            return _VentSize != null && Physics2D.OverlapBoxAll(Player.transform.position, VentSize, 0).All(collider =>
            {
                var layer = collider.gameObject.layer;

                return
                    layer != 8 && layer != 5 &&
                    collider.isTrigger && !collider.name.Contains("Vent");
            });
        }

        public void SpawnVent(Vector3 ventPos, int id, PlainShipRoom room)
        {
            var shipStatus = ShipStatus.Instance;

            var prefab = Object.FindObjectOfType<Vent>();
            var newVent = Object.Instantiate(prefab, prefab.transform.parent);
            var ventId = newVent.Id = id;

            if (Vents.Count == 0)
                newVent.Left = null;
            else
            {
                var left = Vents[^1];
                newVent.Left = left;
                left.Right = newVent;
            }
            newVent.Right = null;

            newVent.transform.position = ventPos;

            if (room != null)
            {
                newVent.transform.parent = room.transform;
                //if (room.transform.position.z > 0)
                //    newVent.transform.position = new Vector3(ventPos.x, ventPos.y, ventPos.z);
            }

            var _newVents = shipStatus.AllVents.ToList();
            _newVents.Add(newVent);
            shipStatus.AllVents = _newVents.ToArray();
            Vents.Add(newVent);
        }

        private Vector3 OffsetVector(Vector3 vector3, float z)
            => new Vector3(vector3.x, vector3.y, vector3.z + z);
        public void MineCallback()
        {
            var ventPos = OffsetVector(Player.transform.position, 0.001f);
            var ventId = ShipStatus.Instance.AllVents.Count;
            var room = HudManager.Instance.roomTracker.LastRoom;

            SpawnVent(ventPos, ventId, room);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Mine, SendOption.Reliable, -1);
            writer.Write(ventId);
            writer.Write(ventPos);
            writer.Write(room == null ? byte.MaxValue : (byte)room.RoomId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}
