// Decompiled with JetBrains decompiler
// Type: TownOfUs.JesterMod.ColourChange
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.JesterMod
{
  [HarmonyPatch(typeof (PIEFJFEOGOL))]
  public class ColourChange
  {
    private static void UpdateMeeting(OOCJALPKPEP __instance)
    {
      using (IEnumerator<HDJGDMFCHDN> enumerator = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          HDJGDMFCHDN current = enumerator.Current;
          if ((Object.op_Inequality((Object) Utils.Jester, (Object) null) && current.get_NameText().get_Text() == Utils.Jester.get_nameText().get_Text()) & FFGALNAPKCD.get_LocalPlayer().isJester())
            current.get_NameText().set_Color(new Color(1f, 0.75f, 0.8f, 1f));
        }
      }
    }

    [HarmonyPatch("Update")]
    public static void Postfix(PIEFJFEOGOL __instance)
    {
      if (Object.op_Inequality((Object) OOCJALPKPEP.get_Instance(), (Object) null))
        ColourChange.UpdateMeeting(OOCJALPKPEP.get_Instance());
      if (!(FFGALNAPKCD.get_AllPlayerControls().get_Count() > 1 & FFGALNAPKCD.get_LocalPlayer().isJester()))
        return;
      FFGALNAPKCD.get_LocalPlayer().get_nameText().set_Color(new Color(1f, 0.75f, 0.8f, 1f));
    }
  }
}
