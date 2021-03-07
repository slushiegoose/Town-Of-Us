// Decompiled with JetBrains decompiler
// Type: TownOfUs.MafiaMod.Janitor.KillButtonTarget
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{
  [HarmonyPatch(typeof (MLPJGKEACMM))]
  public class KillButtonTarget
  {
    public static DDPGLPLGFOI CurrentTarget;

    [HarmonyPatch("SetTarget")]
    public static bool Prefix(MLPJGKEACMM __instance) => !FFGALNAPKCD.get_LocalPlayer().isJanitor();

    public static void SetTarget(MLPJGKEACMM __instance, DDPGLPLGFOI target)
    {
      if (Object.op_Implicit((Object) KillButtonTarget.CurrentTarget) && Object.op_Inequality((Object) KillButtonTarget.CurrentTarget, (Object) target))
        ((Renderer) ((Component) KillButtonTarget.CurrentTarget).GetComponent<SpriteRenderer>()).get_material().SetFloat("_Outline", 0.0f);
      KillButtonTarget.CurrentTarget = target;
      if (Object.op_Implicit((Object) KillButtonTarget.CurrentTarget))
      {
        SpriteRenderer component = (SpriteRenderer) ((Component) KillButtonTarget.CurrentTarget).GetComponent<SpriteRenderer>();
        ((Renderer) component).get_material().SetFloat("_Outline", 1f);
        ((Renderer) component).get_material().SetColor("_OutlineColor", Color.get_red());
        __instance.get_renderer().set_color(LOCPGOACAJF.get_AGOLMCOBHAF());
        ((Renderer) __instance.get_renderer()).get_material().SetFloat("_Desat", 0.0f);
      }
      else
      {
        __instance.get_renderer().set_color(LOCPGOACAJF.get_AEJDBEHFKGE());
        ((Renderer) __instance.get_renderer()).get_material().SetFloat("_Desat", 1f);
      }
    }
  }
}
