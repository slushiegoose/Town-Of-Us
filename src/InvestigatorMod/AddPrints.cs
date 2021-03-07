// Decompiled with JetBrains decompiler
// Type: TownOfUs.InvestigatorMod.AddPrints
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownOfUs.InvestigatorMod
{
  [HarmonyPatch(typeof (FFGALNAPKCD), "FixedUpdate")]
  public static class AddPrints
  {
    private static float _time;
    private const float PeriodInterval = 0.25f;
    public static bool GameStarted;

    private static float Interval => CustomGameOptions.FootprintInterval;

    private static bool Vent => CustomGameOptions.VentFootprintVisible;

    public static Vector2 Position(FFGALNAPKCD player) => Vector2.op_Addition(player.GetTruePosition(), new Vector2(0.0f, 0.366667f));

    public static void Postfix(FFGALNAPKCD __instance)
    {
      if (!AddPrints.GameStarted || !FFGALNAPKCD.get_LocalPlayer().isInvestigator())
        return;
      AddPrints._time += Time.get_deltaTime();
      if ((double) AddPrints._time >= (double) AddPrints.Interval)
      {
        AddPrints._time -= AddPrints.Interval;
        if (FFGALNAPKCD.get_LocalPlayer().isInvestigator())
        {
          List<FFGALNAPKCD>.Enumerator enumerator = FFGALNAPKCD.get_AllPlayerControls().GetEnumerator();
          while (enumerator.MoveNext())
          {
            FFGALNAPKCD player = enumerator.get_Current();
            if (!Object.op_Equality((Object) player, (Object) null) && !player.get_Data().get_DLPCKPBIJOE() && (int) player.get_PlayerId() != (int) FFGALNAPKCD.get_LocalPlayer().get_PlayerId())
            {
              bool flag = !Footprint.AllPrints.Any<Footprint>((Func<Footprint, bool>) (print => (double) Vector3.Distance(print.Position, Vector2.op_Implicit(AddPrints.Position(player))) < 0.5 && (double) print.Color.a > 0.5 && (int) print.Player.get_PlayerId() == (int) player.get_PlayerId()));
              if (AddPrints.Vent && Object.op_Inequality((Object) HLBNNHFCNAJ.get_Instance(), (Object) null) && ((IEnumerable<OPPMFCFACJB>) HLBNNHFCNAJ.get_Instance().get_AllVents()).Any<OPPMFCFACJB>((Func<OPPMFCFACJB, bool>) (vent => (double) Vector2.Distance(Vector2.op_Implicit(((Component) vent).get_gameObject().get_transform().get_position()), AddPrints.Position(player)) < 1.0)))
                flag = false;
              if (flag)
              {
                Footprint footprint = new Footprint(player);
              }
            }
          }
        }
      }
      for (int index = 0; index < Footprint.AllPrints.Count; ++index)
      {
        if (Footprint.AllPrints[index].Update())
          --index;
      }
    }
  }
}
