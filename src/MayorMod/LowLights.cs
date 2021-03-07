// Decompiled with JetBrains decompiler
// Type: TownOfUs.MayorMod.LowLights
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
  [HarmonyPatch(typeof (HLBNNHFCNAJ), "CalculateLightRadius")]
  public static class LowLights
  {
    public static bool Prefix(
      HLBNNHFCNAJ __instance,
      [HarmonyArgument(0)] EGLJNOMOGNP.DCJMABDDJCF player,
      ref float __result)
    {
      if (player == null || player.get_DLPCKPBIJOE())
      {
        __result = __instance.get_MaxLightRadius();
        return false;
      }
      ILEEIKKPGLG ileeikkpglg = (ILEEIKKPGLG) ((Il2CppObjectBase) __instance.get_Systems().get_Item((LJFDDJHBOGF) 7)).Cast<ILEEIKKPGLG>();
      if (player.get_DAPKNDBLKIA())
      {
        __result = __instance.get_MaxLightRadius() * FFGALNAPKCD.get_GameOptions().get_GFDCMFFAAGI();
        return false;
      }
      float num = (float) ileeikkpglg.get_ALFKAPAHDEL() / (float) byte.MaxValue;
      __result = Mathf.Lerp(__instance.get_MinLightRadius(), __instance.get_MaxLightRadius(), num) * FFGALNAPKCD.get_GameOptions().get_POMBOFGMLEN();
      if (player.BIIDBKACNJB().isMayor())
        __result = Mathf.Lerp(__instance.get_MinLightRadius(), __instance.get_MaxLightRadius(), num) * CustomGameOptions.MayorVision;
      if (player.BIIDBKACNJB().isEngineer())
        __result = Mathf.Lerp(__instance.get_MinLightRadius(), __instance.get_MaxLightRadius(), num) * CustomGameOptions.EngineerVision;
      return false;
    }
  }
}
