// Decompiled with JetBrains decompiler
// Type: TownOfUs.ShifterMod.EndGame
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Hazel;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
  [HarmonyPatch(typeof (HLBNNHFCNAJ), "PLBGOMIEONF")]
  public class EndGame
  {
    public static bool Prefix(HLBNNHFCNAJ __instance, [HarmonyArgument(0)] AIMMJPEOPEC reason)
    {
      if (reason != null && reason != 1 || Object.op_Equality((Object) Utils.Shifter, (Object) null))
        return true;
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 58, (SendOption) 1, -1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      Utils.Shifter.get_Data().set_DAPKNDBLKIA(true);
      return true;
    }
  }
}
