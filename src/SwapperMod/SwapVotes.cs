// Decompiled with JetBrains decompiler
// Type: TownOfUs.SwapperMod.SwapVotes
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Reactor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownOfUs.SwapperMod
{
  [HarmonyPatch(typeof (OOCJALPKPEP))]
  public class SwapVotes
  {
    public static HDJGDMFCHDN Swap1;
    public static HDJGDMFCHDN Swap2;

    [HarmonyPatch("KGIPOIOFBJH")]
    public static void Postfix(OOCJALPKPEP __instance)
    {
      PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage(Object.op_Equality((Object) SwapVotes.Swap1, (Object) null) ? (object) "null" : (object) ((Object) SwapVotes.Swap1).ToString());
      PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage(Object.op_Equality((Object) SwapVotes.Swap2, (Object) null) ? (object) "null" : (object) ((Object) SwapVotes.Swap2).ToString());
      if (!(Object.op_Inequality((Object) SwapVotes.Swap1, (Object) null) & Object.op_Inequality((Object) SwapVotes.Swap2, (Object) null)))
        return;
      if (FFGALNAPKCD.get_LocalPlayer().isSwapper())
      {
        using (IEnumerator<GameObject> enumerator = ((IEnumerable<GameObject>) AddButton.Buttons).Where<GameObject>((Func<GameObject, bool>) (button => Object.op_Inequality((Object) button, (Object) null))).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            enumerator.Current.SetActive(false);
        }
      }
      Transform transform1 = ((Component) SwapVotes.Swap1.get_PlayerIcon()).get_transform();
      Transform transform2 = ((Component) SwapVotes.Swap1.get_NameText()).get_transform();
      Transform child1 = ((Component) SwapVotes.Swap1).get_transform().GetChild(4).GetChild(0);
      Vector2 vector2_1 = Vector2.op_Implicit(transform1.get_position());
      Vector2 vector2_2 = Vector2.op_Implicit(transform2.get_position());
      Vector2 vector2_3 = Vector2.op_Implicit(child1.get_position());
      ((SpriteRenderer) ((Component) child1).get_gameObject().GetComponent<SpriteRenderer>()).set_color(new Color(1f, 1f, 1f, 0.0f));
      Transform transform3 = ((Component) SwapVotes.Swap2.get_PlayerIcon()).get_transform();
      Transform transform4 = ((Component) SwapVotes.Swap2.get_NameText()).get_transform();
      Transform child2 = ((Component) SwapVotes.Swap2).get_transform().GetChild(4).GetChild(0);
      Vector2 vector2_4 = Vector2.op_Implicit(transform3.get_position());
      Vector2 vector2_5 = Vector2.op_Implicit(transform4.get_position());
      Vector2 vector2_6 = Vector2.op_Implicit(child2.get_position());
      ((SpriteRenderer) ((Component) child2).get_gameObject().GetComponent<SpriteRenderer>()).set_color(new Color(1f, 1f, 1f, 0.0f));
      Coroutines.Start(SwapVotes.Slide2D(transform1, vector2_1, vector2_4, 2f));
      Coroutines.Start(SwapVotes.Slide2D(transform3, vector2_4, vector2_1, 2f));
      Coroutines.Start(SwapVotes.Slide2D(transform2, vector2_2, vector2_5, 2f));
      Coroutines.Start(SwapVotes.Slide2D(transform4, vector2_5, vector2_2, 2f));
      Coroutines.Start(SwapVotes.Slide2D(child1, vector2_3, vector2_6, 2f));
      Coroutines.Start(SwapVotes.Slide2D(child2, vector2_6, vector2_3, 2f));
    }

    private static IEnumerator Slide2D(
      Transform target,
      Vector2 source,
      Vector2 dest,
      float duration = 0.75f)
    {
      Vector3 temp = (Vector3) null;
      temp.z = target.get_position().z;
      for (float time = 0.0f; (double) time < (double) duration; time += Time.get_deltaTime())
      {
        float t = time / duration;
        temp.x = (__Null) (double) Mathf.SmoothStep((float) source.x, (float) dest.x, t);
        temp.y = (__Null) (double) Mathf.SmoothStep((float) source.y, (float) dest.y, t);
        target.set_position(temp);
        yield return (object) null;
      }
      temp.x = dest.x;
      temp.y = dest.y;
      target.set_position(temp);
    }

    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void Postfix1(OOCJALPKPEP __instance)
    {
      SwapVotes.Swap1 = (HDJGDMFCHDN) null;
      SwapVotes.Swap2 = (HDJGDMFCHDN) null;
    }
  }
}
