using TownOfUs.CrewmateRoles.EngineerMod;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.CrewmateRoles.SeerMod;
using TownOfUs.CustomOption;
using TownOfUs.NeutralRoles.ExecutionerMod;
using TownOfUs.NeutralRoles.ShifterMod;

namespace TownOfUs
{
    public static class CustomGameOptions
    {
        public static int MayorOn => (int) Generate.MayorOn.Get();
        public static int JesterOn => (int) Generate.JesterOn.Get();
        public static int LoversOn => (int) Generate.LoversOn.Get();
        public static int SheriffOn => (int) Generate.SheriffOn.Get();
        public static int JanitorOn => (int) Generate.JanitorOn.Get();
        public static int EngineerOn => (int) Generate.EngineerOn.Get();
        public static int SwapperOn => (int) Generate.SwapperOn.Get();
        public static int ShifterOn => (int) Generate.ShifterOn.Get();
        public static int InvestigatorOn => (int) Generate.InvestigatorOn.Get();
        public static int TimeLordOn => (int) Generate.TimeLordOn.Get();
        public static int MedicOn => (int) Generate.MedicOn.Get();
        public static int SeerOn => (int) Generate.SeerOn.Get();
        public static int GlitchOn => (int) Generate.GlitchOn.Get();
        public static int MorphlingOn => (int) Generate.MorphlingOn.Get();
        public static int CamouflagerOn => (int) Generate.CamouflagerOn.Get();
        public static int ExecutionerOn => (int) Generate.ExecutionerOn.Get();
        public static int SpyOn => (int) Generate.SpyOn.Get();
        public static int SnitchOn => (int) Generate.SnitchOn.Get();
        public static int MinerOn => (int) Generate.MinerOn.Get();
        public static int SwooperOn => (int) Generate.SwooperOn.Get();
        public static int ArsonistOn => (int) Generate.ArsonistOn.Get();
        public static int AltruistOn => (int) Generate.AltruistOn.Get();
        public static int UndertakerOn => (int) Generate.UndertakerOn.Get();
        public static int AssassinOn => (int) Generate.AssassinOn.Get();
        public static int UnderdogOn => (int) Generate.UnderdogOn.Get();
        public static int PhantomOn => (int) Generate.PhantomOn.Get();
        public static int TorchOn => (int) Generate.TorchOn.Get();
        public static int DiseasedOn => (int) Generate.DiseasedOn.Get();
        public static int FlashOn => (int) Generate.FlashOn.Get();
        public static int TiebreakerOn => (int) Generate.TiebreakerOn.Get();
        public static int DrunkOn => (int) Generate.DrunkOn.Get();
        public static int BigBoiOn => (int) Generate.BigBoiOn.Get();
        public static int ButtonBarryOn => (int) Generate.ButtonBarryOn.Get();
        public static int VanillaGame => (int) Generate.VanillaGame.Get();
        public static bool BothLoversDie => Generate.BothLoversDie.Get();
        public static bool ShowSheriff => Generate.ShowSheriff.Get();
        public static bool SheriffKillOther => Generate.SheriffKillOther.Get();
        public static bool SheriffKillsJester => Generate.SheriffKillsJester.Get();
        public static bool SheriffKillsGlitch => Generate.SheriffKillsGlitch.Get();
        public static bool SheriffKillsArsonist => Generate.SheriffKillsArsonist.Get();
        public static float SheriffKillCd => Generate.SheriffKillCd.Get();
        public static int MayorVoteBank => (int) Generate.MayorVoteBank.Get();
        public static bool MayorAnonymous => Generate.MayorAnonymous.Get();
        public static float ShifterCd => Generate.ShifterCd.Get();
        public static ShiftEnum WhoShifts => (ShiftEnum) Generate.WhoShifts.Get();
        public static float FootprintSize => Generate.FootprintSize.Get();
        public static float FootprintInterval => Generate.FootprintInterval.Get();
        public static float FootprintDuration => Generate.FootprintDuration.Get();
        public static bool AnonymousFootPrint => Generate.AnonymousFootPrint.Get();
        public static bool VentFootprintVisible => Generate.VentFootprintVisible.Get();

