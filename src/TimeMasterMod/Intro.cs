// Decompiled with JetBrains decompiler
// Type: TownOfUs.TimeMasterMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isTimeMaster())
        return;
      __instance.get___this().get_Title().set_Text("Time Master");
      AELDHKGBIFD title = __instance.get___this().get_Title();
      title.set_scale(title.get_scale() / 1.4f);
      __instance.get___this().get_Title().set_Color(new Color(0.0f, 0.0f, 1f, 1f));
      __instance.get___this().get_ImpostorText().set_Text("Rewind time");
      ((Renderer) __instance.get___this().get_BackgroundBar()).get_material().set_color(new Color(0.0f, 0.0f, 1f, 1f));
    }
  }
}
