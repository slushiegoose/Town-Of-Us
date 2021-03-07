// Decompiled with JetBrains decompiler
// Type: TownOfUs.ShifterMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isShifter())
        return;
      __instance.get___this().get_Title().set_Text("Shifter");
      __instance.get___this().get_Title().set_Color(new Color(0.6f, 0.6f, 0.6f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Shift around different roles");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(0.6f, 0.6f, 0.6f, 1f));
    }
  }
}
