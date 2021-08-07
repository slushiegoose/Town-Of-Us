using System;
using SeerRevealInfo = TownOfUs.CrewmateRoles.SeerMod.SeerInfo;
using ExecTargetDead = TownOfUs.NeutralRoles.ExecutionerMod.OnTargetDead;

namespace TownOfUs.CustomOption
{
    public class Generate
    {
        #region Crewmate Roles
        public static CustomHeaderOption CrewmateRoles;
        public static CustomNumberOption MayorOn;
        public static CustomNumberOption LoversOn;
        public static CustomNumberOption SheriffOn;
        public static CustomNumberOption EngineerOn;
        public static CustomNumberOption SwapperOn;
        public static CustomNumberOption InvestigatorOn;
        public static CustomNumberOption TimeLordOn;
        public static CustomNumberOption MedicOn;
        public static CustomNumberOption SeerOn;
        public static CustomNumberOption SpyOn;
        public static CustomNumberOption SnitchOn;
        public static CustomNumberOption AltruistOn;
        public static CustomNumberOption ButtonBarryOn;
        #endregion

        #region Neutral Roles
        public static CustomHeaderOption NeutralRoles;
        public static CustomNumberOption JesterOn;
        public static CustomNumberOption ShifterOn;
        public static CustomNumberOption GlitchOn;
        public static CustomNumberOption ExecutionerOn;
        public static CustomNumberOption ArsonistOn;
        public static CustomNumberOption PhantomOn;
        #endregion

        #region Impostor Roles
        public static CustomHeaderOption ImpostorRoles;
        public static CustomNumberOption JanitorOn;
        public static CustomNumberOption MorphlingOn;
        public static CustomNumberOption CamouflagerOn;
        public static CustomNumberOption MinerOn;
        public static CustomNumberOption SwooperOn;
        public static CustomNumberOption UndertakerOn;
        public static CustomNumberOption AssassinOn;
        public static CustomNumberOption UnderdogOn;
        #endregion

        #region Modifiers
        public static CustomHeaderOption Modifiers;
        public static CustomNumberOption TorchOn;
        public static CustomNumberOption DiseasedOn;
        public static CustomNumberOption FlashOn;
        public static CustomNumberOption TiebreakerOn;
        public static CustomNumberOption DrunkOn;
        public static CustomNumberOption BigBoiOn;
        #endregion

        #region Custom Game Settings
        public static CustomHeaderOption CustomGameSettings;
        public static CustomToggleOption ColourblindComms;
        public static CustomToggleOption MeetingColourblind;
        public static CustomToggleOption ImpostorSeeRoles;
        public static CustomToggleOption DeadSeeRoles;
        public static CustomNumberOption MaxImpostorRoles;
        public static CustomNumberOption MaxNeutralRoles;
        public static CustomToggleOption RoleUnderName;
        public static CustomNumberOption VanillaGame;
        #endregion

        #region Mayor
        public static CustomNumberOption MayorVoteBank;
        public static CustomToggleOption MayorAnonymous;
        #endregion

        #region Lovers
        public static CustomToggleOption BothLoversDie;
        #endregion

        #region Sheriff
        public static CustomToggleOption ShowSheriff;
        public static CustomToggleOption SheriffKillOther;
        public static CustomToggleOption SheriffKillsJester;
        public static CustomToggleOption SheriffKillsGlitch;
        public static CustomToggleOption SheriffKillsArsonist;
        public static CustomNumberOption SheriffKillCd;
        public static CustomToggleOption SheriffBodyReport;
        #endregion

        #region Shifter
        public static CustomNumberOption ShifterCd;
        public static CustomStringOption WhoShifts;
        #endregion

        #region Engineer
        public static CustomStringOption EngineerPer;
        #endregion

        #region Investigator
        public static CustomNumberOption FootprintSize;
        public static CustomNumberOption FootprintInterval;
        public static CustomNumberOption FootprintDuration;
        public static CustomToggleOption AnonymousFootPrint;
        public static CustomToggleOption VentFootprintVisible;
        #endregion

        #region Time Lord
        public static CustomToggleOption RewindRevive;
        public static CustomNumberOption RewindDuration;
        public static CustomNumberOption RewindCooldown;
        public static CustomToggleOption TimeLordVitals;
        #endregion

