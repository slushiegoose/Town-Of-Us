// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.Methods
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;

namespace TownOfUs.LoversMod
{
  public class Methods
  {
    public static bool CheckNoImpsNoCrews()
    {
      List<FFGALNAPKCD> list = ((IEnumerable<FFGALNAPKCD>) FFGALNAPKCD.get_AllPlayerControls().ToArray()).Where<FFGALNAPKCD>((Func<FFGALNAPKCD, bool>) (x => !x.get_Data().get_DLPCKPBIJOE())).ToList<FFGALNAPKCD>();
      return list.Count == 2 && ((IEnumerable<FFGALNAPKCD>) list).Where<FFGALNAPKCD>(new Func<FFGALNAPKCD, bool>(Utils.isJester)).Any<FFGALNAPKCD>() & ((IEnumerable<FFGALNAPKCD>) list).Where<FFGALNAPKCD>(new Func<FFGALNAPKCD, bool>(Utils.isShifter)).Any<FFGALNAPKCD>();
    }

    public static bool FourPeopleLeft()
    {
      Il2CppArrayBase<FFGALNAPKCD> array = FFGALNAPKCD.get_AllPlayerControls().ToArray();
      IEnumerable<FFGALNAPKCD> source = ((IEnumerable<FFGALNAPKCD>) array).Where<FFGALNAPKCD>((Func<FFGALNAPKCD, bool>) (x => !x.get_Data().get_DLPCKPBIJOE()));
      return ((IEnumerable<FFGALNAPKCD>) array).Where<FFGALNAPKCD>(new Func<FFGALNAPKCD, bool>(Utils.isLover)).Count<FFGALNAPKCD>() >= 2 && !Utils.Lover1.get_Data().get_DLPCKPBIJOE() && (!Utils.Lover2.get_Data().get_DLPCKPBIJOE() && source.Count<FFGALNAPKCD>() == 4) && Utils.LoverImpostor;
    }

    public static bool CheckLoversWin()
    {
      Il2CppArrayBase<FFGALNAPKCD> array = FFGALNAPKCD.get_AllPlayerControls().ToArray();
      List<FFGALNAPKCD> list = ((IEnumerable<FFGALNAPKCD>) array).Where<FFGALNAPKCD>((Func<FFGALNAPKCD, bool>) (x => !x.get_Data().get_DLPCKPBIJOE())).ToList<FFGALNAPKCD>();
      return ((IEnumerable<FFGALNAPKCD>) array).Where<FFGALNAPKCD>(new Func<FFGALNAPKCD, bool>(Utils.isLover)).Count<FFGALNAPKCD>() >= 2 && !Utils.Lover1.get_Data().get_DLPCKPBIJOE() && !Utils.Lover2.get_Data().get_DLPCKPBIJOE() && list.Count == 3 | list.Count == 2;
    }

    public static void LoversWin()
    {
      if (TownOfUs.JesterMod.EndCriteria.JesterVotedOut)
        return;
      List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator.MoveNext())
      {
        FFGALNAPKCD current = enumerator.get_Current();
        current.get_Data().set_DAPKNDBLKIA(current.isLover());
      }
      EndCriteria.LoveCoupleWins = true;
    }

    public static void NobodyWins()
    {
      if (TownOfUs.JesterMod.EndCriteria.JesterVotedOut)
        return;
      List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator.MoveNext())
        enumerator.get_Current().get_Data().set_DAPKNDBLKIA(false);
      EndCriteria.NobodyWins = true;
    }
  }
}
