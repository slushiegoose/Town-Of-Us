// Decompiled with JetBrains decompiler
// Type: TownOfUs.EngineerMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isEngineer())
        return;
      __instance.get___this().get_Title().set_Text("Engineer");
      __instance.get___this().get_Title().set_Color(new Color(1f, 0.7f, 0.0f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Help fix sabotages");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(1f, 0.7f, 0.0f, 1f));
    }
  }
}
