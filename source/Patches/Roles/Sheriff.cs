using System;

namespace TownOfUs.Roles
{
    public class Sheriff : Role
    {
        
        public PlayerControl ClosestPlayer { get; set;}
        public DateTime LastKilled { get; set;}
        
        public Sheriff(PlayerControl player) : base(player)
        {
            Name = "Sheriff";
            ImpostorText = () => "Shoot the [FF0000FF]Impostor";
            TaskText = () => "Kill off the impostor but don't kill crewmates.";
            Color = UnityEngine.Color.yellow;
            RoleType = RoleEnum.Sheriff;
        }
        
        public float SheriffKillTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastKilled;
            var num = CustomGameOptions.SheriffKillCd * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        protected override bool Criteria()
        {
            return CustomGameOptions.ShowSheriff || base.Criteria();
        }
        
        
        
    }
}