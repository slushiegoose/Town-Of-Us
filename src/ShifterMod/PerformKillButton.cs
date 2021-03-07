// Decompiled with JetBrains decompiler
// Type: TownOfUs.ShifterMod.PerformKillButton
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
  [HarmonyPatch(typeof (MLPJGKEACMM), "PerformKill")]
  public class PerformKillButton
  {
    public static bool Prefix(MLPJGKEACMM __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isShifter())
        return true;
      if (!FFGALNAPKCD.get_LocalPlayer().get_CanMove() || (double) Methods.ShifterShiftTimer() != 0.0)
        return false;
      float num = ((Il2CppArrayBase<float>) KMOGFLPJLLK.get_JMLGACIOLIK()).get_Item(FFGALNAPKCD.get_GameOptions().get_DLIBONBKPKL());
      if ((double) Vector2.Distance(Methods.ClosestPlayer.GetTruePosition(), FFGALNAPKCD.get_LocalPlayer().GetTruePosition()) > (double) num)
        return false;
      byte playerId = Methods.ClosestPlayer.get_PlayerId();
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 68, (SendOption) 1, -1);
      messageWriter.Write(playerId);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      PerformKillButton.Shift(FFGALNAPKCD.get_LocalPlayer(), Methods.ClosestPlayer);
      return false;
    }

    public static void Shift(FFGALNAPKCD shifter, FFGALNAPKCD other)
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
