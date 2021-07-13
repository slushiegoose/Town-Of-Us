using System;
using TownOfUs.CrewmateRoles.TimeLordMod;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class TimeLord : Role
    {
        public TimeLord(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Rewind Time";
            TaskText = () => "Rewind Time!";
            RoleType = RoleEnum.TimeLord;
            Scale = 1.4f;
        }

        public DateTime StartRewind { get; set; }
        public DateTime FinishRewind { get; set; }

        public float TimeLordRewindTimer()
        {
            var utcNow = DateTime.UtcNow;


            TimeSpan timespan;
            float num;

            if (RecordRewind.rewinding)
            {
                timespan = utcNow - StartRewind;
                num = CustomGameOptions.RewindDuration * 1000f / 3f;
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
            return RecordRewind.rewinding ? CustomGameOptions.RewindDuration : CustomGameOptions.SheriffKillCd;
        }
    }
}
