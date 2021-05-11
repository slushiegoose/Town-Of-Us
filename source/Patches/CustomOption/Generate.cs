// This folder is a Stripped down version of Reactor-Essentials
// Please use https://github.com/DorCoMaNdO/Reactor-Essentials because it is more updated and less buggy

using System;

namespace TownOfUs.CustomOption
{
	public class Generate
	{
		public static Func<object, string> PercentFormat { get; } = (value) => $"{value:0}%";
		private static Func<object, string> CooldownFormat { get; } = (value) => $"{value:0.0#}s";
		
		public static CustomHeaderOption CrewmateRoles ;
        public static CustomNumberOption MayorOn;
        public static CustomNumberOption LoversOn;
        public static CustomNumberOption SheriffOn;
        public static CustomNumberOption EngineerOn;
        public static CustomNumberOption SwapperOn;
        public static CustomNumberOption InvestigatorOn;
        public static CustomNumberOption TimeLordOn;
        public static CustomNumberOption MedicOn;
        public static CustomNumberOption SeerOn;
        public static CustomNumberOption ChildOn;
        public static CustomNumberOption SpyOn;
        public static CustomNumberOption SnitchOn ;
        public static CustomNumberOption AltruistOn;
        public static CustomNumberOption ButtonBarryOn;

        
        
        public static CustomHeaderOption NeutralRoles ;
        public static CustomNumberOption JesterOn;
        public static CustomNumberOption ShifterOn;
        public static CustomNumberOption GlitchOn;
        public static CustomNumberOption ExecutionerOn;
        public static CustomNumberOption ArsonistOn;

        
        public static CustomHeaderOption ImpostorRoles ;
        public static CustomNumberOption JanitorOn;
        public static CustomNumberOption MorphlingOn;
        public static CustomNumberOption CamouflagerOn;
        public static CustomNumberOption MinerOn;
        public static CustomNumberOption SwooperOn;


        /*
        public static CustomNumberOption SecurityGuardOn ;
	        */
        
        public static CustomHeaderOption Modifiers ;
        public static CustomNumberOption TorchOn;
        public static CustomNumberOption DiseasedOn ;
        public static CustomNumberOption FlashOn;
        public static CustomNumberOption TiebreakerOn;
        public static CustomNumberOption DrunkOn;
        public static CustomNumberOption BigBoiOn;

        
        public static CustomHeaderOption CustomGameSettings ;
        public static CustomToggleOption ColourblindComms;
        public static CustomToggleOption MeetingColourblind;
        public static CustomToggleOption ImpostorSeeRoles;
        public static CustomToggleOption DeadSeeRoles ;
        public static CustomNumberOption MaxImpostorRoles ;
        public static CustomNumberOption MaxNeutralRoles ;
        public static CustomToggleOption RoleUnderName;
        public static CustomNumberOption VanillaGame;

        public static CustomHeaderOption Mayor ;
        public static CustomNumberOption MayorVoteBank ;
        public static CustomToggleOption MayorAnonymous ;

        public static CustomHeaderOption Lovers ;
        public static CustomToggleOption BothLoversDie;
        
        public static CustomHeaderOption Sheriff ;
        public static CustomToggleOption ShowSheriff;
        public static CustomToggleOption SheriffKillOther ;
        public static CustomToggleOption SheriffKillsJester ;
        public static CustomToggleOption SheriffKillsGlitch ;
        public static CustomToggleOption SheriffKillsArsonist ;
        public static CustomNumberOption SheriffKillCd ;
        public static CustomToggleOption SheriffBodyReport;
        
        
        public static CustomHeaderOption Shifter ;
        public static CustomNumberOption ShifterCd ;
		public static CustomStringOption WhoShifts;
	        
        
        public static CustomHeaderOption Engineer ;
        public static CustomStringOption EngineerPer ;

        
        
        
        public static CustomHeaderOption Investigator ;
        public static CustomNumberOption FootprintSize;
        public static CustomNumberOption FootprintInterval;
        public static CustomNumberOption FootprintDuration;
        public static CustomToggleOption AnonymousFootPrint;
        public static CustomToggleOption VentFootprintVisible;

        public static CustomHeaderOption TimeLord ;
        public static CustomToggleOption RewindRevive;
        public static CustomNumberOption RewindDuration;
        public static CustomNumberOption RewindCooldown;
        public static CustomToggleOption TimeLordVitals ;

