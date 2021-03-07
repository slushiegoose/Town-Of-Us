// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Mafioso.UseButton
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Mafioso
{
  [HarmonyPatch(typeof (LJKIJNFBBHH))]
  public class UseButton
  {
    [HarmonyPatch("SetTarget")]
    public static void Postfix(LJKIJNFBBHH __instance)
    {
      if (FFGALNAPKCD.get_AllPlayerControls().get_Count() <= 1 || !FFGALNAPKCD.get_LocalPlayer().isMafioso() || Utils.Godfather.get_Data().get_DLPCKPBIJOE() || Object.op_Inequality((Object) __instance.get_UseButton().get_sprite(), (Object) __instance.get_SabotageImage()))
        return;
      __instance.get_UseButton().set_sprite(__instance.get_UseImage());
      __instance.get_UseButton().set_color(LJKIJNFBBHH.get_AEJDBEHFKGE());
    }

    [HarmonyPatch("DoClick")]
    public static bool Prefix(LJKIJNFBBHH __instance)
    {
      if (FFGALNAPKCD.get_AllPlayerControls().get_Count() <= 1 || !FFGALNAPKCD.get_LocalPlayer().isMafioso() || Utils.Godfather.get_Data().get_DLPCKPBIJOE())
        return true;
      EGLJNOMOGNP.DCJMABDDJCF data = FFGALNAPKCD.get_LocalPlayer().get_Data();
      return __instance.get_LLEGHLMEGHN() != null || (data == null || !data.get_DAPKNDBLKIA());
    }
  }
}
