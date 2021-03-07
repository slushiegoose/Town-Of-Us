// Decompiled with JetBrains decompiler
// Type: TownOfUs.SheriffMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.SheriffMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isSheriff())
        return;
      __instance.get___this().get_Title().set_Text("Sheriff");
      __instance.get___this().get_Title().set_Color(new Color(1f, 1f, 0.0f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Shoot the [FF0000FF]Impostor");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(1f, 1f, 0.0f, 1f));
    }
  }
}
