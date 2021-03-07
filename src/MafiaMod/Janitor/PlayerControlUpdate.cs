// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Janitor.PlayerControlUpdate
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;

namespace TownOfUs.MafiaMod.Janitor
{
  [HarmonyPatch(typeof (FFGALNAPKCD))]
  public class PlayerControlUpdate
  {
    private static DDPGLPLGFOI closestBody;
    private static float closestDistance;

    [HarmonyPatch("FixedUpdate")]
    public static void Postfix(FFGALNAPKCD __instance)
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
