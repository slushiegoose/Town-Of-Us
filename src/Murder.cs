// Decompiled with JetBrains decompiler
// Type: TownOfUs.Murder
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;

namespace TownOfUs
{
  [HarmonyPatch(typeof (FFGALNAPKCD))]
  public static class Murder
  {
    [HarmonyPatch("MurderPlayer")]
    public static void Prefix(FFGALNAPKCD __instance, [HarmonyArgument(0)] FFGALNAPKCD opponent)
    {
      if (__instance.isSheriff())
        __instance.get_Data().set_DAPKNDBLKIA(true);
      if (!__instance.isShifter())
        return;
      __instance.get_Data().set_DAPKNDBLKIA(true);
    }

    [HarmonyPatch("MurderPlayer")]
    public static void Postfix(FFGALNAPKCD __instance, [HarmonyArgument(0)] FFGALNAPKCD opponent)
    {
      if (!__instance.isSheriff() && !__instance.isShifter())
        return;
      __instance.get_Data().set_DAPKNDBLKIA(false);
    }
  }
}
