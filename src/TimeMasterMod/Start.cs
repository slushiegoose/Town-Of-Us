// Decompiled with JetBrains decompiler
// Type: TownOfUs.TimeMasterMod.Start
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using System;

namespace TownOfUs.TimeMasterMod
{
  [HarmonyPatch(typeof (HLBNNHFCNAJ))]
  public static class Start
  {
    [HarmonyPatch("Start")]
    public static void Postfix(HLBNNHFCNAJ __instance)
    {
      Methods.FinishRewind = DateTime.UtcNow;
      Methods.StartRewind = DateTime.UtcNow;
      Methods.FinishRewind = Methods.FinishRewind.AddSeconds(-10.0);
      Methods.StartRewind = Methods.StartRewind.AddSeconds(-20.0);
    }
  }
}
