// Decompiled with JetBrains decompiler
// Type: TownOfUs.RainbowMod.PlayerTabPatch
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.RainbowMod
{
  [HarmonyPatch(typeof (MAOILGPNFND), "OnEnable")]
  public class PlayerTabPatch
  {
    public static void Postfix(MAOILGPNFND __instance)
    {
      List<HHCPGBKDLEE>.Enumerator enumerator = __instance.get_FDLJPFELGJG().GetEnumerator();
      while (enumerator.MoveNext())
      {
        Transform transform = ((Component) enumerator.get_Current()).get_transform();
        transform.set_localScale(Vector3.op_Multiply(transform.get_localScale(), 0.65f));
      }
    }
  }
}
