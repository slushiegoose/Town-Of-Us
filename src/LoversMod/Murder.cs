// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.Murder
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD))]
  public static class Murder
  {
    [HarmonyPatch("MurderPlayer")]
    public static void Prefix(FFGALNAPKCD __instance, [HarmonyArgument(0)] FFGALNAPKCD opponent)
    {
      if (!__instance.isLover())
        return;
      __instance.get_Data().set_DAPKNDBLKIA(true);
    }

    [HarmonyPatch("MurderPlayer")]
    public static void Postfix(FFGALNAPKCD __instance, [HarmonyArgument(0)] FFGALNAPKCD opponent)
    {
      if (__instance.isLover1())
      {
        __instance.get_Data().set_DAPKNDBLKIA(false);
      }
      else
      {
        if (!(__instance.isLover2() & !Utils.LoverImpostor))
          return;
        __instance.get_Data().set_DAPKNDBLKIA(false);
      }
    }
  }
}
