// Decompiled with JetBrains decompiler
// Type: TownOfUs.JesterMod.EndCriteria
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using UnityEngine;

namespace TownOfUs.JesterMod
{
  [HarmonyPatch(typeof (HLBNNHFCNAJ), "ECNNLBKGIKE")]
  public static class EndCriteria
  {
    public static bool JesterVotedOut;

    public static bool Prefix(HLBNNHFCNAJ __instance)
    {
      if (Object.op_Equality((Object) Utils.Jester, (Object) null) || (!EndCriteria.JesterVotedOut || !Utils.Jester.get_Data().get_DLPCKPBIJOE()))
        return true;
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 55, (SendOption) 1, -1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      Utils.Jester.get_Data().set_DLPCKPBIJOE(false);
      HLBNNHFCNAJ.PLBGOMIEONF((AIMMJPEOPEC) 2, !IANFCOGHJMJ.NGKPBMCNCPO());
      return false;
    }
  }
}
