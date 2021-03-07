// Decompiled with JetBrains decompiler
// Type: TownOfUs.MayorMod.AllowExtraVotes
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using System.Collections.Generic;

namespace TownOfUs.MayorMod
{
  [HarmonyPatch(typeof (HDJGDMFCHDN))]
  public class AllowExtraVotes
  {
    public static List<byte> ExtraVotes = new List<byte>();
    public static int VoteBank;

    [HarmonyPatch("Select")]
    public static bool Prefix(HDJGDMFCHDN __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isMayor())
        return true;
      if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE() || __instance.get_isDead() || (AllowExtraVotes.VoteBank <= 0 || !__instance.get_Parent().Select((int) __instance.get_TargetPlayerId())))
        return false;
      __instance.get_Buttons().SetActive(true);
      return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("VoteForMe")]
    public static bool Prefix2(HDJGDMFCHDN __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isMayor())
        return true;
      if (__instance.get_Parent().get_DCCFKHIPIOF() == 4 || __instance.get_Parent().get_DCCFKHIPIOF() == 3 || AllowExtraVotes.VoteBank <= 0)
        return false;
      --AllowExtraVotes.VoteBank;
      __instance.get_Parent().Confirm(__instance.get_TargetPlayerId());
      return false;
    }
  }
}
