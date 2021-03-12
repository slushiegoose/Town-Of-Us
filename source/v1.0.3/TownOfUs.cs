using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using Essentials.Options;
using HarmonyLib;
using Reactor;
using Reactor.Extensions;
using Reactor.Unstrip;
using TownOfUs.RainbowMod;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace TownOfUs
{
	[BepInPlugin("com.slushiegoose.townofus", "Town Of Us", "1.0.3")]
	[BepInDependency(ReactorPlugin.Id)]
	public class TownOfUs : BasePlugin
	{

		public static Sprite JanitorClean;
        public static Sprite EngineerFix;
        public static Sprite EngineerArrow;
        public static Sprite SwapperSwitch;
        public static Sprite SwapperSwitchDisabled;
        public static Sprite Shift;
        public static Sprite Kill;
        public static Sprite Footprint;
        public static Sprite Rewind;
        public static Sprite NormalKill;
        public static Sprite GreyscaleKill;


        public static readonly CustomNumberOption MayorOn = CustomOption.AddNumber("Mayor", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption JesterOn = CustomOption.AddNumber("Jester", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption LoversOn = CustomOption.AddNumber("Lovers", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption SheriffOn = CustomOption.AddNumber("Sheriff", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption MafiaOn = CustomOption.AddNumber("Mafia", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption EngineerOn = CustomOption.AddNumber("Engineer", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption SwapperOn = CustomOption.AddNumber("Swapper", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption ShifterOn = CustomOption.AddNumber("Shifter", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption InvestigatorOn = CustomOption.AddNumber("Investigator", 0f, 0f, 100f, 10f);
        public static readonly CustomNumberOption TimeMasterOn = CustomOption.AddNumber("Time Master", 0f, 0f, 100f, 10f);


        public static readonly CustomToggleOption BothLoversDie = CustomOption.AddToggle("Both Lovers Die", true);
        public static readonly CustomToggleOption ShowSheriff = CustomOption.AddToggle("Show Sheriff", false);

        public static readonly CustomNumberOption SheriffKillCd =
	        CustomOption.AddNumber("Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f);
        public static readonly CustomNumberOption JanitorCleanCd =
	        CustomOption.AddNumber("Janitor Clean Cooldown", 25f, 10f, 50f, 2.5f);

        public static readonly CustomNumberOption MayorVision =
	        CustomOption.AddNumber("Mayor Vision", 1f);
        public static readonly CustomNumberOption EngineerVision =
	        CustomOption.AddNumber("Engineer Vision", 1f);
        
        public static readonly CustomNumberOption MayorVoteBank =
	        CustomOption.AddNumber("Mayor Vote Bank", 1, 1, 5, 1);
	        
        public static readonly CustomNumberOption ShifterCd =
	        CustomOption.AddNumber("Shifter Cooldown", 25f, 10f, 40f, 2.5f);
        
        public static readonly CustomNumberOption FootprintSize = CustomOption.AddNumber("Footprint Size", 4f, 1f, 10f, 1f);

        public static readonly CustomNumberOption FootprintInterval = CustomOption.AddNumber("Footprint Interval", 1f);
        public static readonly CustomNumberOption FootprintDuration = CustomOption.AddNumber("Footprint Duration", 10f, 1f, 10f, 0.5f);
        public static readonly CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle("Anonymous Footprint", false);
        public static readonly CustomToggleOption VentFootprintVisible = CustomOption.AddToggle("Footprint Vent Visible", false);

        public static readonly CustomToggleOption RewindRevive = CustomOption.AddToggle("Revive During Rewind", false);
        public static readonly CustomNumberOption RewindDuration = CustomOption.AddNumber("Rewind Duration", 3f, 3f, 15f, 0.5f);
        public static readonly CustomNumberOption RewindCooldown = CustomOption.AddNumber("Rewind Cooldown", 25f, 10f, 40f, 2.5f);
        public static readonly CustomToggleOption ReviveExiled = CustomOption.AddToggle("Revive Voted Out", false);
        
        public static Func<CustomOption, object, string> PercentFormat { get; } = (sender, value) => $"{value:0}%";
        private static Func<CustomOption, object, string> VisionFormat { get; } = (sender, value) => $"{value:0.0#}x";
        private static Func<CustomOption, object, string> CooldownFormat { get; } = (sender, value) => $"{value:0.0#}s";


        public static List<CustomOption> AllOptions = new List<CustomOption>
        {
	        MayorOn, JesterOn, LoversOn, SheriffOn, MafiaOn, EngineerOn, SwapperOn, ShifterOn, InvestigatorOn, TimeMasterOn,
	        MayorVision, EngineerVision,
	        SheriffKillCd, JanitorCleanCd, ShifterCd,
	        MayorVoteBank,
	        FootprintInterval, FootprintDuration, AnonymousFootPrint, VentFootprintVisible,
	        RewindRevive, RewindDuration, RewindCooldown
        };
        
        
        public override void Load()
		{
			this._harmony = new Harmony("com.slushiegoose.townofus");

			CustomOption.ShamelessPlug = false;
			
			MayorOn.StringFormat = PercentFormat;
			JesterOn.StringFormat = PercentFormat;
			LoversOn.StringFormat = PercentFormat;
			SheriffOn.StringFormat = PercentFormat;
			MafiaOn.StringFormat = PercentFormat;
			EngineerOn.StringFormat = PercentFormat;
			SwapperOn.StringFormat = PercentFormat;
			ShifterOn.StringFormat = PercentFormat;
			InvestigatorOn.StringFormat = PercentFormat;
			TimeMasterOn.StringFormat = PercentFormat;

			MayorVision.StringFormat = VisionFormat;
			EngineerVision.StringFormat = VisionFormat;

			SheriffKillCd.StringFormat = CooldownFormat;
			JanitorCleanCd.StringFormat = CooldownFormat;
			ShifterCd.StringFormat = CooldownFormat;
			FootprintInterval.StringFormat = CooldownFormat;
			FootprintDuration.StringFormat = CooldownFormat;
			RewindDuration.StringFormat = CooldownFormat;
			RewindCooldown.StringFormat = CooldownFormat;
			
			

			JanitorClean = CreateSprite("TownOfUs.Resources.Janitor.png");
			EngineerFix = CreateSprite("TownOfUs.Resources.Engineer.png");
			EngineerArrow = CreateSprite("TownOfUs.Resources.EngineerArrow.png");
			SwapperSwitch = CreateSprite("TownOfUs.Resources.SwapperSwitch.png");
			SwapperSwitchDisabled = CreateSprite("TownOfUs.Resources.SwapperSwitchDisabled.png");
			Shift = CreateSprite("TownOfUs.Resources.Shift.png");
			Kill = CreateSprite("TownOfUs.Resources.Kill.png");
			Footprint = CreateSprite("TownOfUs.Resources.Footprint.png");
			Rewind = CreateSprite("TownOfUs.Resources.Rewind.png");
			NormalKill = CreateSprite("TownOfUs.Resources.NormalKill.png");
			GreyscaleKill = CreateSprite("TownOfUs.Resources.GreyscaleKill.png");
			
			PalettePatch.Load();
			ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();
			
			this._harmony.PatchAll();
			
		}
        
		private static Sprite CreateSprite(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var tex = GUIExtensions.CreateEmptyTexture();
			var imageStream = assembly.GetManifestResourceStream(name);
			var img = imageStream.ReadFully();
			tex.LoadImage(img);
			tex.DontDestroy();
			var sprite = tex.CreateSprite();
			sprite.DontDestroy();
			return sprite;
		}
		
		
		private Harmony _harmony;
	}
}
