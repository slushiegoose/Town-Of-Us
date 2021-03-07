// Decompiled with JetBrains decompiler
// Type: TownOfUs.SwapperMod.NoSabotage
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace TownOfUs.SwapperMod
{
  [HarmonyPatch(typeof (OKACGFOHADL), "GJCKLEFEIJB")]
  public class NoSabotage
  {
    public static bool Prefix(OKACGFOHADL __instance, [HarmonyArgument(0)] FFGALNAPKCD pc, ref PILBGHDHJLH __result)
    {
      if (!pc.isSwapper())
        return true;
      List<PILBGHDHJLH>.Enumerator enumerator = pc.get_myTasks().GetEnumerator();
      while (enumerator.MoveNext())
      {
        PILBGHDHJLH current = enumerator.get_Current();
        if (!current.get_IsComplete() && current.ValidConsole(__instance))
        {
          int num = current.get_TaskType() == 17 ? 1 : (current.get_TaskType() == 19 ? 1 : 0);
          __result = num == 0 ? current : (PILBGHDHJLH) null;
          return false;
        }
      }
      __result = (PILBGHDHJLH) null;
      return false;
    }
  }
}
