// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Intro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod
{
  [HarmonyPatch(typeof (PENEIDJGGAF.CKACLKCOJFO), "MoveNext")]
  public static class Intro
  {
    public static void Postfix(PENEIDJGGAF.CKACLKCOJFO __instance)
    {
      if (FFGALNAPKCD.get_LocalPlayer().isGodfather())
      {
        ((Component) __instance.get___this().get_ImpostorText()).get_gameObject().SetActive(true);
        __instance.get___this().get_Title().set_Text("Godfather");
        __instance.get___this().get_ImpostorText().set_Text("Kill all crewmates");
      }
      else if (FFGALNAPKCD.get_LocalPlayer().isMafioso())
      {
        ((Component) __instance.get___this().get_ImpostorText()).get_gameObject().SetActive(true);
        __instance.get___this().get_Title().set_Text("Mafioso");
        __instance.get___this().get_ImpostorText().set_Text("Work with the [FF0000FF]Mafia[] to kill the Crewmates");
      }
      else
      {
        if (!FFGALNAPKCD.get_LocalPlayer().isJanitor())
          return;
        ((Component) __instance.get___this().get_ImpostorText()).get_gameObject().SetActive(true);
        __instance.get___this().get_Title().set_Text("Janitor");
        __instance.get___this().get_ImpostorText().set_Text("Clean bodies to prevent Crewmates from discovering them.");
      }
    }
  }
}
