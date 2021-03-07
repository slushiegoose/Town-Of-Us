// Decompiled with JetBrains decompiler
// Type: TownOfUs.TownOfUs
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using BepInEx;
using BepInEx.IL2CPP;
using Essentials.Options;
using HarmonyLib;
using Reactor.Extensions;
using Reactor.Unstrip;
using System;
using System.Collections.Generic;
using System.Reflection;
using TownOfUs.RainbowMod;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace TownOfUs
{
  [BepInPlugin("com.slushiegoose.townofus", "Town Of Us", "1.0.3")]
  [BepInDependency]
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
    public static readonly CustomNumberOption MayorOn = CustomOption.AddNumber("Mayor", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption JesterOn = CustomOption.AddNumber("Jester", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption LoversOn = CustomOption.AddNumber("Lovers", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption SheriffOn = CustomOption.AddNumber("Sheriff", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption MafiaOn = CustomOption.AddNumber("Mafia", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption EngineerOn = CustomOption.AddNumber("Engineer", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption SwapperOn = CustomOption.AddNumber("Swapper", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption ShifterOn = CustomOption.AddNumber("Shifter", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption InvestigatorOn = CustomOption.AddNumber("Investigator", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomNumberOption TimeMasterOn = CustomOption.AddNumber("Time Master", 0.0f, 0.0f, 100f, 10f);
    public static readonly CustomToggleOption BothLoversDie = CustomOption.AddToggle("Both Lovers Die", true);
    public static readonly CustomToggleOption ShowSheriff = CustomOption.AddToggle("Show Sheriff", false);
    public static readonly CustomNumberOption SheriffKillCd = CustomOption.AddNumber("Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f);
    public static readonly CustomNumberOption JanitorCleanCd = CustomOption.AddNumber("Janitor Clean Cooldown", 25f, 10f, 50f, 2.5f);
    public static readonly CustomNumberOption MayorVision = CustomOption.AddNumber("Mayor Vision", 1f, 0.25f, 5f, 0.25f);
    public static readonly CustomNumberOption EngineerVision = CustomOption.AddNumber("Engineer Vision", 1f, 0.25f, 5f, 0.25f);
    public static readonly CustomNumberOption MayorVoteBank = CustomOption.AddNumber("Mayor Vote Bank", 1f, 1f, 5f, 1f);
    public static readonly CustomNumberOption ShifterCd = CustomOption.AddNumber("Shifter Cooldown", 25f, 10f, 40f, 2.5f);
    public static readonly CustomNumberOption FootprintSize = CustomOption.AddNumber("Footprint Size", 4f, 1f, 10f, 1f);
    public static readonly CustomNumberOption FootprintInterval = CustomOption.AddNumber("Footprint Interval", 1f, 0.25f, 5f, 0.25f);
    public static readonly CustomNumberOption FootprintDuration = CustomOption.AddNumber("Footprint Duration", 10f, 1f, 10f, 0.5f);
    public static readonly CustomToggleOption AnonymousFootPrint = CustomOption.AddToggle("Anonymous Footprint", false);
    public static readonly CustomToggleOption VentFootprintVisible = CustomOption.AddToggle("Footprint Vent Visible", false);
    public static readonly CustomToggleOption RewindRevive = CustomOption.AddToggle("Revive During Rewind", false);
    public static readonly CustomNumberOption RewindDuration = CustomOption.AddNumber("Rewind Duration", 3f, 3f, 15f, 0.5f);
    public static readonly CustomNumberOption RewindCooldown = CustomOption.AddNumber("Rewind Cooldown", 25f, 10f, 40f, 2.5f);
    public static readonly CustomToggleOption ReviveExiled = CustomOption.AddToggle("Revive Voted Out", false);
    public static List<CustomOption> AllOptions;
    private Harmony _harmony;

    public static Func<CustomOption, object, string> PercentFormat { get; } = (Func<CustomOption, object, string>) ((sender, value) => string.Format("{0:0}%", value));

    private static Func<CustomOption, object, string> VisionFormat { get; } = (Func<CustomOption, object, string>) ((sender, value) => string.Format("{0:0.0#}x", value));

    private static Func<CustomOption, object, string> CooldownFormat { get; } = (Func<CustomOption, object, string>) ((sender, value) => string.Format("{0:0.0#}s", value));

    public virtual void Load()
    {
      this._harmony = new Harmony("com.slushiegoose.townofus");
      CustomOption.ShamelessPlug = (__Null) 0;
      ((CustomOption) TownOfUs.TownOfUs.MayorOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.JesterOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.LoversOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.SheriffOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.MafiaOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.EngineerOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.SwapperOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.ShifterOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.InvestigatorOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.TimeMasterOn).StringFormat = (__Null) TownOfUs.TownOfUs.PercentFormat;
      ((CustomOption) TownOfUs.TownOfUs.MayorVision).StringFormat = (__Null) TownOfUs.TownOfUs.VisionFormat;
      ((CustomOption) TownOfUs.TownOfUs.EngineerVision).StringFormat = (__Null) TownOfUs.TownOfUs.VisionFormat;
      ((CustomOption) TownOfUs.TownOfUs.SheriffKillCd).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      ((CustomOption) TownOfUs.TownOfUs.JanitorCleanCd).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      ((CustomOption) TownOfUs.TownOfUs.ShifterCd).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      ((CustomOption) TownOfUs.TownOfUs.FootprintInterval).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      ((CustomOption) TownOfUs.TownOfUs.FootprintDuration).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      ((CustomOption) TownOfUs.TownOfUs.RewindDuration).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      ((CustomOption) TownOfUs.TownOfUs.RewindCooldown).StringFormat = (__Null) TownOfUs.TownOfUs.CooldownFormat;
      TownOfUs.TownOfUs.JanitorClean = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.Janitor.png");
      TownOfUs.TownOfUs.EngineerFix = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.Engineer.png");
      TownOfUs.TownOfUs.EngineerArrow = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.EngineerArrow.png");
      TownOfUs.TownOfUs.SwapperSwitch = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.SwapperSwitch.png");
      TownOfUs.TownOfUs.SwapperSwitchDisabled = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.SwapperSwitchDisabled.png");
      TownOfUs.TownOfUs.Shift = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.Shift.png");
      TownOfUs.TownOfUs.Kill = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.Kill.png");
      TownOfUs.TownOfUs.Footprint = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.Footprint.png");
      TownOfUs.TownOfUs.Rewind = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.Rewind.png");
      TownOfUs.TownOfUs.NormalKill = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.NormalKill.png");
      TownOfUs.TownOfUs.GreyscaleKill = TownOfUs.TownOfUs.CreateSprite("TownOfUs.Resources.GreyscaleKill.png");
      PalettePatch.Load();
      ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();
      this._harmony.PatchAll();
    }

    private static Sprite CreateSprite(string name)
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      Texture2D emptyTexture = GUIExtensions.CreateEmptyTexture(0, 0);
      byte[] numArray = Reactor.Extensions.Extensions.ReadFully(executingAssembly.GetManifestResourceStream(name));
      ImageConversion.LoadImage(emptyTexture, numArray, false);
      UnityObjectExtensions.DontDestroy<Texture2D>((M0) emptyTexture);
      Sprite sprite = GUIExtensions.CreateSprite(emptyTexture);
      UnityObjectExtensions.DontDestroy<Sprite>((M0) sprite);
      return sprite;
    }

    public TownOfUs() => base.\u002Ector();

    static TownOfUs()
    {
      List<CustomOption> customOptionList = new List<CustomOption>();
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.MayorOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.JesterOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.LoversOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.SheriffOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.MafiaOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.EngineerOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.SwapperOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.ShifterOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.InvestigatorOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.TimeMasterOn);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.MayorVision);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.EngineerVision);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.SheriffKillCd);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.JanitorCleanCd);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.ShifterCd);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.MayorVoteBank);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.FootprintInterval);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.FootprintDuration);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.AnonymousFootPrint);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.VentFootprintVisible);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.RewindRevive);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.RewindDuration);
      customOptionList.Add((CustomOption) TownOfUs.TownOfUs.RewindCooldown);
      TownOfUs.TownOfUs.AllOptions = customOptionList;
    }
  }
}