        public static bool RewindRevive => Generate.RewindRevive.Get();
        public static float RewindDuration => Generate.RewindDuration.Get();
        public static float RewindCooldown => Generate.RewindCooldown.Get();
        public static bool TimeLordVitals => Generate.TimeLordVitals.Get();
        public static ShieldOptions ShowShielded => (ShieldOptions) Generate.ShowShielded.Get();

        public static NotificationOptions NotificationShield =>
            (NotificationOptions) Generate.WhoGetsNotification.Get();

        public static bool ShieldBreaks => Generate.ShieldBreaks.Get();
        public static float MedicReportNameDuration => Generate.MedicReportNameDuration.Get();
        public static float MedicReportColorDuration => Generate.MedicReportColorDuration.Get();
        public static bool ShowReports => Generate.MedicReportSwitch.Get();
        public static float SeerCd => Generate.SeerCooldown.Get();
        public static SeerInfo SeerInfo => (SeerInfo) Generate.SeerInfo.Get();
        public static SeeReveal SeeReveal => (SeeReveal) Generate.SeeReveal.Get();
        public static bool NeutralRed => Generate.NeutralRed.Get();
        public static float MimicCooldown => Generate.MimicCooldownOption.Get();
        public static float MimicDuration => Generate.MimicDurationOption.Get();
        public static float HackCooldown => Generate.HackCooldownOption.Get();
        public static float HackDuration => Generate.HackDurationOption.Get();
        public static float GlitchKillCooldown => Generate.GlitchKillCooldownOption.Get();
        public static float InitialGlitchKillCooldown => Generate.InitialGlitchKillCooldownOption.Get();
        public static int GlitchHackDistance => Generate.GlitchHackDistanceOption.Get();
        public static float MorphlingCd => Generate.MorphlingCooldown.Get();
        public static float MorphlingDuration => Generate.MorphlingDuration.Get();
        public static float CamouflagerCd => Generate.CamouflagerCooldown.Get();
        public static float CamouflagerDuration => Generate.CamouflagerDuration.Get();
        public static bool ColourblindComms => Generate.ColourblindComms.Get();
        public static bool MeetingColourblind => Generate.MeetingColourblind.Get();
        public static OnTargetDead OnTargetDead => (OnTargetDead) Generate.OnTargetDead.Get();
        public static bool SnitchOnLaunch => Generate.SnitchOnLaunch.Get();
        public static bool SnitchSeesNeutrals => Generate.SnitchSeesNeutrals.Get();
        public static float MineCd => Generate.MineCooldown.Get();
        public static float SwoopCd => Generate.SwoopCooldown.Get();
        public static float SwoopDuration => Generate.SwoopDuration.Get();
        public static bool ImpostorSeeRoles => Generate.ImpostorSeeRoles.Get();
        public static bool DeadSeeRoles => Generate.DeadSeeRoles.Get();
        public static float DouseCd => Generate.DouseCooldown.Get();
        public static bool ArsonistGameEnd => Generate.ArsonistGameEnd.Get();
        public static int MaxImpostorRoles => (int) Generate.MaxImpostorRoles.Get();
        public static int MaxNeutralRoles => (int) Generate.MaxNeutralRoles.Get();
        public static bool RoleUnderName => Generate.RoleUnderName.Get();
        public static EngineerFixPer EngineerFixPer => (EngineerFixPer) Generate.EngineerPer.Get();
        public static float ReviveDuration => Generate.ReviveDuration.Get();
        public static bool AltruistTargetBody => Generate.AltruistTargetBody.Get();
        public static bool SheriffBodyReport => Generate.SheriffBodyReport.Get();
        public static float DragCd => Generate.DragCooldown.Get();
        public static bool AssassinGuessNeutrals => Generate.AssassinGuessNeutrals.Get();
        public static bool AssassinCrewmateGuess => Generate.AssassinCrewmateGuess.Get();
        public static int AssassinKills => (int) Generate.AssassinKills.Get();
        public static bool AssassinMultiKill => Generate.AssassinMultiKill.Get();
    }
}