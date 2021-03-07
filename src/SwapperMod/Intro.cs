// Decompiled with JetBrains decompiler
// Type: TownOfUs.SwapperMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SwapperMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isSwapper())
        return;
      __instance.get___this().get_Title().set_Text("Swapper");
      __instance.get___this().get_Title().set_Color(new Color(0.4f, 0.9f, 0.4f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Swap the votes of two people");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(0.4f, 0.9f, 0.4f, 1f));
    }
  }
}
