// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Role
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD), "SetTasks")]
  public static class Role
  {
    public static void Postfix(FFGALNAPKCD __instance)
    {
      if (Object.op_Equality((Object) FFGALNAPKCD.get_LocalPlayer(), (Object) null))
        return;
      if (FFGALNAPKCD.get_LocalPlayer().isGodfather())
      {
        ABFBCNBODMA abfbcnbodma = (ABFBCNBODMA) new GameObject("GodfatherTask").AddComponent<ABFBCNBODMA>();
        ((Component) abfbcnbodma).get_transform().SetParent(((Component) __instance).get_transform(), false);
        abfbcnbodma.set_Text("[FF0000FF]Role: Godfather\nKill all the crewmates.[]");
        __instance.get_myTasks().Insert(0, (PILBGHDHJLH) abfbcnbodma);
      }
      if (FFGALNAPKCD.get_LocalPlayer().isMafioso())
      {
        ABFBCNBODMA abfbcnbodma = (ABFBCNBODMA) new GameObject("MafiosoTask").AddComponent<ABFBCNBODMA>();
        ((Component) abfbcnbodma).get_transform().SetParent(((Component) __instance).get_transform(), false);
        abfbcnbodma.set_Text("[FF0000FF]Role: Mafioso\nInherit the Godfather once they die.[]");
        __instance.get_myTasks().Insert(0, (PILBGHDHJLH) abfbcnbodma);
      }
      if (!FFGALNAPKCD.get_LocalPlayer().isJanitor())
        return;
      ABFBCNBODMA abfbcnbodma1 = (ABFBCNBODMA) new GameObject("JanitorTask").AddComponent<ABFBCNBODMA>();
      ((Component) abfbcnbodma1).get_transform().SetParent(((Component) __instance).get_transform(), false);
      abfbcnbodma1.set_Text("[FF0000FF]Role: Janitor\nClean up bodies.[]");
      __instance.get_myTasks().Insert(0, (PILBGHDHJLH) abfbcnbodma1);
    }
  }
}
