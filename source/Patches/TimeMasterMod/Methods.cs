using System;

namespace TownOfUs.TimeMasterMod
{
    public class Methods
    {

        public static DateTime StartRewind;
        public static DateTime FinishRewind;

        public static float TimeMasterRewindTimer()
        {
            var utcNow = DateTime.UtcNow;


            TimeSpan timespan;
            float num;

            if (RecordRewind.rewinding)
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


        public static float GetCooldown()
        {
            return RecordRewind.rewinding ? CustomGameOptions.RewindDuration : CustomGameOptions.SheriffKillCd;
        }
    }
}