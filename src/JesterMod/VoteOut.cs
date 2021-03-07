// Decompiled with JetBrains decompiler
// Type: TownOfUs.JesterMod.VoteOut
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.JesterMod
{
  [HarmonyPatch(typeof (CNNGMDOPELD), "Begin")]
  public static class VoteOut
  {
    public static void Postfix([HarmonyArgument(0)] EGLJNOMOGNP.DCJMABDDJCF exiled, CNNGMDOPELD __instance)
    {
      if (Object.op_Equality((Object) Utils.Jester, (Object) null) || exiled == null || (int) exiled.get_JKOMCOJCAID() != (int) Utils.Jester.get_PlayerId())
        return;
      EndCriteria.JesterVotedOut = true;
      VoteOut.JesterWins();
    }

    private static void JesterWins()
    {
      List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator.MoveNext())
      {
        FFGALNAPKCD current = enumerator.get_Current();
        current.get_Data().set_DAPKNDBLKIA(current.isJester());
      }
    }
  }
}