        #region Medic
        public static CustomStringOption ShowShielded;
        public static CustomToggleOption MedicReportSwitch;
        public static CustomNumberOption MedicReportNameDuration;
        public static CustomNumberOption MedicReportColorDuration;
        public static CustomStringOption WhoGetsNotification;
        public static CustomToggleOption ShieldBreaks;
        #endregion

        #region Seer
        public static CustomNumberOption SeerCooldown;
        public static CustomStringOption SeerInfo;
        public static CustomStringOption SeeReveal;
        public static CustomToggleOption NeutralRed;
        #endregion

        #region The Glitch
        public static CustomNumberOption MimicCooldownOption;
        public static CustomNumberOption MimicDurationOption;
        public static CustomNumberOption HackCooldownOption;
        public static CustomNumberOption HackDurationOption;
        public static CustomNumberOption GlitchKillCooldownOption;
        public static CustomNumberOption InitialGlitchKillCooldownOption;
        public static CustomStringOption GlitchHackDistanceOption;
        #endregion

        #region Morphling
        public static CustomNumberOption MorphlingCooldown;
        public static CustomNumberOption MorphlingDuration;
        #endregion

        #region Camouflager
        public static CustomNumberOption CamouflagerCooldown;
        public static CustomNumberOption CamouflagerDuration;
        #endregion

        #region Executioner
        public static CustomStringOption OnTargetDead;
        #endregion

        #region Snitch
        public static CustomToggleOption SnitchOnLaunch;
        public static CustomToggleOption SnitchSeesNeutrals;
        #endregion

        #region Altruist
        public static CustomNumberOption ReviveDuration;
        public static CustomToggleOption AltruistTargetBody;
        #endregion

        #region Miner
        public static CustomNumberOption MineCooldown;
        #endregion

        #region Swooper
        public static CustomNumberOption SwoopCooldown;
        public static CustomNumberOption SwoopDuration;
        #endregion

        #region Arsonist
        public static CustomNumberOption DouseCooldown;
        public static CustomToggleOption ArsonistGameEnd;
        #endregion

        #region Undertaker
        public static CustomNumberOption DragCooldown;
        #endregion

        #region Assassin
        public static CustomNumberOption AssassinKills;
        public static CustomToggleOption AssassinGuessNeutrals;
        public static CustomToggleOption AssassinCrewmateGuess;
        public static CustomToggleOption AssassinMultiKill;
        #endregion

        public static Func<object, string> PercentFormat { get; } = value => $"{value:0}%";
        private static Func<object, string> CooldownFormat { get; } = value => $"{value:0.0#}s";

        private static CustomNumberOption CreateOptionGroup(int id, string name, string color, CustomOption[] children)
        {
            var option = new CustomNumberOption(true, id, $"<color={color}>{name}</color>", 0f, 0f, 100f, 10f, PercentFormat);
            if (children.Length > 0)
            {
                var allOptions = CustomOption.AllOptions;
                allOptions.Remove(option);
                allOptions.Insert(allOptions.IndexOf(children[0]), option);
                foreach (var child in children)
                {
                    var old = child.ShouldShow;
                    child.ShouldShow = () => old() && (float)option.Value > 0f;
                    child.Parent = option;
                }
            }
            return option;
        }

