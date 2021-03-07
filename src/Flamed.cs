// Decompiled with JetBrains decompiler
// Type: TownOfUs.Flamed
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
  [HarmonyPatch(typeof (CFKDGHIEFFA.BDAOHIACLAN), "MoveNext")]
  public class Flamed
  {
    public static void Prefix(CFKDGHIEFFA.BDAOHIACLAN __instance)
    {
      OAKKJMGJBGG killAnimPrefab = __instance.get_killAnimPrefab();
      SpriteRenderer component = (SpriteRenderer) ((Component) __instance.get___this().get_flameParent().get_transform().GetChild(0)).get_gameObject().GetComponent<SpriteRenderer>();
      if (Object.op_Equality((Object) killAnimPrefab, (Object) __instance.get___this().get_ReportOverlay()) || Object.op_Equality((Object) killAnimPrefab, (Object) __instance.get___this().get_EmergencyOverlay()))
      {
        component.set_color(Color.get_white());
        component.set_sprite(TownOfUs.TownOfUs.NormalKill);
      }
      else
      {
        switch (Flamed.GetOption(__instance.get_killer(), __instance.get_victim()))
        {
          case MurderEnum.Normal:
            component.set_color(Color.get_white());
            component.set_sprite(TownOfUs.TownOfUs.NormalKill);
            break;
          case MurderEnum.Sheriff:
            component.set_color(Color.get_yellow());
            component.set_sprite(TownOfUs.TownOfUs.GreyscaleKill);
            break;
          case MurderEnum.Shifter:
            component.set_sprite(TownOfUs.TownOfUs.GreyscaleKill);
            component.set_color(Color.get_white());
            break;
          case MurderEnum.Lover:
            component.set_sprite(TownOfUs.TownOfUs.GreyscaleKill);
            component.set_color(new Color(1f, 0.4f, 0.8f));
            break;
        }
      }
    }

    private static MurderEnum GetOption(
      EGLJNOMOGNP.DCJMABDDJCF killer,
      EGLJNOMOGNP.DCJMABDDJCF victim)
    {
      if (killer.BIIDBKACNJB().isSheriff())
        return MurderEnum.Sheriff;
      if (victim.BIIDBKACNJB().isShifter() && (!killer.get_DAPKNDBLKIA() || killer.BIIDBKACNJB().isShifter()))
        return MurderEnum.Shifter;
      return killer.BIIDBKACNJB().isLover() && victim.BIIDBKACNJB().isLover() && (int) killer.get_JKOMCOJCAID() == (int) victim.get_JKOMCOJCAID() ? MurderEnum.Lover : MurderEnum.Normal;
    }
  }
}
