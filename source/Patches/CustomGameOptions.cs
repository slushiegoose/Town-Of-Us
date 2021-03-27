namespace TownOfUs
{
    public static class CustomGameOptions
    {
        public static int MayorOn => (int) TownOfUs.MayorOn.GetValue();
        public static int JesterOn => (int) TownOfUs.JesterOn.GetValue();
        public static int LoversOn => (int) TownOfUs.LoversOn.GetValue();
        public static int SheriffOn => (int) TownOfUs.SheriffOn.GetValue();
        public static int MafiaOn => (int) TownOfUs.MafiaOn.GetValue();
        public static int EngineerOn => (int) TownOfUs.EngineerOn.GetValue();
        public static int SwapperOn => (int) TownOfUs.SwapperOn.GetValue();
        public static int ShifterOn => (int) TownOfUs.ShifterOn.GetValue();
        public static int InvestigatorOn => (int) TownOfUs.InvestigatorOn.GetValue();
        public static int TimeLordOn => (int) TownOfUs.TimeLordOn.GetValue();
        public static int MedicOn => (int) TownOfUs.MedicOn.GetValue();
        public static int SeerOn => (int) TownOfUs.SeerOn.GetValue();
        public static int GlitchOn => (int) TownOfUs.GlitchOn.GetValue();
        public static int MorphlingOn => (int) TownOfUs.MorphlingOn.GetValue();
        public static int CamouflagerOn => (int) TownOfUs.CamouflagerOn.GetValue();
        public static int ExecutionerOn => (int) TownOfUs.ExecutionerOn.GetValue();
        public static int ChildOn => (int) TownOfUs.ChildOn.GetValue(); 
        public static int SpyOn => (int) TownOfUs.SpyOn.GetValue(); 
            
        public static int TorchOn => (int) TownOfUs.TorchOn.GetValue();
        public static int DiseasedOn => (int) TownOfUs.DiseasedOn.GetValue();
        public static int FlashOn => (int) TownOfUs.FlashOn.GetValue();
        
        public static bool BothLoversDie => TownOfUs.BothLoversDie.GetValue();
        public static bool ShowSheriff => TownOfUs.ShowSheriff.GetValue();
        public static bool SheriffKillOther => TownOfUs.SheriffKillOther.GetValue();
        public static bool SheriffKillsJester => TownOfUs.SheriffKillsJester.GetValue();
        public static float SheriffKillCd => TownOfUs.SheriffKillCd.GetValue();
        public static float JanitorCleanCd => TownOfUs.JanitorCleanCd.GetValue();
        public static bool TwoMafia => TownOfUs.TwoMafia.GetValue();
        public static bool JanitorKill => TownOfUs.JanitorKill.GetValue();
        public static int MayorVoteBank => (int) TownOfUs.MayorVoteBank.GetValue();
        public static float ShifterCd => TownOfUs.ShifterCd.GetValue();
        
        public static float FootprintSize => TownOfUs.FootprintSize.GetValue();
        public static float FootprintInterval => TownOfUs.FootprintInterval.GetValue();
        public static float FootprintDuration => TownOfUs.FootprintDuration.GetValue();
        public static bool AnonymousFootPrint => TownOfUs.AnonymousFootPrint.GetValue();
        public static bool VentFootprintVisible => TownOfUs.VentFootprintVisible.GetValue();
        
        public static bool RewindRevive => TownOfUs.RewindRevive.GetValue();
        public static float RewindDuration  => TownOfUs.RewindDuration.GetValue();
        public static float RewindCooldown => TownOfUs.RewindCooldown.GetValue();
        public static bool TimeLordVitals => TownOfUs.TimeLordVitals.GetValue();


        public static MedicMod.ShieldOptions ShowShielded => (MedicMod.ShieldOptions) TownOfUs.ShowShielded.GetValue();

        public static float MedicReportNameDuration => TownOfUs.MedicReportNameDuration.GetValue();
        public static float MedicReportColorDuration => TownOfUs.MedicReportColorDuration.GetValue();
        public static bool ShowReports => TownOfUs.MedicReportSwitch.GetValue();
        public static bool PlayerMurderIndicator => TownOfUs.PlayerMurderIndicator.GetValue();

        public static float SeerCd => TownOfUs.SeerCooldown.GetValue();
        public static SeerMod.SeerInfo SeerInfo => (SeerMod.SeerInfo) TownOfUs.SeerInfo.GetValue();
        public static SeerMod.SeeReveal SeeReveal => (SeerMod.SeeReveal) TownOfUs.SeeReveal.GetValue();
        public static bool NeutralRed => TownOfUs.NeutralRed.GetValue();
        public static float MimicCooldown => TownOfUs.MimicCooldownOption.GetValue();
        public static float MimicDuration => TownOfUs.MimicDurationOption.GetValue();
        public static float HackCooldown => TownOfUs.HackCooldownOption.GetValue();
        public static float HackDuration => TownOfUs.HackDurationOption.GetValue();
        public static float GlitchKillCooldown => TownOfUs.GlitchKillCooldownOption.GetValue();
        public static float InitialGlitchKillCooldown => TownOfUs.InitialGlitchKillCooldownOption.GetValue();
        public static int GlitchHackDistance => TownOfUs.GlitchHackDistanceOption.GetValue();

        public static float MorphlingCd => TownOfUs.MorphlingCooldown.GetValue();
        public static float MorphlingDuration => TownOfUs.MorphlingDuration.GetValue();
        
        public static float CamouflagerCd => TownOfUs.CamouflagerCooldown.GetValue();
        public static float CamouflagerDuration => TownOfUs.CamouflagerDuration.GetValue();
        public static bool ColourblindComms => TownOfUs.ColourblindComms.GetValue();
        public static bool MeetingColourblind => TownOfUs.MeetingColourblind.GetValue();

        public static ExecutionerMod.OnTargetDead OnTargetDead =>
            (ExecutionerMod.OnTargetDead) TownOfUs.OnTargetDead.GetValue();
    }
}