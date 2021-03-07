// Decompiled with JetBrains decompiler
// Type: TownOfUs.EngineerMod.Button
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.EngineerMod
{
  [HarmonyPatch(typeof (COIODMJFKDF))]
  public class Button
  {
    [HarmonyPatch("Update")]
    public static void Postfix(COIODMJFKDF __instance)
    {
      if (__instance.get_DCCFKHIPIOF() != 1 || !PerformKill.UsedThisRound)
        return;
      __instance.get_StatusText().set_Text("AN ENGINEER HAS\nUSED THEIR FIX\nFOR THIS ROUND.");
      __instance.get_NumberText().set_Text(string.Empty);
      __instance.set_ButtonActive(false);
      ((Component) __instance.get_ClosedLid()).get_gameObject().SetActive(true);
      ((Component) __instance.get_OpenLid()).get_gameObject().SetActive(false);
    }
  }
}
