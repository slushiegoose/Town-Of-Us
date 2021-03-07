// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.NameChange
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.MafiaMod
{
  [HarmonyPatch(typeof (PIEFJFEOGOL))]
  public class NameChange
  {
    private static void UpdateMeeting(OOCJALPKPEP __instance)
    {
      if (FFGALNAPKCD.get_AllPlayerControls().get_Count() <= 1 || !FFGALNAPKCD.get_LocalPlayer().get_Data().get_DAPKNDBLKIA() || (Object.op_Equality((Object) Utils.Godfather, (Object) null) || Object.op_Equality((Object) Utils.Mafioso, (Object) null) || Object.op_Equality((Object) Utils.Janitor, (Object) null)))
        return;
      using (IEnumerator<HDJGDMFCHDN> enumerator = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          HDJGDMFCHDN current = enumerator.Current;
          string text = current.get_NameText().get_Text();
          if (text == ((Object) Utils.Godfather).get_name() || text == ((Object) Utils.Godfather).get_name() + " (G)")
            current.get_NameText().set_Text(((Object) Utils.Godfather).get_name() + " (G)");
          if (text == ((Object) Utils.Mafioso).get_name() || text == ((Object) Utils.Mafioso).get_name() + " (M)")
            current.get_NameText().set_Text(((Object) Utils.Mafioso).get_name() + " (M)");
          if (text == ((Object) Utils.Janitor).get_name() || text == ((Object) Utils.Janitor).get_name() + " (J)")
            current.get_NameText().set_Text(((Object) Utils.Janitor).get_name() + " (J)");
        }
      }
    }

    [HarmonyPatch("Update")]
    public static void Postfix(PIEFJFEOGOL __instance)
    {
      if (Object.op_Inequality((Object) OOCJALPKPEP.get_Instance(), (Object) null))
        NameChange.UpdateMeeting(OOCJALPKPEP.get_Instance());
      if (FFGALNAPKCD.get_AllPlayerControls().get_Count() <= 1 || (!Object.op_Inequality((Object) Utils.Godfather, (Object) null) || !Object.op_Inequality((Object) Utils.Mafioso, (Object) null) || !Object.op_Inequality((Object) Utils.Janitor, (Object) null) || !FFGALNAPKCD.get_LocalPlayer().get_Data().get_DAPKNDBLKIA()))
        return;
      List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator.MoveNext())
      {
        FFGALNAPKCD current = enumerator.get_Current();
        if (current.isGodfather())
          current.get_nameText().set_Text(((Object) Utils.Godfather).get_name() + " (G)");
        if (current.isMafioso())
          current.get_nameText().set_Text(((Object) Utils.Mafioso).get_name() + " (M)");
        if (current.isJanitor())
          current.get_nameText().set_Text(((Object) Utils.Janitor).get_name() + " (J)");
      }
    }
  }
}
