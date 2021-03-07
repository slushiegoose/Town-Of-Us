// Decompiled with JetBrains decompiler
// Type: TownOfUs.MayorMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isMayor())
        return;
      __instance.get___this().get_Title().set_Text("Mayor");
      __instance.get___this().get_Title().set_Color(new Color(0.44f, 0.31f, 0.66f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Save your votes to double vote");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(0.44f, 0.31f, 0.66f, 1f));
    }
  }
}
