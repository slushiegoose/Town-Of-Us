using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hazel;
using TownOfUs.Extensions;

namespace TownOfUs.Roles
{
    public class Miner : Role
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
                    );
                }
                return _VentSize;
            }
        }

        public Miner(PlayerControl player) : base(player)
        {
            Name = "Miner";
            ImpostorText = () => "From the top, make it drop, that's a vent";
            TaskText = () => ImpostorText();
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Miner;
            Faction = Faction.Impostors;
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
                    Position = TOUConstants.OverKillbutton,
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
                    (layer == 8 || layer == 5) &&
                    !collider.isTrigger &&
                    !collider.name.Contains("Vent");
            });
        }

        public void SpawnVent(Vector2 position, int id)
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

            var ventPos = newVent.transform.position = new Vector3(
                position.x,
                position.y,
                0.1f
            );

            var _newVents = shipStatus.AllVents.ToList();
            _newVents.Add(newVent);
            shipStatus.AllVents = _newVents.ToArray();
            Vents.Add(newVent);
        }

        public void MineCallback()
        {
            var ventPos = (Vector2) Player.transform.position;
            var ventId = ShipStatus.Instance.AllVents.Count;
            SpawnVent(ventPos, ventId);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Mine, SendOption.Reliable, -1);
            writer.Write(ventId);
            writer.Write(ventPos);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}
