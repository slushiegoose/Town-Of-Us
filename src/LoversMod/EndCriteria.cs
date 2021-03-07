// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.EndCriteria
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (HLBNNHFCNAJ), "ECNNLBKGIKE")]
  public static class EndCriteria
  {
    public static bool LoveCoupleWins;
    public static bool NobodyWins;

    [HarmonyPriority(0)]
    public static bool Prefix(HLBNNHFCNAJ __instance)
    {
      if (TownOfUs.JesterMod.EndCriteria.JesterVotedOut)
        return false;
      if (Object.op_Inequality((Object) Utils.Lover1, (Object) null) && Object.op_Inequality((Object) Utils.Lover2, (Object) null))
      {
        if (__instance.get_Systems().ContainsKey((LJFDDJHBOGF) 8) && (double) ((PPIIPAAMDAD) ((Il2CppObjectBase) __instance.get_Systems().get_Item((LJFDDJHBOGF) 8)).Cast<PPIIPAAMDAD>()).get_HMJFAFANEEL() < 0.0 || __instance.get_Systems().ContainsKey((LJFDDJHBOGF) 21) && (double) ((KJKDNMBDHKJ) ((Il2CppObjectBase) __instance.get_Systems().get_Item((LJFDDJHBOGF) 21)).Cast<KJKDNMBDHKJ>()).get_HMJFAFANEEL() < 0.0 || __instance.get_Systems().ContainsKey((LJFDDJHBOGF) 3) && (double) ((KJKDNMBDHKJ) ((Il2CppObjectBase) __instance.get_Systems().get_Item((LJFDDJHBOGF) 3)).Cast<KJKDNMBDHKJ>()).get_HMJFAFANEEL() < 0.0)
          return true;
        if (Methods.FourPeopleLeft())
          return false;
      }
      if (Methods.CheckLoversWin())
      {
        MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 56, (SendOption) 1, -1);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
        Methods.LoversWin();
        HLBNNHFCNAJ.PLBGOMIEONF((AIMMJPEOPEC) 2, false);
        return false;
      }
      if (!Methods.CheckNoImpsNoCrews())
        return true;
      MessageWriter messageWriter1 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 59, (SendOption) 1, -1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter1);
      Methods.NobodyWins();
      HLBNNHFCNAJ.PLBGOMIEONF((AIMMJPEOPEC) 2, false);
      return false;
    }
  }
}
