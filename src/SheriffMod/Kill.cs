// Decompiled with JetBrains decompiler
// Type: TownOfUs.SheriffMod.Kill
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using Il2CppSystem.Reflection;
using System;
using UnhollowerBaseLib;

namespace TownOfUs.SheriffMod
{
  [HarmonyPatch(typeof (MLPJGKEACMM))]
  public static class Kill
  {
    [HarmonyPatch("PerformKill")]
    private static bool Prefix(MethodBase __originalMethod)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isSheriff())
        return true;
      if (!FFGALNAPKCD.get_LocalPlayer().get_CanMove() || (double) Methods.SheriffKillTimer() != 0.0 || Methods.getDistBetweenPlayers(FFGALNAPKCD.get_LocalPlayer(), Methods.ClosestPlayer) >= (double) ((Il2CppArrayBase<float>) KMOGFLPJLLK.get_JMLGACIOLIK()).get_Item(FFGALNAPKCD.get_GameOptions().get_DLIBONBKPKL()))
        return false;
      if (!Methods.ClosestPlayer.get_Data().get_DAPKNDBLKIA())
      {
        MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 61, (SendOption) 1, -1);
        messageWriter.Write(FFGALNAPKCD.get_LocalPlayer().get_PlayerId());
        messageWriter.Write(FFGALNAPKCD.get_LocalPlayer().get_PlayerId());
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
        FFGALNAPKCD.get_LocalPlayer().MurderPlayer(FFGALNAPKCD.get_LocalPlayer());
      }
      else
      {
        MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 61, (SendOption) 1, -1);
        messageWriter.Write(FFGALNAPKCD.get_LocalPlayer().get_PlayerId());
        messageWriter.Write(Methods.ClosestPlayer.get_PlayerId());
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
        FFGALNAPKCD.get_LocalPlayer().MurderPlayer(Methods.ClosestPlayer);
      }
      Methods.LastKilled = DateTime.UtcNow;
      return false;
    }
  }
}
