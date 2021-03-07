// Decompiled with JetBrains decompiler
// Type: TownOfUs.SwapperMod.AddButton
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\Users\brayj\Downloads\TownOfUs-2020.12.9s (1).dll

using HarmonyLib;
using Reactor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TownOfUs.SwapperMod
{
  [HarmonyPatch(typeof (OOCJALPKPEP))]
  public class AddButton
  {
    public static readonly List<bool> ListOfActives = new List<bool>();
    public static readonly List<GameObject> Buttons = new List<GameObject>();
    private static int _mostRecentId;

    private static Sprite ActiveSprite => TownOfUs.TownOfUs.SwapperSwitch;

    public static Sprite DisabledSprite => TownOfUs.TownOfUs.SwapperSwitchDisabled;

    public static void GenButton(int index, bool isDead)
    {
      if (isDead)
      {
        AddButton.Buttons.Add((GameObject) null);
        AddButton.ListOfActives.Add(false);
      }
      else
      {
        GameObject gameObject1 = ((Component) ((Il2CppArrayBase<HDJGDMFCHDN>) OOCJALPKPEP.get_Instance().get_HBDFFAHBIGI()).get_Item(index).get_Buttons().get_transform().GetChild(0)).get_gameObject();
        GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, ((Component) ((Il2CppArrayBase<HDJGDMFCHDN>) OOCJALPKPEP.get_Instance().get_HBDFFAHBIGI()).get_Item(index)).get_transform());
        SpriteRenderer component1 = (SpriteRenderer) gameObject2.GetComponent<SpriteRenderer>();
        HHMBANDDIOA component2 = (HHMBANDDIOA) gameObject2.GetComponent<HHMBANDDIOA>();
        component1.set_sprite(AddButton.DisabledSprite);
        gameObject2.get_transform().set_position(Vector3.op_Subtraction(gameObject1.get_transform().get_position(), new Vector3(0.5f, 0.0f, 0.0f)));
        Transform transform = gameObject2.get_transform();
        transform.set_localScale(Vector3.op_Multiply(transform.get_localScale(), 0.8f));
        gameObject2.set_layer(5);
        gameObject2.get_transform().set_parent(gameObject1.get_transform().get_parent().get_parent());
        component2.set_OnClick(new Button.ButtonClickedEvent());
        ((UnityEvent) component2.get_OnClick()).AddListener(UnityAction.op_Implicit(AddButton.SetActive(index)));
        AddButton.Buttons.Add(gameObject2);
        AddButton.ListOfActives.Add(false);
      }
    }

    private static Action SetActive(int index)
    {
      return new Action(Listener);

      void Listener()
      {
        if (AddButton.ListOfActives.Count<bool>((Func<bool, bool>) (x => x)) == 2 && Object.op_Equality((Object) ((SpriteRenderer) AddButton.Buttons[index].GetComponent<SpriteRenderer>()).get_sprite(), (Object) AddButton.DisabledSprite))
          return;
        ((SpriteRenderer) AddButton.Buttons[index].GetComponent<SpriteRenderer>()).set_sprite(AddButton.ListOfActives[index] ? AddButton.DisabledSprite : AddButton.ActiveSprite);
        AddButton.ListOfActives[index] = !AddButton.ListOfActives[index];
        AddButton._mostRecentId = index;
        PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage((object) string.Join<bool>(" ", (IEnumerable<bool>) AddButton.ListOfActives));
      }
    }

    [HarmonyPatch("Start")]
    public static void Postfix(OOCJALPKPEP __instance)
    {
      AddButton.ListOfActives.Clear();
      AddButton.Buttons.Clear();
      if (!FFGALNAPKCD.get_LocalPlayer().isSwapper() || FFGALNAPKCD.get_LocalPlayer().get_Data().get_DLPCKPBIJOE())
        return;
      for (int index = 0; index < ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Length(); ++index)
        AddButton.GenButton(index, ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index).get_isDead());
    }
  }
}
