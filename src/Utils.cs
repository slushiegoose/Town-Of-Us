// Decompiled with JetBrains decompiler
// Type: TownOfUs.Utils
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs
{
  [HarmonyPatch]
  public static class Utils
  {
    public static FFGALNAPKCD Jester;
    public static FFGALNAPKCD Mayor;
    public static FFGALNAPKCD Sheriff;
    public static FFGALNAPKCD Engineer;
    public static FFGALNAPKCD Swapper;
    public static FFGALNAPKCD Shifter;
    public static FFGALNAPKCD Investigator;
    public static FFGALNAPKCD TimeMaster;
    public static FFGALNAPKCD Godfather;
    public static FFGALNAPKCD Mafioso;
    public static FFGALNAPKCD Janitor;
    public static FFGALNAPKCD Lover1;
    public static FFGALNAPKCD Lover2;
    public static bool LoverImpostor;

    public static void Null()
    {
      Utils.Mayor = (FFGALNAPKCD) null;
      Utils.Jester = (FFGALNAPKCD) null;
      Utils.Sheriff = (FFGALNAPKCD) null;
      Utils.Lover1 = (FFGALNAPKCD) null;
      Utils.Lover2 = (FFGALNAPKCD) null;
      Utils.Janitor = (FFGALNAPKCD) null;
      Utils.Mafioso = (FFGALNAPKCD) null;
      Utils.Godfather = (FFGALNAPKCD) null;
      Utils.Engineer = (FFGALNAPKCD) null;
      Utils.Swapper = (FFGALNAPKCD) null;
      Utils.Shifter = (FFGALNAPKCD) null;
      Utils.Investigator = (FFGALNAPKCD) null;
      Utils.TimeMaster = (FFGALNAPKCD) null;
    }

    public static bool isJester(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Jester, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Jester.get_PlayerId();

    public static bool isMayor(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Mayor, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Mayor.get_PlayerId();

    public static bool isSheriff(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Sheriff, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Sheriff.get_PlayerId();

    public static bool isEngineer(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Engineer, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Engineer.get_PlayerId();

    public static bool isLover1(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Lover1, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Lover1.get_PlayerId();

    public static bool isLover2(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Lover2, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Lover2.get_PlayerId();

    public static bool isLover(this FFGALNAPKCD player) => player.isLover1() | player.isLover2();

    public static FFGALNAPKCD OtherLover(this FFGALNAPKCD player) => player.isLover1() ? Utils.Lover2 : (player.isLover2() ? Utils.Lover1 : (FFGALNAPKCD) null);

    public static bool isGodfather(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Godfather, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Godfather.get_PlayerId();

    public static bool isMafioso(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Mafioso, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Mafioso.get_PlayerId();

    public static bool isJanitor(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Janitor, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Janitor.get_PlayerId();

    public static bool isSwapper(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Swapper, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Swapper.get_PlayerId();

    public static bool isShifter(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Shifter, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Shifter.get_PlayerId();

    public static bool isInvestigator(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.Investigator, (Object) null) && (int) player.get_PlayerId() == (int) Utils.Investigator.get_PlayerId();

    public static bool isTimeMaster(this FFGALNAPKCD player) => !Object.op_Equality((Object) Utils.TimeMaster, (Object) null) && (int) player.get_PlayerId() == (int) Utils.TimeMaster.get_PlayerId();

    public static bool IsCrewmate(this FFGALNAPKCD player) => Utils.GetRole(player) == Roles.Crewmate;

    public static Roles GetRole(FFGALNAPKCD player)
    {
      if (Object.op_Equality((Object) player, (Object) null) || player.get_Data() == null)
        return Roles.None;
      if (player.isSheriff())
        return Roles.Sheriff;
      if (player.isJester())
        return Roles.Jester;
      if (player.isEngineer())
        return Roles.Engineer;
      if (player.isMayor())
        return Roles.Mayor;
      if (player.isLover1())
        return Roles.Lover1;
      if (player.isLover2() & !Utils.LoverImpostor)
        return Roles.Lover2;
      if (player.isSwapper())
        return Roles.Swapper;
      if (player.isInvestigator())
        return Roles.Investigator;
      return player.isTimeMaster() ? Roles.TimeMaster : (player.get_Data().get_DAPKNDBLKIA() ? Roles.Impostor : Roles.Crewmate);
    }

    public static FFGALNAPKCD PlayerById(byte id)
    {
      List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator.MoveNext())
      {
        FFGALNAPKCD current = enumerator.get_Current();
        if ((int) current.get_PlayerId() == (int) id)
          return current;
      }
      return (FFGALNAPKCD) null;
    }

    public static List<FFGALNAPKCD> getCrewmates(
      IEnumerable<EGLJNOMOGNP.DCJMABDDJCF> infection)
    {
      List<FFGALNAPKCD> ffgalnapkcdList = new List<FFGALNAPKCD>();
      List<FFGALNAPKCD>.Enumerator enumerator1 = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator1.MoveNext())
      {
        FFGALNAPKCD current1 = enumerator1.get_Current();
        bool flag = false;
        using (IEnumerator<EGLJNOMOGNP.DCJMABDDJCF> enumerator2 = infection.GetEnumerator())
        {
          while (((IEnumerator) enumerator2).MoveNext())
          {
            EGLJNOMOGNP.DCJMABDDJCF current2 = enumerator2.Current;
            if ((int) current1.get_PlayerId() == (int) current2.BIIDBKACNJB().get_PlayerId())
              flag = true;
          }
        }
        if (!flag)
          ffgalnapkcdList.Add(current1);
      }
      return ffgalnapkcdList;
    }

    public static List<FFGALNAPKCD> getImpostors(
      IEnumerable<EGLJNOMOGNP.DCJMABDDJCF> infection)
    {
      List<FFGALNAPKCD> ffgalnapkcdList = new List<FFGALNAPKCD>();
      List<FFGALNAPKCD>.Enumerator enumerator1 = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
      while (enumerator1.MoveNext())
      {
        FFGALNAPKCD current1 = enumerator1.get_Current();
        bool flag = false;
        using (IEnumerator<EGLJNOMOGNP.DCJMABDDJCF> enumerator2 = infection.GetEnumerator())
        {
          while (((IEnumerator) enumerator2).MoveNext())
          {
            EGLJNOMOGNP.DCJMABDDJCF current2 = enumerator2.Current;
            if ((int) current1.get_PlayerId() == (int) current2.BIIDBKACNJB().get_PlayerId())
              flag = true;
          }
        }
        if (flag)
          ffgalnapkcdList.Add(current1);
      }
      return ffgalnapkcdList;
    }
  }
}
