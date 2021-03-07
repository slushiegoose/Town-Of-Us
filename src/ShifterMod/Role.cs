// Decompiled with JetBrains decompiler
// Type: TownOfUs.ShifterMod.Role
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD), "SetTasks")]
  public static class Role
  {
    public static void Postfix(FFGALNAPKCD __instance)
    {
      if (Object.op_Equality((Object) FFGALNAPKCD.get_LocalPlayer(), (Object) null) || !__instance.isShifter())
        return;
      ABFBCNBODMA abfbcnbodma = (ABFBCNBODMA) new GameObject("ShifterTask").AddComponent<ABFBCNBODMA>();
      ((Component) abfbcnbodma).get_transform().SetParent(((Component) __instance).get_transform(), false);
      abfbcnbodma.set_Text("[999999FF]Role: Shifter\nSteal other people's roles.[]");
      __instance.get_myTasks().Insert(0, (PILBGHDHJLH) abfbcnbodma);
    }
  }
}
