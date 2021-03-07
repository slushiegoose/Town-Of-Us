// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Mafioso.HUD
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Mafioso
{
  [HarmonyPatch(typeof (PIEFJFEOGOL), "Update")]
  public static class HUD
  {
    public static void Postfix(PIEFJFEOGOL __instance)
    {
      if (FFGALNAPKCD.get_AllPlayerControls().get_Count() <= 1 || !FFGALNAPKCD.get_LocalPlayer().isMafioso())
        return;
      MLPJGKEACMM killButton = __instance.get_KillButton();
      if (!Utils.Godfather.get_Data().get_DLPCKPBIJOE())
      {
        ((Component) killButton).get_gameObject().SetActive(false);
        killButton.set_isActive(false);
      }
      else
      {
        if (Utils.Mafioso.get_Data().get_DLPCKPBIJOE())
          return;
        ((Component) killButton).get_gameObject().SetActive(true);
        killButton.set_isActive(true);
      }
    }
  }
}
