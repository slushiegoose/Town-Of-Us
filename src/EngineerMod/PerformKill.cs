// Decompiled with JetBrains decompiler
// Type: TownOfUs.EngineerMod.PerformKill
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;

namespace TownOfUs.EngineerMod
{
  [HarmonyPatch(typeof (MLPJGKEACMM), "PerformKill")]
  public class PerformKill
  {
    public static DateTime SabotageTime;
    public static bool UsedThisRound;

    public static bool Prefix(MLPJGKEACMM __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isEngineer())
        return true;
      if (!FFGALNAPKCD.get_LocalPlayer().get_CanMove() || PerformKill.UsedThisRound)
        return false;
      MPOMGOCBHNI mpomgocbhni = (MPOMGOCBHNI) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 17)).Cast<MPOMGOCBHNI>();
      Il2CppArrayBase<LGPMPFLCFBA> array = mpomgocbhni.get_DJIOLDAFJNO().ToArray();
      bool flag = mpomgocbhni.get_ICHIBHDMDGF().DHDPPNPJJGD();
      if (!((IEnumerable<LGPMPFLCFBA>) array).Any<LGPMPFLCFBA>((Func<LGPMPFLCFBA, bool>) (s => s.DHDPPNPJJGD())) | flag)
        return false;
      PerformKill.UsedThisRound = true;
      PerformKill.SabotageTime = DateTime.get_Now();
      switch ((int) HLBNNHFCNAJ.get_Instance().get_Type())
      {
        case 0:
          if (((FDNBHNIJKJI) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 14)).Cast<FDNBHNIJKJI>()).DHDPPNPJJGD())
            return PerformKill.FixComms();
          if (((KJKDNMBDHKJ) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 3)).Cast<KJKDNMBDHKJ>()).DHDPPNPJJGD())
            return PerformKill.FixReactor((LJFDDJHBOGF) 3);
          if (((PPIIPAAMDAD) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 8)).Cast<PPIIPAAMDAD>()).DHDPPNPJJGD())
            return PerformKill.FixOxygen();
          ILEEIKKPGLG lights1 = (ILEEIKKPGLG) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 7)).Cast<ILEEIKKPGLG>();
          if (lights1.DHDPPNPJJGD())
            return PerformKill.FixLights(lights1);
          break;
        case 1:
          if (((JJOKBJOEDCJ) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 14)).Cast<JJOKBJOEDCJ>()).DHDPPNPJJGD())
            return PerformKill.FixMiraComms();
          if (((KJKDNMBDHKJ) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 3)).Cast<KJKDNMBDHKJ>()).DHDPPNPJJGD())
            return PerformKill.FixReactor((LJFDDJHBOGF) 3);
          if (((PPIIPAAMDAD) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 8)).Cast<PPIIPAAMDAD>()).DHDPPNPJJGD())
            return PerformKill.FixOxygen();
          ILEEIKKPGLG lights2 = (ILEEIKKPGLG) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 7)).Cast<ILEEIKKPGLG>();
          if (lights2.DHDPPNPJJGD())
            return PerformKill.FixLights(lights2);
          break;
        case 2:
          if (((FDNBHNIJKJI) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 14)).Cast<FDNBHNIJKJI>()).DHDPPNPJJGD())
            return PerformKill.FixComms();
          if (((KJKDNMBDHKJ) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 21)).Cast<KJKDNMBDHKJ>()).DHDPPNPJJGD())
            return PerformKill.FixReactor((LJFDDJHBOGF) 21);
          ILEEIKKPGLG lights3 = (ILEEIKKPGLG) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 7)).Cast<ILEEIKKPGLG>();
          if (lights3.DHDPPNPJJGD())
            return PerformKill.FixLights(lights3);
          break;
      }
      return false;
    }

    private static bool FixComms()
    {
      HLBNNHFCNAJ.get_Instance().RpcRepairSystem((LJFDDJHBOGF) 14, 0);
      return false;
    }

    private static bool FixMiraComms()
    {
      HLBNNHFCNAJ.get_Instance().RpcRepairSystem((LJFDDJHBOGF) 14, 16);
      HLBNNHFCNAJ.get_Instance().RpcRepairSystem((LJFDDJHBOGF) 14, 17);
      return false;
    }

    private static bool FixReactor(LJFDDJHBOGF system)
    {
      HLBNNHFCNAJ.get_Instance().RpcRepairSystem(system, 16);
      return false;
    }

    private static bool FixOxygen()
    {
      HLBNNHFCNAJ.get_Instance().RpcRepairSystem((LJFDDJHBOGF) 8, 16);
      return false;
    }

    private static bool FixLights(ILEEIKKPGLG lights)
    {
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 63, (SendOption) 1, -1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      lights.set_LNOCHNPPIFJ(lights.get_HBLBMJBKOKA());
      return false;
    }
  }
}
