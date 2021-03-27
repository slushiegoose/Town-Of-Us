using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Miner : Role
    {

        public KillButtonManager _mineButton;
        public DateTime LastMined;
        public readonly List<Vent> Vents = new List<Vent>();
        public bool CanPlace { get; set; }
        public Vector2 VentSize { get; set; }
        
        public KillButtonManager MineButton
        {
            get { return _mineButton;}
            set
            {
                _mineButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }



        public Miner(PlayerControl player) : base(player)
        {
            Name = "Miner";
            ImpostorText = () => "Place vents down to help the Impostors";
            TaskText = () => "Create vents to help the impostors";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Miner;
            Faction = Faction.Impostors;
        }

        public float MineTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastMined;
            var num = CustomGameOptions.MineCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}