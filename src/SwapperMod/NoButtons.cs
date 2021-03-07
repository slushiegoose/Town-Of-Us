// Decompiled with JetBrains decompiler
// Type: TownOfUs.SwapperMod.NoButtons
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;

namespace TownOfUs.SwapperMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD), "OJPCECLPCCF")]
  public class NoButtons
  {
    public static void Postfix()
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isSwapper())
        return;
      FFGALNAPKCD.get_LocalPlayer().set_RemainingEmergencies(0);
    }
  }
}
