// Decompiled with JetBrains decompiler
// Type: TownOfUs.JesterMod.RemoveTasks
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.JesterMod
{
  [HarmonyPatch(typeof (PIEFJFEOGOL), "Update")]
  public class RemoveTasks
  {
    public static void Postfix(PIEFJFEOGOL __instance)
    {
      if (Object.op_Equality((Object) Utils.Jester, (Object) null))
        return;
      List<PILBGHDHJLH> pilbghdhjlhList = new List<PILBGHDHJLH>();
      List<PILBGHDHJLH>.Enumerator enumerator1 = Utils.Jester.get_myTasks().GetEnumerator();
      while (enumerator1.MoveNext())
      {
        PILBGHDHJLH current = enumerator1.get_Current();
        if (current.get_TaskType() != 19 && current.get_TaskType() != 17 && Object.op_Equality((Object) ((Component) current).get_gameObject().GetComponent<ABFBCNBODMA>(), (Object) null))
          pilbghdhjlhList.Add(current);
      }
      using (List<PILBGHDHJLH>.Enumerator enumerator2 = pilbghdhjlhList.GetEnumerator())
      {
        while (enumerator2.MoveNext())
        {
          PILBGHDHJLH current = enumerator2.Current;
          Utils.Jester.RemoveTask(current);
        }
      }
    }
  }
}
