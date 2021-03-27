using System;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class TimeLord : Role
    {

        public DateTime StartRewind { get; set;}
        public DateTime FinishRewind { get; set;}
        
        public TimeLord(PlayerControl player) : base(player)
        {
            Name = "Time Lord";
            ImpostorText = () => "Rewind Time";
            TaskText = () => "Rewind Time!";
            Color = new Color(0f, 0f, 1f, 1f);
            RoleType = RoleEnum.TimeLord;
            Scale = 1.4f;
        }
        
        public float TimeLordRewindTimer()
        {
            var utcNow = DateTime.UtcNow;


            TimeSpan timespan;
            float num;

            if (TimeLordMod.RecordRewind.rewinding)
            {
                timespan = utcNow - StartRewind;
                num = (CustomGameOptions.RewindDuration * 1000f)/3f;
            }
            else
            {
                timespan = utcNow - FinishRewind;
                num = CustomGameOptions.RewindCooldown * 1000f;
            }


            var flag2 = num - (float) timespan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timespan.TotalMilliseconds) / 1000f;
        }


        public float GetCooldown()
        {
            return TimeLordMod.RecordRewind.rewinding ? CustomGameOptions.RewindDuration : CustomGameOptions.SheriffKillCd;
        }
    }
}