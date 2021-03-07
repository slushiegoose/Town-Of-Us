// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.Role
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD), "SetTasks")]
  public static class Role
  {
    public static void Postfix(FFGALNAPKCD __instance)
    {
      if (Object.op_Equality((Object) FFGALNAPKCD.get_LocalPlayer(), (Object) null) || !__instance.isLover())
        return;
      ABFBCNBODMA task = (ABFBCNBODMA) new GameObject("LoversTask").AddComponent<ABFBCNBODMA>();
      ((Component) task).get_transform().SetParent(((Component) __instance).get_transform(), false);
      task.GenTaskText(__instance);
      __instance.get_myTasks().Insert(0, (PILBGHDHJLH) task);
    }

    public static void GenTaskText(this ABFBCNBODMA task, FFGALNAPKCD __instance)
    {
      string str = __instance.get_Data().get_DAPKNDBLKIA() ? "Loving [FF0000FF]Impostor[FF80D5FF]" : "Lover";
      FFGALNAPKCD ffgalnapkcd = __instance.OtherLover();
      task.set_Text("[FF80D5FF]Role: " + str + "\nStay alive with your love " + ((Object) ffgalnapkcd).get_name() + " \nand win together[]");
    }
  }
}
