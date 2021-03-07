// Decompiled with JetBrains decompiler
// Type: TownOfUs.JesterMod.Outro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.JesterMod
{
  [HarmonyPatch(typeof (ABNGEPFHMHP), "Start")]
  public static class Outro
  {
    public static void Postfix(ABNGEPFHMHP __instance)
    {
      if (!EndCriteria.JesterVotedOut)
        return;
      FOONEKDGJBL[] foonekdgjblArray = Il2CppArrayBase<FOONEKDGJBL>.op_Implicit((Il2CppArrayBase<FOONEKDGJBL>) Object.FindObjectsOfType<FOONEKDGJBL>());
      foonekdgjblArray[0].get_NameText().set_Text("[FFBFCCFF]" + foonekdgjblArray[0].get_NameText().get_Text());
      ((Renderer) __instance.get_BackgroundBar()).get_material().set_color(new Color(1f, 0.75f, 0.8f, 1f));
      AELDHKGBIFD aeldhkgbifd = (AELDHKGBIFD) Object.Instantiate<AELDHKGBIFD>((M0) __instance.get_WinText());
      aeldhkgbifd.set_Text("Jester wins");
      aeldhkgbifd.set_Color(new Color(1f, 0.75f, 0.8f, 1f));
      Vector3 localPosition = ((Component) __instance.get_WinText()).get_transform().get_localPosition();
      localPosition.y = (__Null) 1.5;
      ((Component) aeldhkgbifd).get_transform().set_position(localPosition);
      aeldhkgbifd.set_scale(1f);
    }
  }
}
