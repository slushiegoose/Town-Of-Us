using System;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Camouflager : Role

    {
        public KillButtonManager _camouflageButton;
        public bool Camouflaged => TimeRemaining > 0f;
        public float TimeRemaining = 0f;
        public DateTime LastCamouflaged;
        public bool Enabled;
        
        public KillButtonManager CamouflageButton
        {
            get { return _camouflageButton;}
            set
            {
                _camouflageButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }

        public Camouflager(PlayerControl player) : base(player)
        {
            Name = "Camouflager";
            ImpostorText = () => "Camouflage and turn everyone grey";
            TaskText = () => "Camouflage and get secret kills";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Camouflager;
            Faction = Faction.Impostors;
        }

        public void Camouflage()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            Utils.Camouflage();
        }

        public void UnCamouflage()
        {
            Enabled = false;
            LastCamouflaged = DateTime.UtcNow;
            Utils.UnCamouflage();
        }
        
        public float CamouflageTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastCamouflaged;
            var num = CustomGameOptions.CamouflagerCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }


    }
}