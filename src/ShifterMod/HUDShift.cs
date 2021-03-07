// Decompiled with JetBrains decompiler
// Type: TownOfUs.ShifterMod.HUDShift
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;

namespace TownOfUs.ShifterMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD))]
  public class HUDShift
  {
    [HarmonyPatch("FixedUpdate")]
    public static void Postfix(FFGALNAPKCD __instance) => HUDShift.UpdateShiftButton(__instance);

    public static void UpdateShiftButton(FFGALNAPKCD __instance)
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
