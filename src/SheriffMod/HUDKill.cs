// Decompiled with JetBrains decompiler
// Type: TownOfUs.SheriffMod.HUDKill
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.SheriffMod
{
  [HarmonyPatch(typeof (PIEFJFEOGOL))]
  public class HUDKill
  {
    private static MLPJGKEACMM KillButton;

    [HarmonyPatch("Update")]
    public static void Postfix(PIEFJFEOGOL __instance) => HUDKill.UpdateKillButton(__instance);

    private static void UpdateKillButton(PIEFJFEOGOL __instance)
    {
      HUDKill.KillButton = __instance.get_KillButton();
      if (!(FFGALNAPKCD.get_AllPlayerControls().get_Count() > 1 & Object.op_Inequality((Object) Utils.Sheriff, (Object) null)))
        return;
      if (FFGALNAPKCD.get_LocalPlayer().isSheriff())
      {
        if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE())
        {
          ((Component) HUDKill.KillButton).get_gameObject().SetActive(false);
          HUDKill.KillButton.set_isActive(false);
        }
        else
        {
          ((Component) HUDKill.KillButton).get_gameObject().SetActive(true);
          HUDKill.KillButton.set_isActive(true);
          HUDKill.KillButton.SetCoolDown(Methods.SheriffKillTimer(), FFGALNAPKCD.get_GameOptions().get_IGHCIKIDAMO() + 15f);
          Methods.ClosestPlayer = Methods.getClosestPlayer(FFGALNAPKCD.get_LocalPlayer());
          if (Methods.getDistBetweenPlayers(FFGALNAPKCD.get_LocalPlayer(), Methods.ClosestPlayer) < (double) ((Il2CppArrayBase<float>) KMOGFLPJLLK.get_JMLGACIOLIK()).get_Item(FFGALNAPKCD.get_GameOptions().get_DLIBONBKPKL()))
            HUDKill.KillButton.SetTarget(Methods.ClosestPlayer);
        }
      }
      else
      {
        if (!FFGALNAPKCD.get_LocalPlayer().get_Data().get_DAPKNDBLKIA())
          return;
        if (FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE())
        {
          ((Component) HUDKill.KillButton).get_gameObject().SetActive(false);
          HUDKill.KillButton.set_isActive(false);
        }
        else
        {
          ((Component) __instance.get_KillButton()).get_gameObject().SetActive(true);
          __instance.get_KillButton().set_isActive(true);
        }
      }
    }
  }
}
