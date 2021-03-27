using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Essentials.Options;
using HarmonyLib;
using Reactor;
using Reactor.Extensions;
using Reactor.Unstrip;
using TownOfUs.RainbowMod;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace TownOfUs
{
	[BepInPlugin("com.slushiegoose.townofus", "Town Of Us", "1.1.0")]
	[BepInDependency(ReactorPlugin.Id)]
	public class TownOfUs : BasePlugin
	{

		public ConfigEntry<string> Ip { get; set; }
		public ConfigEntry<ushort> Port { get; set; }
		
		public static Sprite JanitorClean;
        public static Sprite EngineerFix;
        public static Sprite SwapperSwitch;
        public static Sprite SwapperSwitchDisabled;
        public static Sprite Shift;
        public static Sprite Kill;
        public static Sprite Footprint;
        public static Sprite Rewind;
        public static Sprite NormalKill;
        public static Sprite GreyscaleKill;
        public static Sprite ShiftKill;
        public static Sprite MedicSprite;
        public static Sprite SeerSprite;
        public static Sprite SampleSprite;
        public static Sprite MorphSprite;
        public static Sprite UseButton;
        public static Sprite Camouflage;
        public static Sprite Admin;

        public static int num = 0;

        public static readonly CustomHeaderOption Roles =
	        CustomOption.AddHeader(num++.ToString(), "Roles");
        public static readonly CustomNumberOption MayorOn = CustomOption.AddNumber(true, num++.ToString(), "[704FA8FF]Mayor", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption JesterOn = CustomOption.AddNumber(true, num++.ToString(), "[FFBFCCFF]Jester", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption LoversOn = CustomOption.AddNumber(true, num++.ToString(), "[FF66CCFF]Lovers", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption SheriffOn = CustomOption.AddNumber(true, num++.ToString(), "[FFFF00FF]Sheriff", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption MafiaOn = CustomOption.AddNumber(true, num++.ToString(), "[FF0000FF]Mafia", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption EngineerOn = CustomOption.AddNumber(true, num++.ToString(), "[FFA60AFF]Engineer", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption SwapperOn = CustomOption.AddNumber(true, num++.ToString(), "[66E666FF]Swapper", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption ShifterOn = CustomOption.AddNumber(true, num++.ToString(), "[999999FF]Shifter", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption InvestigatorOn = CustomOption.AddNumber(true, num++.ToString(), "[00B3B3FF]Investigator", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption TimeLordOn = CustomOption.AddNumber(true, num++.ToString(), "[0000FFFF]Time Lord", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption MedicOn = CustomOption.AddNumber(true, num++.ToString(), "[006600FF]Medic", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption SeerOn = CustomOption.AddNumber(true, num++.ToString(), "[FFCC80FF]Seer", 0f, 0f, 100f, 10f);

        public static readonly CustomNumberOption GlitchOn = CustomOption.AddNumber(true, num++.ToString(), "[00FF00FF]The Glitch", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption MorphlingOn = CustomOption.AddNumber(true, num++.ToString(), "[FF0000FF]Morphling", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption CamouflagerOn = CustomOption.AddNumber(true, num++.ToString(), "[FF0000FF]Camouflager", 0f, 0f, 100f, 10f);

        public static readonly CustomNumberOption ExecutionerOn =
	        CustomOption.AddNumber(true, num++.ToString(), "[8C4005FF]Executioner", 0f, 0f, 100f, 10f);

        public static readonly CustomNumberOption ChildOn = CustomOption.AddNumber(true, num++.ToString(), "Child", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption SpyOn = CustomOption.AddNumber(true, num++.ToString(), "[CCA3CCFF]Spy", 0f, 0f, 100f, 10f);

        public static readonly CustomHeaderOption Modifiers =
	        CustomOption.AddHeader(num++.ToString(), "Modifiers");
        public static readonly CustomNumberOption TorchOn = CustomOption.AddNumber(true, num++.ToString(), "[FFFF99FF]Torch", 0f, 0f, 100f, 10f);

        public static readonly CustomNumberOption DiseasedOn =
	        CustomOption.AddNumber(true, num++.ToString(), "[808080FF]Diseased", 0f, 0f, 100f, 10f);

        public static readonly CustomNumberOption FlashOn = CustomOption.AddNumber(true, num++.ToString(), "[FF8080FF]Flash", 0f, 0f, 100f, 10f);
        
        public static readonly CustomHeaderOption CustomGameSettings =
	        CustomOption.AddHeader(num++.ToString(), "Custom Game Settings");
        public static readonly CustomToggleOption ColourblindComms = CustomOption.AddToggle(num++.ToString(), "Camouflaged Comms", false);
        public static readonly CustomToggleOption MeetingColourblind = CustomOption.AddToggle(num++.ToString(), "Camouflaged Meetings", false);


        public static readonly CustomHeaderOption Mayor =
	        CustomOption.AddHeader(num++.ToString(), "[704FA8FF]Mayor");
        public static readonly CustomNumberOption MayorVoteBank =
	        CustomOption.AddNumber(num++.ToString(), "Mayor Vote Bank", 1, 1, 5, 1);
        
        public static readonly CustomHeaderOption Lovers =
	        CustomOption.AddHeader(num++.ToString(), "[FF66CCFF]Lovers");
        public static readonly CustomToggleOption BothLoversDie = CustomOption.AddToggle(num++.ToString(), "Both Lovers Die", true);
        
        public static readonly CustomHeaderOption Sheriff =
	        CustomOption.AddHeader(num++.ToString(), "[FFFF00FF]Sheriff");
        public static readonly CustomToggleOption ShowSheriff = CustomOption.AddToggle(num++.ToString(), "Show Sheriff", false);

        public static readonly CustomToggleOption SheriffKillOther =
	        CustomOption.AddToggle(num++.ToString(), "Sheriff Miskill Kills Crewmate", false);

        public static readonly CustomToggleOption SheriffKillsJester =
	        CustomOption.AddToggle(num++.ToString(), "Sheriff Kills Jester", false);

        public static readonly CustomNumberOption SheriffKillCd =
	        CustomOption.AddNumber(num++.ToString(), "Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f);

        public static readonly CustomHeaderOption Mafia =
	        CustomOption.AddHeader(num++.ToString(), "[FF0000FF]Mafia");
        public static readonly CustomToggleOption TwoMafia =
	        CustomOption.AddToggle(num++.ToString(), "Two-Imp Mafia", false);

        public static readonly CustomToggleOption JanitorKill =
	        CustomOption.AddToggle(num++.ToString(), "Janitor can kill if Sole Impostor Left", false);
        public static readonly CustomNumberOption JanitorCleanCd =
	        CustomOption.AddNumber(num++.ToString(), "Janitor Clean Cooldown", 25f, 10f, 50f, 2.5f);
        
        
        public static readonly CustomHeaderOption Shifter =
	        CustomOption.AddHeader(num++.ToString(), "[999999FF]Shifter");
        public static readonly CustomNumberOption ShifterCd =
	        CustomOption.AddNumber(num++.ToString(), "Shifter Cooldown", 25f, 10f, 40f, 2.5f);
        
        
        public static readonly CustomHeaderOption Investigator =
	        CustomOption.AddHeader(num++.ToString(), "[00B3B3FF]Investigator");
        public static readonly CustomNumberOption FootprintSize = CustomOption.AddNumber(num++.ToString(), "Footprint Size", 4f, 1f, 10f, 1f);

        public static readonly CustomNumberOption FootprintInterval = CustomOption.AddNumber(num++.ToString(), "Footprint Interval", 1f);
        public static readonly CustomNumberOption FootprintDuration = CustomOption.AddNumber(num++.ToString(), "Footprint Duration", 10f, 1f, 10f, 0.5f);
        public static readonly CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle(num++.ToString(), "Anonymous Footprint", false);
        public static readonly CustomToggleOption VentFootprintVisible = CustomOption.AddToggle(num++.ToString(), "Footprint Vent Visible", false);

        public static readonly CustomHeaderOption TimeLord =
	        CustomOption.AddHeader(num++.ToString(), "[0000FFFF]Time Lord");
        public static readonly CustomToggleOption RewindRevive = CustomOption.AddToggle(num++.ToString(), "Revive During Rewind", false);
        public static readonly CustomNumberOption RewindDuration = CustomOption.AddNumber(num++.ToString(), "Rewind Duration", 3f, 3f, 15f, 0.5f);
        public static readonly CustomNumberOption RewindCooldown = CustomOption.AddNumber(num++.ToString(), "Rewind Cooldown", 25f, 10f, 40f, 2.5f);

        public static readonly CustomToggleOption TimeLordVitals =
	        CustomOption.AddToggle(num++.ToString(), "Time Lord can use Vitals", false);

        public static readonly CustomHeaderOption Medic =
	        CustomOption.AddHeader(num++.ToString(), "[006600FF]Medic");
        
        public static readonly CustomStringOption ShowShielded =
	        CustomOption.AddString(num++.ToString(), "Show Shielded Player", new[] {"Self", "Medic", "Self+Medic", "Everyone"});
        public static readonly CustomToggleOption PlayerMurderIndicator =
	        CustomOption.AddToggle(num++.ToString(), "Murder Attempt Indicator for Shielded Player", false);
        
        public static readonly CustomToggleOption MedicReportSwitch = CustomOption.AddToggle(num++.ToString(), "Show Medic Reports", true);

        public static readonly CustomNumberOption MedicReportNameDuration =
	        CustomOption.AddNumber(num++.ToString(), "Time Where Medic Reports Will Have Name", 0, 0, 60, 2.5f);

        public static readonly CustomNumberOption MedicReportColorDuration =
	        CustomOption.AddNumber(num++.ToString(), "Time Where Medic Reports Will Have Color Type", 15, 0, 120, 2.5f);
        
        public static readonly CustomHeaderOption Seer =
	        CustomOption.AddHeader(num++.ToString(), "[FFCC80FF]Seer");
        
        public static readonly CustomNumberOption SeerCooldown =
	        CustomOption.AddNumber(num++.ToString(), "Seer Cooldown", 25f, 10f, 100f, 2.5f);

        public static readonly CustomStringOption SeerInfo =
	        CustomOption.AddString(num++.ToString(), "Info that Seer sees", new string[] {"Role", "Team"});

        public static readonly CustomStringOption SeeReveal =
	        CustomOption.AddString(num++.ToString(), "Who Sees That They Are Revealed", new string[] {"Crew", "Imps+Neut", "All", "Nobody"});

        public static readonly CustomToggleOption NeutralRed =
	        CustomOption.AddToggle(num++.ToString(), "Neutrals show up as Impostors", false);
        
        
        public static readonly CustomHeaderOption TheGlitch =
	        CustomOption.AddHeader(num++.ToString(), "[00FF00FF]The Glitch");
        public static readonly CustomNumberOption MimicCooldownOption = CustomOption.AddNumber(num++.ToString(), "Mimic Cooldown", 30, 10, 120, 2.5f);
        public static readonly CustomNumberOption MimicDurationOption = CustomOption.AddNumber(num++.ToString(), "Mimic Duration", 10, 1, 30, 1f);
        public static readonly CustomNumberOption HackCooldownOption = CustomOption.AddNumber(num++.ToString(), "Hack Cooldown", 30, 10, 120, 2.5f);
        public static readonly CustomNumberOption HackDurationOption = CustomOption.AddNumber(num++.ToString(), "Hack Duration", 10, 1, 30, 1f);
        public static readonly CustomNumberOption GlitchKillCooldownOption = CustomOption.AddNumber(num++.ToString(), "Glitch Kill Cooldown", 30, 10, 120, 2.5f);
        public static readonly CustomNumberOption InitialGlitchKillCooldownOption = CustomOption.AddNumber(num++.ToString(), "Initial Glitch Kill Cooldown", 10, 10, 120, 2.5f);
        public static readonly CustomStringOption GlitchHackDistanceOption = CustomOption.AddString(num++.ToString(), "Glitch Hack Distance", new string[] { "Short", "Normal", "Long" });

        
        public static readonly CustomHeaderOption Morphling =
	        CustomOption.AddHeader(num++.ToString(), "[FF0000FF]Morphling");
        public static readonly CustomNumberOption MorphlingCooldown =
	        CustomOption.AddNumber(num++.ToString(), "Morphling Cooldown", 25, 10, 40, 2.5f);
        public static readonly CustomNumberOption MorphlingDuration =
	        CustomOption.AddNumber(num++.ToString(), "Morphling Duration", 10, 5, 15, 1f);

        public static readonly CustomHeaderOption Camouflager =
	        CustomOption.AddHeader(num++.ToString(), "[FF0000FF]Camouflager");
        public static readonly CustomNumberOption CamouflagerCooldown =
	        CustomOption.AddNumber(num++.ToString(), "Camouflager Cooldown", 25, 10, 40, 2.5f);
        public static readonly CustomNumberOption CamouflagerDuration =
	        CustomOption.AddNumber(num++.ToString(), "Camouflager Duration", 10, 5, 15, 1f);

        public static readonly CustomHeaderOption Executioner =
	        CustomOption.AddHeader(num++.ToString(), "[8C4005FF]Executioner");
        public static readonly CustomStringOption OnTargetDead = CustomOption.AddString(num++.ToString(), "Executioner becomes on Target Dead", new string[] { "Crew", "Jester" });

        
        
        public static Func<CustomOption, object, string> PercentFormat { get; } = (sender, value) => $"{value:0}%";
        private static Func<CustomOption, object, string> CooldownFormat { get; } = (sender, value) => $"{value:0.0#}s";


        public static readonly List<CustomOption> AllOptions = new List<CustomOption>
        {
	        Roles,
	        
	        MayorOn, JesterOn, LoversOn, SheriffOn, MafiaOn, EngineerOn, SwapperOn, ShifterOn,
	        InvestigatorOn, TimeLordOn, MedicOn, SeerOn, GlitchOn, MorphlingOn, CamouflagerOn,
	        ExecutionerOn, ChildOn, SpyOn,
	        
	        Modifiers,
	        TorchOn, DiseasedOn, FlashOn,
	        
	        CustomGameSettings,
	        ColourblindComms, MeetingColourblind,
	        Mayor, MayorVoteBank,
	        Lovers, BothLoversDie,
	        Sheriff, SheriffKillCd, SheriffKillOther, SheriffKillsJester,
	        Mafia, TwoMafia, JanitorKill, JanitorCleanCd,
	        Shifter, ShifterCd,
	        
	        Investigator, FootprintInterval, FootprintDuration, AnonymousFootPrint, VentFootprintVisible,
	        TimeLord, RewindRevive, RewindDuration, RewindCooldown, TimeLordVitals,
	        
	        Medic, ShowShielded, PlayerMurderIndicator, MedicReportSwitch, MedicReportColorDuration, MedicReportNameDuration,
	        Seer, SeerCooldown, SeerInfo, SeeReveal, NeutralRed,
	        
	        TheGlitch, MimicCooldownOption, MimicDurationOption, HackCooldownOption, HackDurationOption, GlitchKillCooldownOption, InitialGlitchKillCooldownOption, GlitchHackDistanceOption,
	        Morphling, MorphlingCooldown, MorphlingDuration,
	        Camouflager, CamouflagerCooldown, CamouflagerDuration,
			Executioner, OnTargetDead,
        };
        


        public override void Load()
		{
			this._harmony = new Harmony("com.slushiegoose.townofus");

			CustomOption.LobbyTextScroller = true;
			
			MayorOn.StringFormat = PercentFormat;
			JesterOn.StringFormat = PercentFormat;
			LoversOn.StringFormat = PercentFormat;
			SheriffOn.StringFormat = PercentFormat;
			MafiaOn.StringFormat = PercentFormat;
			EngineerOn.StringFormat = PercentFormat;
			SwapperOn.StringFormat = PercentFormat;
			ShifterOn.StringFormat = PercentFormat;
			InvestigatorOn.StringFormat = PercentFormat;
			TimeLordOn.StringFormat = PercentFormat;
			MedicOn.StringFormat = PercentFormat;
			SeerOn.StringFormat = PercentFormat;
			GlitchOn.StringFormat = PercentFormat;
			MorphlingOn.StringFormat = PercentFormat;
			CamouflagerOn.StringFormat = PercentFormat;
			ExecutionerOn.StringFormat = PercentFormat;
			ChildOn.StringFormat = PercentFormat;
			SpyOn.StringFormat = PercentFormat;
			
			
			TorchOn.StringFormat = PercentFormat;
			DiseasedOn.StringFormat = PercentFormat;
			FlashOn.StringFormat = PercentFormat;
			
			

			SheriffKillCd.StringFormat = CooldownFormat;
			JanitorCleanCd.StringFormat = CooldownFormat;
			ShifterCd.StringFormat = CooldownFormat;
			FootprintInterval.StringFormat = CooldownFormat;
			FootprintDuration.StringFormat = CooldownFormat;
			RewindDuration.StringFormat = CooldownFormat;
			RewindCooldown.StringFormat = CooldownFormat;
			SeerCooldown.StringFormat = CooldownFormat;

			MedicReportColorDuration.StringFormat = CooldownFormat;
			MedicReportNameDuration.StringFormat = CooldownFormat;
			
			MimicCooldownOption.StringFormat = CooldownFormat;
			MimicDurationOption.StringFormat = CooldownFormat;
			HackCooldownOption.StringFormat = CooldownFormat;
			HackDurationOption.StringFormat = CooldownFormat;
			GlitchKillCooldownOption.StringFormat = CooldownFormat;
			InitialGlitchKillCooldownOption.StringFormat = CooldownFormat;

			MorphlingCooldown.StringFormat = CooldownFormat;
			MorphlingDuration.StringFormat = CooldownFormat;
			
			CamouflagerCooldown.StringFormat = CooldownFormat;
			CamouflagerDuration.StringFormat = CooldownFormat;




			JanitorClean = CreateSprite("TownOfUs.Resources.Janitor.png");
			EngineerFix = CreateSprite("TownOfUs.Resources.Engineer.png");
			//EngineerArrow = CreateSprite("TownOfUs.Resources.EngineerArrow.png");
			SwapperSwitch = CreateSprite("TownOfUs.Resources.SwapperSwitch.png");
			SwapperSwitchDisabled = CreateSprite("TownOfUs.Resources.SwapperSwitchDisabled.png");
			Shift = CreateSprite("TownOfUs.Resources.Shift.png");
			Kill = CreateSprite("TownOfUs.Resources.Kill.png");
			Footprint = CreateSprite("TownOfUs.Resources.Footprint.png");
			Rewind = CreateSprite("TownOfUs.Resources.Rewind.png");
			NormalKill = CreateSprite("TownOfUs.Resources.NormalKill.png");
			GreyscaleKill = CreateSprite("TownOfUs.Resources.GreyscaleKill.png");
			ShiftKill = CreateSprite("TownOfUs.Resources.ShiftKill.png");
			MedicSprite = CreateSprite("TownOfUs.Resources.Medic.png");
			SeerSprite = CreateSprite("TownOfUs.Resources.Seer.png");
			SampleSprite = CreateSprite("TownOfUs.Resources.Sample.png");
			MorphSprite = CreateSprite("TownOfUs.Resources.Morph.png");
			UseButton = CreateSprite("TownOfUs.Resources.UseButton.png");
			Camouflage = CreateSprite("TownOfUs.Resources.Camouflage.png");
			Admin = CreateSprite("TownOfUs.Resources.Admin.png");
			
			PalettePatch.Load();
			ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();
			
			Ip = Config.Bind("Custom", "Ipv4 or Hostname", "127.0.0.1");
			Port = Config.Bind("Custom", "Port", (ushort) 22023);
			var defaultRegions = ServerManager.DefaultRegions.ToList();
			var ip = Ip.Value;
			if (Uri.CheckHostName(Ip.Value).ToString() == "Dns")
			{
				foreach (var address in Dns.GetHostAddresses(Ip.Value))
				{
					if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
						continue;
					ip = address.ToString();
					break;
				}
			}

			/*defaultRegions.Insert(0, new RegionInfo(
				"Custom", ip, new[]
				{
					new ServerInfo($"Custom-Server", ip, Port.Value)
				})
			);
			*/

			ServerManager.DefaultRegions = defaultRegions.ToArray();
			
			this._harmony.PatchAll();
			
		}
        
		private static Sprite CreateSprite(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var tex = GUIExtensions.CreateEmptyTexture();
			var imageStream = assembly.GetManifestResourceStream(name);
			var img = imageStream.ReadFully();
			LoadImage(tex, img, true);
			tex.DontDestroy();
			var sprite = tex.CreateSprite();
			sprite.DontDestroy();
			return sprite;
		}
		
		private static void LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
		{
			_iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
			var il2CPPArray = (Il2CppStructArray<byte>) data;
			_iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
		}

		private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);

		private static DLoadImage _iCallLoadImage;
		
		
		private Harmony _harmony;
	}
}
