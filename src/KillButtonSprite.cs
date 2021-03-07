// Decompiled with JetBrains decompiler
// Type: TownOfUs.KillButtonSprite
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
  [HarmonyPatch(typeof (PIEFJFEOGOL))]
  public class KillButtonSprite
  {
    private static Sprite Janitor => TownOfUs.TownOfUs.JanitorClean;

    private static Sprite Shift => TownOfUs.TownOfUs.Shift;

    private static Sprite Kill => TownOfUs.TownOfUs.Kill;

    private static Sprite Rewind => TownOfUs.TownOfUs.Rewind;

    [HarmonyPatch("Update")]
    public static void Postfix(PIEFJFEOGOL __instance)
    {
      bool flag = false;
      if (FFGALNAPKCD.get_LocalPlayer().isJanitor())
      {
        __instance.get_KillButton().get_renderer().set_sprite(KillButtonSprite.Janitor);
        flag = true;
      }
      else if (FFGALNAPKCD.get_LocalPlayer().isShifter())
      {
        __instance.get_KillButton().get_renderer().set_sprite(KillButtonSprite.Shift);
        flag = true;
      }
      else if (FFGALNAPKCD.get_LocalPlayer().isTimeMaster())
        __instance.get_KillButton().get_renderer().set_sprite(KillButtonSprite.Rewind);
      else
        __instance.get_KillButton().get_renderer().set_sprite(KillButtonSprite.Kill);
      if (((!Input.GetKeyInt((KeyCode) 113) ? 0 : (Object.op_Inequality((Object) __instance.get_KillButton(), (Object) null) ? 1 : 0)) & (flag ? 1 : 0)) == 0)
        return;
      __instance.get_KillButton().PerformKill();
    }
  }
}
