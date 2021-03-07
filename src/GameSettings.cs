// Decompiled with JetBrains decompiler
// Type: TownOfUs.GameSettings
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using Essentials.Options;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfUs
{
  [HarmonyPatch]
  public static class GameSettings
  {
    public static string GameSettingsText;
    private static float defaultBounds;

    public static string StringBuild()
    {
      StringBuilder stringBuilder = new StringBuilder("Roles:\n");
      using (List<CustomOption>.Enumerator enumerator = TownOfUs.TownOfUs.AllOptions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CustomOption current = enumerator.Current;
          stringBuilder.AppendLine((Delegate) current.StringFormat == (Delegate) TownOfUs.TownOfUs.PercentFormat ? string.Format("     {0}: {1}", (object) current.Name, (object) current) : string.Format("{0}: {1}", (object) current.Name, (object) current));
        }
      }
      return stringBuilder.ToString();
    }

    [HarmonyPatch(typeof (KMOGFLPJLLK), "CKKJMLEDCJB")]
    [HarmonyBefore(new string[] {"com.comando.essentials"})]
    public static class GameSettingsPatch1
    {
      public static void Postfix(ref string __result) => GameSettings.GameSettingsText = __result;
    }

    [HarmonyPatch(typeof (KMOGFLPJLLK), "CKKJMLEDCJB")]
    [HarmonyAfter(new string[] {"com.comando.essentials"})]
    public static class GameSettingsPatch2
    {
      public static void Postfix(ref string __result) => __result = GameSettings.GameSettingsText;
    }

    [HarmonyPatch(typeof (PFLIBLFPGGB), "FixedUpdate")]
    public static class LobbyFix
    {
      private static bool _isCustom;
      private static float _lastUpdated;

      public static bool Prefix(PFLIBLFPGGB __instance)
      {
        // ISSUE: unable to decompile the method.
      }
    }

    [HarmonyPatch(typeof (PIEFJFEOGOL), "Update")]
    [HarmonyAfter(new string[] {"com.comando.essentials"})]
    public static class FixScale
    {
      public static void Prefix(PIEFJFEOGOL __instance) => __instance.get_GameSettings().set_scale(0.6f);
    }

    [HarmonyPatch(typeof (PHCKLDDNJNP), "Start")]
    public static class Start
    {
      public static void Postfix(ref PHCKLDDNJNP __instance) => GameSettings.defaultBounds = ((CIAACBCIDFI) ((Component) __instance).GetComponentInParent<CIAACBCIDFI>()).get_YBounds().get_max();
    }

    [HarmonyPatch(typeof (PHCKLDDNJNP), "Update")]
    public static class Update
    {
      public static void Postfix(ref PHCKLDDNJNP __instance) => ((CIAACBCIDFI) ((Component) __instance).GetComponentInParent<CIAACBCIDFI>()).get_YBounds().set_max(20f);
    }
  }
}
