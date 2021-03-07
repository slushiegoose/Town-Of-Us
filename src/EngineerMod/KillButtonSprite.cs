// Decompiled with JetBrains decompiler
// Type: TownOfUs.EngineerMod.KillButtonSprite
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
  [HarmonyPatch(typeof (PIEFJFEOGOL))]
  public class KillButtonSprite
  {
    private static Sprite Sprite => TownOfUs.TownOfUs.EngineerFix;

    [HarmonyPatch("Update")]
    public static void Postfix(PIEFJFEOGOL __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isEngineer() || Object.op_Equality((Object) __instance.get_KillButton(), (Object) null))
        return;
      __instance.get_KillButton().get_renderer().set_sprite(KillButtonSprite.Sprite);
      __instance.get_KillButton().SetCoolDown(0.0f, 10f);
      ((Component) __instance.get_KillButton()).get_gameObject().SetActive(!FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE());
      if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE())
        return;
      MPOMGOCBHNI mpomgocbhni = (MPOMGOCBHNI) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 17)).Cast<MPOMGOCBHNI>();
      Il2CppArrayBase<LGPMPFLCFBA> array = mpomgocbhni.get_DJIOLDAFJNO().ToArray();
      bool flag1 = mpomgocbhni.get_ICHIBHDMDGF().DHDPPNPJJGD();
      bool flag2 = ((IEnumerable<LGPMPFLCFBA>) array).Any<LGPMPFLCFBA>((Func<LGPMPFLCFBA, bool>) (s => s.DHDPPNPJJGD()));
      SpriteRenderer renderer = __instance.get_KillButton().get_renderer();
      if (flag2 & !flag1 & !PerformKill.UsedThisRound)
      {
        renderer.set_color(LOCPGOACAJF.get_AGOLMCOBHAF());
        ((Renderer) renderer).get_material().SetFloat("_Desat", 0.0f);
      }
      else
      {
        renderer.set_color(LOCPGOACAJF.get_AEJDBEHFKGE());
        ((Renderer) renderer).get_material().SetFloat("_Desat", 1f);
      }
    }
  }
}
