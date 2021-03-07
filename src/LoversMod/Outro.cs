// Decompiled with JetBrains decompiler
// Type: TownOfUs.LoversMod.Outro
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.LoversMod
{
  [HarmonyPatch(typeof (ABNGEPFHMHP))]
  internal class Outro
  {
    [HarmonyPatch("Start")]
    public static void Postfix(ABNGEPFHMHP __instance)
    {
      if (EndCriteria.NobodyWins)
      {
        AELDHKGBIFD aeldhkgbifd = (AELDHKGBIFD) Object.Instantiate<AELDHKGBIFD>((M0) __instance.get_WinText());
        Color color = __instance.get_WinText().get_Color();
        color.a = (__Null) 1.0;
        aeldhkgbifd.set_Color(color);
        aeldhkgbifd.set_Text("Only neutral roles were left");
        Vector3 localPosition = ((Component) __instance.get_WinText()).get_transform().get_localPosition();
        localPosition.y = (__Null) 1.5;
        ((Component) aeldhkgbifd).get_transform().set_position(localPosition);
        aeldhkgbifd.set_scale(0.5f);
      }
      else
      {
        if (!EndCriteria.LoveCoupleWins || TownOfUs.JesterMod.EndCriteria.JesterVotedOut)
          return;
        FOONEKDGJBL[] foonekdgjblArray = Il2CppArrayBase<FOONEKDGJBL>.op_Implicit((Il2CppArrayBase<FOONEKDGJBL>) Object.FindObjectsOfType<FOONEKDGJBL>());
        if (Object.op_Inequality((Object) foonekdgjblArray[0], (Object) null))
        {
          Transform transform = ((Component) foonekdgjblArray[0]).get_gameObject().get_transform();
          transform.set_position(Vector3.op_Subtraction(transform.get_position(), new Vector3(1.5f, 0.0f, 0.0f)));
          foonekdgjblArray[0].SetFlipX(true);
          foonekdgjblArray[0].get_NameText().set_Text("[FF80D5FF]" + foonekdgjblArray[0].get_NameText().get_Text());
        }
        if (Object.op_Inequality((Object) foonekdgjblArray[1], (Object) null))
        {
          foonekdgjblArray[1].SetFlipX(false);
          ((Component) foonekdgjblArray[1]).get_gameObject().get_transform().set_position(Vector3.op_Addition(((Component) foonekdgjblArray[0]).get_gameObject().get_transform().get_position(), new Vector3(1.2f, 0.0f, 0.0f)));
          Transform transform1 = ((Component) foonekdgjblArray[1]).get_gameObject().get_transform();
          transform1.set_localScale(Vector3.op_Multiply(transform1.get_localScale(), 0.92f));
          Transform transform2 = ((Component) foonekdgjblArray[1].get_HatSlot()).get_transform();
          transform2.set_position(Vector3.op_Addition(transform2.get_position(), new Vector3(0.1f, 0.0f, 0.0f)));
          foonekdgjblArray[1].get_NameText().set_Text("[FF80D5FF]" + foonekdgjblArray[1].get_NameText().get_Text());
        }
        ((Renderer) __instance.get_BackgroundBar()).get_material().set_color(new Color(1f, 0.4f, 0.8f, 1f));
        AELDHKGBIFD aeldhkgbifd = (AELDHKGBIFD) Object.Instantiate<AELDHKGBIFD>((M0) __instance.get_WinText());
        aeldhkgbifd.set_Text("Love couple wins");
        aeldhkgbifd.set_Color(new Color(1f, 0.4f, 0.8f, 1f));
        Vector3 localPosition = ((Component) __instance.get_WinText()).get_transform().get_localPosition();
        localPosition.y = (__Null) 1.5;
        ((Component) aeldhkgbifd).get_transform().set_position(localPosition);
        aeldhkgbifd.set_scale(1f);
      }
    }
  }
}