        public static CustomHeaderOption Medic ;
        public static CustomStringOption ShowShielded ;
        public static CustomToggleOption MedicReportSwitch;
        public static CustomNumberOption MedicReportNameDuration ;
        public static CustomNumberOption MedicReportColorDuration ;
        public static CustomStringOption WhoGetsNotification;
        public static CustomToggleOption ShieldBreaks;
        
        public static CustomHeaderOption Seer ;
        public static CustomNumberOption SeerCooldown ;
        public static CustomStringOption SeerInfo ;
        public static CustomStringOption SeeReveal ;
        public static CustomToggleOption NeutralRed ;
        
        
        public static CustomHeaderOption TheGlitch ;
        public static CustomNumberOption MimicCooldownOption;
        public static CustomNumberOption MimicDurationOption;
        public static CustomNumberOption HackCooldownOption;
        public static CustomNumberOption HackDurationOption;
        public static CustomNumberOption GlitchKillCooldownOption;
        public static CustomNumberOption InitialGlitchKillCooldownOption;
        public static CustomStringOption GlitchHackDistanceOption;

        
        public static CustomHeaderOption Morphling ;
        public static CustomNumberOption MorphlingCooldown ;
        public static CustomNumberOption MorphlingDuration ;

        public static CustomHeaderOption Camouflager ;
        public static CustomNumberOption CamouflagerCooldown ;
        public static CustomNumberOption CamouflagerDuration ;

        public static CustomHeaderOption Executioner ;
        public static CustomStringOption OnTargetDead;

        public static CustomHeaderOption Snitch;
        public static CustomToggleOption SnitchOnLaunch ;
        
        public static CustomHeaderOption Altruist;
        public static CustomNumberOption ReviveDuration ;
        public static CustomToggleOption AltruistTargetBody;
	        
        public static CustomHeaderOption Miner;
        public static CustomNumberOption MineCooldown ;
        
        public static CustomHeaderOption Swooper;
        public static CustomNumberOption SwoopCooldown ;
        public static CustomNumberOption SwoopDuration ;
        
        public static CustomHeaderOption Arsonist;
        public static CustomNumberOption DouseCooldown ;
        public static CustomToggleOption ArsonistGameEnd;
		
		
		
