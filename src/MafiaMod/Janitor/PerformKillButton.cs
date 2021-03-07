// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Janitor.PerformKillButton
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using Reactor;
using System;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{
  [HarmonyPatch(typeof (MLPJGKEACMM), "PerformKill")]
  public class PerformKillButton
  {
    public static DateTime LastCleaned;

    public static bool Prefix(MLPJGKEACMM __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isJanitor())
        return true;
      if (!FFGALNAPKCD.get_LocalPlayer().get_CanMove() || (double) PerformKillButton.JanitorTimer() != 0.0)
        return false;
      float num = ((Il2CppArrayBase<float>) KMOGFLPJLLK.get_JMLGACIOLIK()).get_Item(FFGALNAPKCD.get_GameOptions().get_DLIBONBKPKL());
      if ((double) Vector2.Distance(KillButtonTarget.CurrentTarget.get_TruePosition(), FFGALNAPKCD.get_LocalPlayer().GetTruePosition()) > (double) num)
        return false;
      byte parentId = KillButtonTarget.CurrentTarget.get_ParentId();
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 62, (SendOption) 1, -1);
      messageWriter.Write(parentId);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      Coroutines.Start(Coroutine.CleanCoroutine(KillButtonTarget.CurrentTarget));
      return false;
    }

    public static float JanitorTimer()
    {
      TimeSpan timeSpan = DateTime.UtcNow - PerformKillButton.LastCleaned;
      float num = CustomGameOptions.JanitorCleanCd * 1000f;
      return (double) num - timeSpan.TotalMilliseconds < 0.0 ? 0.0f : (float) (((double) num - timeSpan.TotalMilliseconds) / 1000.0);
    }
  }
}
