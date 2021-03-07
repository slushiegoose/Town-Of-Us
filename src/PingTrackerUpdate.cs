// Decompiled with JetBrains decompiler
// Type: TownOfUs.PingTrackerUpdate
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;

namespace TownOfUs
{
  [HarmonyPriority(700)]
  [HarmonyPatch(typeof (ELDIDNABIPI), "Update")]
  public static class PingTrackerUpdate
  {
    public static void Postfix(ELDIDNABIPI __instance)
    {
      AELDHKGBIFD text = __instance.get_text();
      text.set_Text(text.get_Text() + "\n[00FF00FF]TownOfUs Mod\nv1.0.3\nBy slushiegoose []");
    }
  }
}
