// Decompiled with JetBrains decompiler
// Type: TownOfUs.TimeMasterMod.PerformKillButton
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;

namespace TownOfUs.TimeMasterMod
{
  [HarmonyPatch(typeof (MLPJGKEACMM), "PerformKill")]
  public class PerformKillButton
  {
    public static bool Prefix(MLPJGKEACMM __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isTimeMaster())
        return true;
      if (!FFGALNAPKCD.get_LocalPlayer().get_CanMove() || !((double) Methods.TimeMasterRewindTimer() == 0.0 & !RecordRewind.rewinding))
        return false;
      StartStop.StartRewind();
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 69, (SendOption) 1, -1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      return false;
    }
  }
}
