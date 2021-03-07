// Decompiled with JetBrains decompiler
// Type: TownOfUs.InvestigatorMod.StartGame
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;

namespace TownOfUs.InvestigatorMod
{
  [HarmonyPatch(typeof (HLBNNHFCNAJ), "Start")]
  public static class StartGame
  {
    public static void Postfix(HLBNNHFCNAJ __instance) => AddPrints.GameStarted = true;
  }
}
