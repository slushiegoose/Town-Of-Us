// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO))]
  public static class Intro
  {
    [HarmonyPatch("MoveNext")]
    public static bool Prefix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isLover())
        return true;
      List<FFGALNAPKCD> list = new List<FFGALNAPKCD>();
      list.Add(FFGALNAPKCD.get_LocalPlayer());
      list.Add(FFGALNAPKCD.get_LocalPlayer().OtherLover());
      __instance.set_yourTeam(list);
      return true;
    }

    [HarmonyPatch("MoveNext")]
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isLover())
        return;
      __instance.get___this().get_Title().set_Text("Lover");
      if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DAPKNDBLKIA())
      {
        __instance.get___this().get_Title().set_Text("Loving Impostor");
        AELDHKGBIFD title = __instance.get___this().get_Title();
        title.set_scale(title.get_scale() / 2.3f);
      }
      __instance.get___this().get_Title().set_Color(new Color(1f, 0.4f, 0.8f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("You are in [FF66CCFF]Love [FFFFFFFF]with [FF66CCFF]" + ((Object) FFGALNAPKCD.get_LocalPlayer().OtherLover()).get_name());
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(1f, 0.4f, 0.8f, 1f));
    }
  }
}