        private static CustomNumberOption CreateCooldownOption(
            int id, string name,
            float value, float min, float max, float increment,
            Func<bool> ShouldShow = null
        ) => new CustomNumberOption(id, name, value, min, max, increment, CooldownFormat) {
            ShouldShow = ShouldShow ?? (() => true)
        };
        public static void GenerateAll()
        {
            var num = 0;

            Patches.ExportButton = new Export(num++);
            Patches.ImportButton = new Import(num++);


            CrewmateRoles = new CustomHeaderOption(num++, "Crewmate Roles");
            MayorOn = CreateOptionGroup(num++, "Mayor", "#704FA8FF", new CustomOption[] {
                MayorVoteBank = new CustomNumberOption(num++, "Initial Mayor Vote Bank", 1, 1, 5, 1),
                MayorAnonymous = new CustomToggleOption(num++, "Mayor Votes Show Anonymous", false) {
                    ShouldShow = () => !PlayerControl.GameOptions.AnonymousVotes
                },
            });
            LoversOn = CreateOptionGroup(num++, "Lovers", "#FF66CCFF", new CustomOption[] {
                BothLoversDie = new CustomToggleOption(num++, "Both Lovers Die", true),
            });
            SheriffOn = CreateOptionGroup(num++, "Sheriff", "#FFFF00FF", new CustomOption[] {
                ShowSheriff = new CustomToggleOption(num++, "Show Sheriff", false),
                SheriffKillOther = new CustomToggleOption(num++, "Sheriff Miskill Kills Crewmate", false),
                SheriffKillsJester = new CustomToggleOption(num++, "Sheriff Kills Jester", false) {
                    ShouldShow = () => CustomGameOptions.JesterOn > 0f || (
                        CustomGameOptions.ExecutionerOn > 0f && CustomGameOptions.OnTargetDead == ExecTargetDead.Jester
                    )
                },
                SheriffKillsGlitch = new CustomToggleOption(num++, "Sheriff Kills The Glitch", false) {
                    ShouldShow = () => CustomGameOptions.GlitchOn > 0f
                },
                SheriffKillsArsonist = new CustomToggleOption(num++, "Sheriff Kills Arsonist", false) {
                    ShouldShow = () => CustomGameOptions.ArsonistOn > 0f
                },
                SheriffKillCd = CreateCooldownOption(num++, "Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f),
                SheriffBodyReport = new CustomToggleOption(num++, "Sheriff can report who they've killed", true),
            });
            EngineerOn = CreateOptionGroup(num++, "Engineer", "#FFA60AFF", new CustomOption[] {
                EngineerPer = new CustomStringOption(num++, "Engineer Fix Per", new[] {"Round", "Game"}),
            });
            SwapperOn = CreateOptionGroup(num++, "Swapper", "#66E666FF", new CustomOption[] { });
            InvestigatorOn = CreateOptionGroup(num++, "Investigator", "#00B3B3FF", new CustomOption[] {
                FootprintSize = new CustomNumberOption(num++, "Footprint Size", 4f, 1f, 10f, 1f),
                FootprintInterval = CreateCooldownOption(num++, "Footprint Interval", 1f, 0.25f, 5f, 0.25f),
                FootprintDuration = CreateCooldownOption(num++, "Footprint Duration", 10f, 1f, 10f, 0.5f),
                AnonymousFootPrint = new CustomToggleOption(num++, "Anonymous Footprint", false),
                VentFootprintVisible = new CustomToggleOption(num++, "Footprint Vent Visible", false),
            });
            TimeLordOn = CreateOptionGroup(num++, "Time Lord", "#0000FFFF", new CustomOption[] {
                RewindRevive = new CustomToggleOption(num++, "Revive During Rewind", false),
                RewindDuration = CreateCooldownOption(num++, "Rewind Duration", 3f, 3f, 15f, 0.5f),
                RewindCooldown = CreateCooldownOption(num++, "Rewind Cooldown", 25f, 10f, 40f, 2.5f),
                TimeLordVitals = new CustomToggleOption(num++, "Time Lord can use Vitals", false),
            });
            Func<bool> reportsEnabled = () => CustomGameOptions.ShowReports;
            MedicOn = CreateOptionGroup(num++, "Medic", "#006600FF", new CustomOption[] {
                ShowShielded = new CustomStringOption(num++, "Show Shielded Player", new[] {"Self", "Medic", "Self+Medic", "Everyone"}),
                MedicReportSwitch = new CustomToggleOption(num++, "Show Medic Reports", true),
                MedicReportNameDuration = CreateCooldownOption(num++, "Time Where Medic Reports Will Have Name", 0, 0, 60, 2.5f, reportsEnabled),
                MedicReportColorDuration = CreateCooldownOption(num++, "Time Where Medic Reports Will Have Color Type", 15, 0, 120, 2.5f, reportsEnabled),
                WhoGetsNotification = new CustomStringOption(num++, "Who gets murder attempt indicator", new[] { "Medic", "Shielded", "Everyone", "Nobody" }),
                ShieldBreaks = new CustomToggleOption(num++, "Shield breaks on murder attempt", false),
            });
            SeerOn = CreateOptionGroup(num++, "Seer", "#FFCC80FF", new CustomOption[] {
                SeerCooldown = CreateCooldownOption(num++, "Seer Cooldown", 25f, 10f, 100f, 2.5f),
                SeerInfo = new CustomStringOption(num++, "Info that Seer sees", new[] { "Role", "Team" }),
                SeeReveal = new CustomStringOption(num++, "Who Sees That They Are Revealed", new[] { "Crew", "Imps+Neut", "All", "Nobody" }),
                NeutralRed = new CustomToggleOption(num++, "Neutrals show up as Impostors", false) {
                    ShouldShow = () => CustomGameOptions.SeerInfo == SeerRevealInfo.Faction
                },
            });
            SpyOn = CreateOptionGroup(num++, "Spy", "#CCA3CCFF", new CustomOption[] { });
            SnitchOn = CreateOptionGroup(num++, "Snitch", "#D4AF37FF", new CustomOption[] {
                SnitchOnLaunch = new CustomToggleOption(num++, "Snitch knows who they are on Game Start", false),
                SnitchSeesNeutrals = new CustomToggleOption(num++, "Snitch sees neutral roles", false),
            });
            AltruistOn = CreateOptionGroup(num++, "Altruist", "#660000FF", new CustomOption[] {
                ReviveDuration = CreateCooldownOption(num++, "Altruist Revive Duration", 10, 1, 30, 1f),
                AltruistTargetBody = new CustomToggleOption(num++, "Target's body disappears on beginning of revive", false),
            });

            NeutralRoles = new CustomHeaderOption(num++, "Neutral Roles");

            JesterOn = CreateOptionGroup(num++, "Jester", "#FFBFCCFF", new CustomOption[] { });
            ShifterOn = CreateOptionGroup(num++, "Shifter", "#999999FF", new CustomOption[] {
                ShifterCd = CreateCooldownOption(num++, "Shifter Cooldown", 30f, 10f, 60f, 2.5f),
                WhoShifts = new CustomStringOption(num++, "Who gets the Shifter role on Shift", new[] { "NoImps", "RegCrew", "Nobody" }),
            });
            GlitchOn = CreateOptionGroup(num++, "The Glitch", "#00FF00FF", new CustomOption[] {
                MimicCooldownOption = CreateCooldownOption(num++, "Mimic Cooldown", 30, 10, 120, 2.5f),
                MimicDurationOption = CreateCooldownOption(num++, "Mimic Duration", 10, 1, 30, 1f),
                HackCooldownOption = CreateCooldownOption(num++, "Hack Cooldown", 30, 10, 120, 2.5f),
                HackDurationOption = CreateCooldownOption(num++, "Hack Duration", 10, 1, 30, 1f),
                GlitchKillCooldownOption = CreateCooldownOption(num++, "Glitch Kill Cooldown", 30, 10, 120, 2.5f),
                InitialGlitchKillCooldownOption = CreateCooldownOption(num++, "Initial Glitch Kill Cooldown", 10, 10, 120, 2.5f),
                GlitchHackDistanceOption = new CustomStringOption(num++, "Glitch Hack Distance", new[] { "Short", "Normal", "Long" }),
            });
            ExecutionerOn = CreateOptionGroup(num++, "Executioner", "#8C4005FF", new CustomOption[] {
                OnTargetDead = new CustomStringOption(num++, "Executioner becomes on Target Dead", new[] {"Crew", "Jester"}),
            });
            ArsonistOn = CreateOptionGroup(num++, "Arsonist", "#FF4D00FF", new CustomOption[] {
                DouseCooldown = CreateCooldownOption(num++, "Douse Cooldown", 25, 10, 40, 2.5f),
                ArsonistGameEnd = new CustomToggleOption(num++, "Game keeps going so long as Arsonist is alive", false),
            });
            PhantomOn = CreateOptionGroup(num++, "Phantom", "#662962", new CustomOption[] { });

            ImpostorRoles = new CustomHeaderOption(num++, "Impostor Roles");

            AssassinOn = CreateOptionGroup(num++, "Assassin", "#FF0000FF", new CustomOption[] {
                AssassinKills = new CustomNumberOption(num++, "Number of Assassin Kills", 1, 1, 5, 1),
                AssassinCrewmateGuess = new CustomToggleOption(num++, "Assassin can Guess \"Crewmate\"", false),
                AssassinGuessNeutrals = new CustomToggleOption(num++, "Assassin can Guess Neutral roles", false),
                AssassinMultiKill = new CustomToggleOption(num++, "Assassin can kill more than once per meeting", true) {
                    ShouldShow = () => CustomGameOptions.AssassinKills > 1
                },
            });
            JanitorOn = CreateOptionGroup(num++, "Janitor", "#FF0000FF", new CustomOption[] { });
            MorphlingOn = CreateOptionGroup(num++, "Morphling", "#FF0000FF", new CustomOption[] {
                MorphlingCooldown = CreateCooldownOption(num++, "Morphling Cooldown", 25, 10, 40, 2.5f),
                MorphlingDuration = CreateCooldownOption(num++, "Morphling Duration", 10, 5, 15, 1f),
            });
            CamouflagerOn = CreateOptionGroup(num++, "Camouflager", "#FF0000FF", new CustomOption[] {
                CamouflagerCooldown = CreateCooldownOption(num++, "Camouflager Cooldown", 25, 10, 40, 2.5f),
                CamouflagerDuration = CreateCooldownOption(num++, "Camouflager Duration", 10, 5, 15, 1f),
            });
            MinerOn = CreateOptionGroup(num++, "Miner", "#FF0000FF", new CustomOption[] {
                MineCooldown = CreateCooldownOption(num++, "Mine Cooldown", 25, 10, 40, 2.5f),
            });
            SwooperOn = CreateOptionGroup(num++, "Swooper", "#FF0000FF", new CustomOption[] {
                SwoopCooldown = CreateCooldownOption(num++, "Swoop Cooldown", 25, 10, 40, 2.5f),
                SwoopDuration = CreateCooldownOption(num++, "Swoop Duration", 10, 5, 15, 1f),
            });
            UndertakerOn = CreateOptionGroup(num++, "Undertaker", "#FF0000FF", new CustomOption[] {
                DragCooldown = CreateCooldownOption(num++, "Drag Cooldown", 25, 10, 40, 2.5f),
            });
            UnderdogOn = CreateOptionGroup(num++, "Underdog", "#FF0000FF", new CustomOption[] { });

            Modifiers = new CustomHeaderOption(num++, "Modifiers");

            TorchOn = CreateOptionGroup(num++, "Torch", "#FFFF99FF", new CustomOption[] { });
            DiseasedOn = CreateOptionGroup(num++, "Diseased", "#808080FF", new CustomOption[] { });
            FlashOn = CreateOptionGroup(num++, "Flash", "#FF8080FF", new CustomOption[] { });
            TiebreakerOn = CreateOptionGroup(num++, "Tiebreaker", "#99E699FF", new CustomOption[] { });
            DrunkOn = CreateOptionGroup(num++, "Drunk", "#758000FF", new CustomOption[] { });
            BigBoiOn = CreateOptionGroup(num++, "Giant", "#FF8080FF", new CustomOption[] { });
            ButtonBarryOn = CreateOptionGroup(num++, "Button Barry", "#E600FFFF", new CustomOption[] { });

            CustomGameSettings = new CustomHeaderOption(num++, "Custom Game Settings");
            ColourblindComms = new CustomToggleOption(num++, "Camouflaged Comms", false);
            MeetingColourblind = new CustomToggleOption(num++, "Camouflaged Meetings", false);
            ImpostorSeeRoles = new CustomToggleOption(num++, "Impostors can see the roles of their team", false);

            DeadSeeRoles = new CustomToggleOption(num++, "Dead can see everyone's roles", false);

            MaxImpostorRoles = new CustomNumberOption(num++, "Max Impostor Roles", 1f, 1f, 3f, 1f);
            MaxNeutralRoles = new CustomNumberOption(num++, "Max Neutral Roles", 1f, 1f, 5f, 1f);
            RoleUnderName = new CustomToggleOption(num++, "Role Appears Under Name", true);
            VanillaGame = new CustomNumberOption(num++, "Probability of a completely vanilla game", 0f, 0f, 100f, 5f, PercentFormat);
        }
    }
}