		public static void GenerateAll()
		{
			var num = 0;

			Patches.ExportButton = new Export(num++);
			Patches.ImportButton = new Import(num++);
			
			
			
			CrewmateRoles =
				new CustomHeaderOption(num++, "Crewmate Roles");
			MayorOn = new CustomNumberOption(true, num++, "<color=#704FA8FF>Mayor</color>", 0f, 0f, 100f, 10f, PercentFormat);
			LoversOn = new CustomNumberOption(true, num++, "<color=#FF66CCFF>Lovers</color>", 0f, 0f, 100f, 10f, PercentFormat);
			SheriffOn = new CustomNumberOption(true, num++, "<color=#FFFF00FF>Sheriff</color>", 0f, 0f, 100f, 10f, PercentFormat);
			EngineerOn = new CustomNumberOption(true, num++, "<color=#FFA60AFF>Engineer</color>", 0f, 0f, 100f, 10f, PercentFormat);
			SwapperOn = new CustomNumberOption(true, num++, "<color=#66E666FF>Swapper</color>", 0f, 0f, 100f, 10f, PercentFormat);
			InvestigatorOn = new CustomNumberOption(true, num++, "<color=#00B3B3FF>Investigator</color>", 0f, 0f, 100f, 10f, PercentFormat);
			TimeLordOn = new CustomNumberOption(true, num++, "<color=#0000FFFF>Time Lord</color>", 0f, 0f, 100f, 10f, PercentFormat);
			MedicOn = new CustomNumberOption(true, num++, "<color=#006600FF>Medic</color>", 0f, 0f, 100f, 10f, PercentFormat);
			SeerOn = new CustomNumberOption(true, num++, "<color=#FFCC80FF>Seer</color>", 0f, 0f, 100f, 10f, PercentFormat);
			ChildOn = new CustomNumberOption(true, num++, "Child", 0f, 0f, 100f, 10f, PercentFormat);
			SpyOn = new CustomNumberOption(true, num++, "<color=#CCA3CCFF>Spy</color>", 0f, 0f, 100f, 10f, PercentFormat);
			SnitchOn =
				new CustomNumberOption(true, num++, "<color=#D4AF37FF>Snitch</color>", 0f, 0f, 100f, 10f, PercentFormat);
			AltruistOn =
				new CustomNumberOption(true, num++, "<color=#660000FF>Altruist</color>", 0f, 0f, 100f, 10f, PercentFormat);


			NeutralRoles =
				new CustomHeaderOption(num++, "Neutral Roles");
			JesterOn = new CustomNumberOption(true, num++, "<color=#FFBFCCFF>Jester</color>", 0f, 0f, 100f, 10f, PercentFormat);
			ShifterOn = new CustomNumberOption(true, num++, "<color=#999999FF>Shifter</color>", 0f, 0f, 100f, 10f, PercentFormat);
			GlitchOn = new CustomNumberOption(true, num++, "<color=#00FF00FF>The Glitch</color>", 0f, 0f, 100f, 10f, PercentFormat);
			ExecutionerOn = new CustomNumberOption(true, num++, "<color=#8C4005FF>Executioner</color>", 0f, 0f, 100f, 10f, PercentFormat);
			ArsonistOn = new CustomNumberOption(true, num++, "<color=#FF4D00FF>Arsonist</color>", 0f, 0f, 100f, 10f, PercentFormat);


			ImpostorRoles =
				new CustomHeaderOption(num++, "Impostor Roles");
			JanitorOn = new CustomNumberOption(true, num++, "<color=#FF0000FF>Janitor</color>", 0f, 0f, 100f, 10f, PercentFormat);
			MorphlingOn = new CustomNumberOption(true, num++, "<color=#FF0000FF>Morphling</color>", 0f, 0f, 100f, 10f, PercentFormat);
			CamouflagerOn = new CustomNumberOption(true, num++, "<color=#FF0000FF>Camouflager</color>", 0f, 0f, 100f, 10f, PercentFormat);
			MinerOn = new CustomNumberOption(true, num++, "<color=#FF0000FF>Miner</color>", 0f, 0f, 100f, 10f, PercentFormat);
			SwooperOn = new CustomNumberOption(true, num++, "<color=#FF0000FF>Swooper</color>", 0f, 0f, 100f, 10f, PercentFormat);


			Modifiers = new CustomHeaderOption(num++, "Modifiers");
			TorchOn = new CustomNumberOption(true, num++, "<color=#FFFF99FF>Torch</color>", 0f, 0f, 100f, 10f, PercentFormat);
			DiseasedOn =
				new CustomNumberOption(true, num++, "<color=#808080FF>Diseased</color>", 0f, 0f, 100f, 10f, PercentFormat);
			FlashOn = new CustomNumberOption(true, num++, "<color=#FF8080FF>Flash</color>", 0f, 0f, 100f, 10f, PercentFormat);
			TiebreakerOn = new CustomNumberOption(true, num++, "<color=#99E699FF>Tiebreaker</color>", 0f, 0f, 100f, 10f, PercentFormat);
			DrunkOn = new CustomNumberOption(true, num++, "<color=#758000FF>Drunk</color>", 0f, 0f, 100f, 10f, PercentFormat);
			BigBoiOn = new CustomNumberOption(true, num++, "<color=#FF8080FF>Giant</color>", 0f, 0f, 100f, 10f, PercentFormat);
			ButtonBarryOn =
				new CustomNumberOption(true, num++, "<color=#E600FFFF>Button Barry</color>", 0f, 0f, 100f, 5f, PercentFormat);


			CustomGameSettings =
				new CustomHeaderOption(num++, "Custom Game Settings");
			ColourblindComms = new CustomToggleOption(num++, "Camouflaged Comms", false);
			MeetingColourblind = new CustomToggleOption(num++, "Camouflaged Meetings", false);
			ImpostorSeeRoles = new CustomToggleOption(num++, "Impostors can see the roles of their team", false);

			DeadSeeRoles =
				new CustomToggleOption(num++, "Dead can see everyone's roles", false);

			MaxImpostorRoles =
				new CustomNumberOption(num++, "Max Impostor Roles", 1f, 1f, 3f, 1f);
			MaxNeutralRoles =
				new CustomNumberOption(num++, "Max Neutral Roles", 1f, 1f, 5f, 1f);
			RoleUnderName = new CustomToggleOption(num++, "Role Appears Under Name", true);
			VanillaGame = new CustomNumberOption(num++, "Probability of a completely vanilla game", 0f, 0f, 100f, 5f, PercentFormat);

			Mayor =
				new CustomHeaderOption(num++, "<color=#704FA8FF>Mayor</color>");

			MayorVoteBank =
				new CustomNumberOption(num++, "Initial Mayor Vote Bank", 1, 1, 5, 1);

			MayorAnonymous =
				new CustomToggleOption(num++, "Mayor Votes Show Anonymous", false);

			Lovers =
				new CustomHeaderOption(num++, "<color=#FF66CCFF>Lovers</color>");
			BothLoversDie = new CustomToggleOption(num++, "Both Lovers Die", true);

			Sheriff =
				new CustomHeaderOption(num++, "<color=#FFFF00FF>Sheriff</color>");
			ShowSheriff = new CustomToggleOption(num++, "Show Sheriff", false);

			SheriffKillOther =
				new CustomToggleOption(num++, "Sheriff Miskill Kills Crewmate", false);

			SheriffKillsJester =
				new CustomToggleOption(num++, "Sheriff Kills Jester", false);
			SheriffKillsGlitch =
				new CustomToggleOption(num++, "Sheriff Kills The Glitch", false);
			SheriffKillsArsonist =
				new CustomToggleOption(num++, "Sheriff Kills Arsonist", false);

			SheriffKillCd =
				new CustomNumberOption(num++, "Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat);
			SheriffBodyReport = new CustomToggleOption(num++, "Sheriff can report who they've killed", true);

			

			Engineer =
				new CustomHeaderOption(num++, "<color=#FFA60AFF>Engineer</color>");
			EngineerPer =
				new CustomStringOption(num++, "Engineer Fix Per", new[] {"Round", "Game"});




			Investigator =
				new CustomHeaderOption(num++, "<color=#00B3B3FF>Investigator</color>");
			FootprintSize = new CustomNumberOption(num++, "Footprint Size", 4f, 1f, 10f, 1f);

			FootprintInterval = new CustomNumberOption(num++, "Footprint Interval", 1f, 0.25f, 5f, 0.25f, CooldownFormat);
			FootprintDuration = new CustomNumberOption(num++, "Footprint Duration", 10f, 1f, 10f, 0.5f, CooldownFormat);
			AnonymousFootPrint = new CustomToggleOption(num++, "Anonymous Footprint", false);
			VentFootprintVisible = new CustomToggleOption(num++, "Footprint Vent Visible", false);

			TimeLord =
				new CustomHeaderOption(num++, "<color=#0000FFFF>Time Lord</color>");
			RewindRevive = new CustomToggleOption(num++, "Revive During Rewind", false);
			RewindDuration = new CustomNumberOption(num++, "Rewind Duration", 3f, 3f, 15f, 0.5f, CooldownFormat);
			RewindCooldown = new CustomNumberOption(num++, "Rewind Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat);

			TimeLordVitals =
				new CustomToggleOption(num++, "Time Lord can use Vitals", false);

			Medic =
				new CustomHeaderOption(num++, "<color=#006600FF>Medic</color>");

			ShowShielded =
				new CustomStringOption(num++, "Show Shielded Player",
					new[] {"Self", "Medic", "Self+Medic", "Everyone"});

			MedicReportSwitch = new CustomToggleOption(num++, "Show Medic Reports", true);

			MedicReportNameDuration =
				new CustomNumberOption(num++, "Time Where Medic Reports Will Have Name", 0, 0, 60, 2.5f, CooldownFormat);

			MedicReportColorDuration =
				new CustomNumberOption(num++, "Time Where Medic Reports Will Have Color Type", 15, 0, 120, 2.5f, CooldownFormat);
			
			WhoGetsNotification =
				new CustomStringOption(num++, "Who gets murder attempt indicator", new[] {"Medic", "Shielded", "Everyone", "Nobody"});

			ShieldBreaks = new CustomToggleOption(num++, "Shield breaks on murder attempt", false);

			Seer =
				new CustomHeaderOption(num++, "<color=#FFCC80FF>Seer</color>");

			SeerCooldown =
				new CustomNumberOption(num++, "Seer Cooldown", 25f, 10f, 100f, 2.5f, CooldownFormat);

			SeerInfo =
				new CustomStringOption(num++, "Info that Seer sees", new string[] {"Role", "Team"});
			

			SeeReveal =
				new CustomStringOption(num++, "Who Sees That They Are Revealed",
					new string[] {"Crew", "Imps+Neut", "All", "Nobody"});
			NeutralRed =
				new CustomToggleOption(num++, "Neutrals show up as Impostors", false);
			
			Snitch = new CustomHeaderOption(num++, "<color=#D4AF37FF>Snitch</color>");
			SnitchOnLaunch =
				new CustomToggleOption(num++, "Snitch knows who they are on Game Start", false);
			
			Altruist = new CustomHeaderOption(num++, "<color=#660000FF>Altruist</color>");
			ReviveDuration =
				new CustomNumberOption(num++, "Altruist Revive Duration", 10, 1, 30, 1f, CooldownFormat);
			AltruistTargetBody =
				new CustomToggleOption(num++, "Target's body disappears on beginning of revive", false);

			Shifter =
				new CustomHeaderOption(num++, "<color=#999999FF>Shifter</color>");
			ShifterCd =
				new CustomNumberOption(num++, "Shifter Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat);
			WhoShifts = new CustomStringOption(num++,
				"Who gets the Shifter role on Shift", new[] {"NoImps", "RegCrew", "Nobody"});
			
			
			TheGlitch =
				new CustomHeaderOption(num++, "<color=#00FF00FF>The Glitch</color>");
			MimicCooldownOption = new CustomNumberOption(num++, "Mimic Cooldown", 30, 10, 120, 2.5f, CooldownFormat);
			MimicDurationOption = new CustomNumberOption(num++, "Mimic Duration", 10, 1, 30, 1f, CooldownFormat);
			HackCooldownOption = new CustomNumberOption(num++, "Hack Cooldown", 30, 10, 120, 2.5f, CooldownFormat);
			HackDurationOption = new CustomNumberOption(num++, "Hack Duration", 10, 1, 30, 1f, CooldownFormat);
			GlitchKillCooldownOption = new CustomNumberOption(num++, "Glitch Kill Cooldown", 30, 10, 120, 2.5f, CooldownFormat);
			InitialGlitchKillCooldownOption =
				new CustomNumberOption(num++, "Initial Glitch Kill Cooldown", 10, 10, 120, 2.5f, CooldownFormat);
			GlitchHackDistanceOption =
				new CustomStringOption(num++, "Glitch Hack Distance", new string[] {"Short", "Normal", "Long"});

			Executioner =
				new CustomHeaderOption(num++, "<color=#8C4005FF>Executioner</color>");
			OnTargetDead = new CustomStringOption(num++, "Executioner becomes on Target Dead",
				new string[] {"Crew", "Jester"});
			
			Arsonist = new CustomHeaderOption(num++, "<color=#FF4D00FF>Arsonist</color>");

			DouseCooldown =
				new CustomNumberOption(num++, "Douse Cooldown", 25, 10, 40, 2.5f, CooldownFormat);

			ArsonistGameEnd = new CustomToggleOption(num++, "Game keeps going so long as Arsonist is alive", false);
			
			
			Morphling =
				new CustomHeaderOption(num++, "<color=#FF0000FF>Morphling</color>");
			MorphlingCooldown =
				new CustomNumberOption(num++, "Morphling Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
			MorphlingDuration =
				new CustomNumberOption(num++, "Morphling Duration", 10, 5, 15, 1f, CooldownFormat);

			Camouflager =
				new CustomHeaderOption(num++, "<color=#FF0000FF>Camouflager</color>");
			CamouflagerCooldown =
				new CustomNumberOption(num++, "Camouflager Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
			CamouflagerDuration =
				new CustomNumberOption(num++, "Camouflager Duration", 10, 5, 15, 1f, CooldownFormat);

			

			

			Miner = new CustomHeaderOption(num++, "<color=#FF0000FF>Miner</color>");

			MineCooldown =
				new CustomNumberOption(num++, "Mine Cooldown", 25, 10, 40, 2.5f, CooldownFormat);

			Swooper = new CustomHeaderOption(num++, "<color=#FF0000FF>Swooper</color>");

			SwoopCooldown =
				new CustomNumberOption(num++, "Swoop Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
			SwoopDuration =
				new CustomNumberOption(num++, "Swoop Duration", 10, 5, 15, 1f, CooldownFormat);

			
		}
	}
}