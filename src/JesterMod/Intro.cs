// Decompiled with JetBrains decompiler
// Type: TownOfUs.JesterMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.JesterMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO))]
  public static class Intro
  {
    [HarmonyPatch("MoveNext")]
    public static bool Prefix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isJester())
        return true;
      List<FFGALNAPKCD> list = new List<FFGALNAPKCD>();
      list.Add(FFGALNAPKCD.get_LocalPlayer());
      __instance.set_yourTeam(list);
      return true;
    }

    [HarmonyPatch("MoveNext")]
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isJester())
        return;
      __instance.get___this().get_Title().set_Text("Jester");
      __instance.get___this().get_Title().set_Color(new Color(1f, 0.75f, 0.8f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Get voted out");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(1f, 0.75f, 0.8f, 1f));
    }
  }
}
