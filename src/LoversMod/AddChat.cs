// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.AddChat
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (GEHKHGLKFHE))]
  public static class AddChat
  {
    [HarmonyPatch("AddChat")]
    public static bool Prefix()
    {
      FFGALNAPKCD localPlayer = FFGALNAPKCD.get_LocalPlayer();
      return Object.op_Equality((Object) localPlayer, (Object) null) || (Object.op_Inequality((Object) OOCJALPKPEP.get_Instance(), (Object) null) || Object.op_Inequality((Object) PFLIBLFPGGB.get_Instance(), (Object) null) || localPlayer.get_Data().get_DLPCKPBIJOE() || localPlayer.isLover());
    }
  }
}
