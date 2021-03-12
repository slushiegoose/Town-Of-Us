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
        public static int TimeMasterOn => (int) TownOfUs.TimeMasterOn.GetValue();

        public static bool BothLoversDie => TownOfUs.BothLoversDie.GetValue();
        public static bool ShowSheriff => TownOfUs.ShowSheriff.GetValue();
        public static float SheriffKillCd => TownOfUs.SheriffKillCd.GetValue();
        public static float JanitorCleanCd => TownOfUs.JanitorCleanCd.GetValue();
        public static float MayorVision => TownOfUs.MayorVision.GetValue();
        public static float EngineerVision => TownOfUs.EngineerVision.GetValue();
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
        public static bool ReviveExiled => TownOfUs.ReviveExiled.GetValue();
    }
}