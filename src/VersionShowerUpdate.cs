// Decompiled with JetBrains decompiler
// Type: TownOfUs.VersionShowerUpdate
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;

namespace TownOfUs
{
  [HarmonyPriority(700)]
  [HarmonyPatch(typeof (BOCOFLHKCOJ), "Start")]
  public static class VersionShowerUpdate
  {
    public static void Postfix(BOCOFLHKCOJ __instance)
    {
      AELDHKGBIFD text = __instance.get_text();
      text.set_Text(text.get_Text() + "\n\n\n\n\n\n\n[00FF00FF]loaded TownOfUs Mod v1.0.3 by slushiegoose[]");
    }
  }
}
