using System;

namespace TownOfUs.Roles
{
    public class Janitor : Role
    {
        public Godfather Godfather {get; set;}
        public Mafioso Mafioso {get; set;}

        public DateTime LastCleaned {get; set;}
        public DeadBody CurrentTarget {get; set;}
        
        protected internal override string NameText()
        {
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed && !MeetingHud.Instance)
            {
                return "";
            }
            return Player.name + " (J)";
        }
        
        protected override bool Criteria()
        {
            var localPlayerRole = GetRole(PlayerControl.LocalPlayer);

            return localPlayerRole == this || localPlayerRole == Godfather || !CustomGameOptions.TwoMafia && localPlayerRole == Mafioso;

        }
        
        public Janitor(PlayerControl player) : base(player)
        {
            Name = "Janitor";
            ImpostorText = () => "Clean up bodies";
            TaskText = () => "Clean bodies to prevent Crewmates from discovering them.";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Janitor;
            Faction = Faction.Impostors;
        }
        
        public float JanitorTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastCleaned;
            var num = CustomGameOptions.JanitorCleanCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}