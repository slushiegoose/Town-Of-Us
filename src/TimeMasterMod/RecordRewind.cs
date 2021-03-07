// Decompiled with JetBrains decompiler
// Type: TownOfUs.TimeMasterMod.RecordRewind
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownOfUs.TimeMasterMod
{
  [HarmonyPatch(typeof (PIEFJFEOGOL), "Update")]
  public class RecordRewind
  {
    public static bool rewinding = false;
    public static List<PointInTime> points = new List<PointInTime>();
    private static float deadTime;
    private static bool isDead = false;

    private static float recordTime => CustomGameOptions.RewindDuration;

    public static void Record()
    {
      if ((double) RecordRewind.points.Count > (double) Mathf.Round(RecordRewind.recordTime / Time.get_deltaTime()))
        RecordRewind.points.RemoveAt(RecordRewind.points.Count - 1);
      if (Object.op_Equality((Object) FFGALNAPKCD.get_LocalPlayer(), (Object) null))
        return;
      RecordRewind.points.Insert(0, new PointInTime(((Component) FFGALNAPKCD.get_LocalPlayer()).get_transform().get_position(), ((Rigidbody2D) ((Component) FFGALNAPKCD.get_LocalPlayer()).get_gameObject().GetComponent<Rigidbody2D>()).get_velocity(), Time.get_time()));
      if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE() && !RecordRewind.isDead)
      {
        RecordRewind.isDead = true;
        RecordRewind.deadTime = GKBKEMKIPIE.get_DAEIHIGEAIC() != null || CustomGameOptions.ReviveExiled ? Time.get_time() : 0.0f;
      }
      else
      {
        if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE() || !RecordRewind.isDead)
          return;
        RecordRewind.isDead = false;
        RecordRewind.deadTime = 0.0f;
      }
    }

    public static void Rewind()
    {
      Console.WriteLine("Rewinding...");
      Console.Write(RecordRewind.points.Count);
      if (RecordRewind.points.Count > 2)
      {
        if (!FFGALNAPKCD.get_LocalPlayer().get_inVent())
        {
          RecordRewind.points.RemoveAt(0);
          RecordRewind.points.RemoveAt(0);
          PointInTime point = RecordRewind.points[0];
          ((Component) FFGALNAPKCD.get_LocalPlayer()).get_transform().set_position(point.position);
          ((Rigidbody2D) ((Component) FFGALNAPKCD.get_LocalPlayer()).get_gameObject().GetComponent<Rigidbody2D>()).set_velocity(point.velocity);
          if (RecordRewind.isDead && (double) point.unix < (double) RecordRewind.deadTime && FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE() && CustomGameOptions.RewindRevive)
          {
            FFGALNAPKCD localPlayer = FFGALNAPKCD.get_LocalPlayer();
            RecordRewind.ReviveBody(localPlayer);
            localPlayer.get_myTasks().RemoveAt(0);
            RecordRewind.deadTime = 0.0f;
            RecordRewind.isDead = false;
            MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 70, (SendOption) 1, -1);
            messageWriter.Write(FFGALNAPKCD.get_LocalPlayer().get_PlayerId());
            ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
          }
        }
        RecordRewind.points.RemoveAt(0);
      }
      else
        StartStop.StopRewind();
    }

    public static void ReviveBody(FFGALNAPKCD player)
    {
      player.Revive();
      DDPGLPLGFOI ddpglplgfoi = ((IEnumerable<DDPGLPLGFOI>) Object.FindObjectsOfType<DDPGLPLGFOI>()).FirstOrDefault<DDPGLPLGFOI>((Func<DDPGLPLGFOI, bool>) (b => (int) b.get_ParentId() == (int) player.get_PlayerId()));
      if (!Object.op_Inequality((Object) ddpglplgfoi, (Object) null))
        return;
      Object.Destroy((Object) ((Component) ddpglplgfoi).get_gameObject());
    }

    public static void Postfix()
    {
      if (Object.op_Equality((Object) Utils.TimeMaster, (Object) null))
        return;
      if (RecordRewind.rewinding)
        RecordRewind.Rewind();
      else
        RecordRewind.Record();
      if ((DateTime.UtcNow - Methods.StartRewind).TotalMilliseconds <= (double) CustomGameOptions.RewindDuration * 1000.0 || !(Methods.FinishRewind < Methods.StartRewind))
        return;
      StartStop.StopRewind();
    }
  }
}
