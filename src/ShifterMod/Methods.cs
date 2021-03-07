// Decompiled with JetBrains decompiler
// Type: TownOfUs.ShifterMod.Methods
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using Il2CppSystem.Collections.Generic;
using System;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
  public class Methods
  {
    public static FFGALNAPKCD ClosestPlayer;
    public static DateTime LastShifted;

    public static float ShifterShiftTimer()
    {
      TimeSpan timeSpan = DateTime.UtcNow - Methods.LastShifted;
      float num = CustomGameOptions.SheriffKillCd * 1000f;
      return (double) num - timeSpan.TotalMilliseconds < 0.0 ? 0.0f : (float) (((double) num - timeSpan.TotalMilliseconds) / 1000.0);
    }

    public static FFGALNAPKCD getClosestPlayer(FFGALNAPKCD refplayer)
    {
      double num = double.MaxValue;
      FFGALNAPKCD ffgalnapkcd = (FFGALNAPKCD) null;
      List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator.MoveNext())
      {
        FFGALNAPKCD current = enumerator.get_Current();
        if (!current.get_Data().get_DLPCKPBIJOE() && (int) current.get_PlayerId() != (int) refplayer.get_PlayerId())
        {
          double distBetweenPlayers = Methods.getDistBetweenPlayers(current, refplayer);
          if (distBetweenPlayers < num)
          {
            num = distBetweenPlayers;
            ffgalnapkcd = current;
          }
        }
      }
      return ffgalnapkcd;
    }

    public static double getDistBetweenPlayers(FFGALNAPKCD player, FFGALNAPKCD refplayer) => (double) Vector2.Distance(refplayer.GetTruePosition(), player.GetTruePosition());
  }
}
