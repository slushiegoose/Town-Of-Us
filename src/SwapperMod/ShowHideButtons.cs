// Decompiled with JetBrains decompiler
// Type: TownOfUs.SwapperMod.ShowHideButtons
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Hazel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace TownOfUs.SwapperMod
{
  [HarmonyPatch(typeof (OOCJALPKPEP))]
  public class ShowHideButtons
  {
    [HarmonyPatch("Confirm")]
    public static bool Prefix(OOCJALPKPEP __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isSwapper())
        return true;
      using (IEnumerator<GameObject> enumerator = ((IEnumerable<GameObject>) AddButton.Buttons).Where<GameObject>((Func<GameObject, bool>) (button => Object.op_Inequality((Object) button, (Object) null))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Equality((Object) ((SpriteRenderer) current.GetComponent<SpriteRenderer>()).get_sprite(), (Object) AddButton.DisabledSprite))
            current.SetActive(false);
          ((HHMBANDDIOA) current.GetComponent<HHMBANDDIOA>()).set_OnClick(new Button.ButtonClickedEvent());
        }
      }
      if (AddButton.ListOfActives.Count<bool>((Func<bool, bool>) (x => x)) == 2)
      {
        bool flag = true;
        for (int index = 0; index < AddButton.ListOfActives.Count; ++index)
        {
          if (AddButton.ListOfActives[index])
          {
            if (flag)
            {
              SwapVotes.Swap1 = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index);
              flag = false;
            }
            else
              SwapVotes.Swap2 = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index);
          }
        }
      }
      if (Object.op_Equality((Object) SwapVotes.Swap1, (Object) null) || Object.op_Equality((Object) SwapVotes.Swap2, (Object) null))
        return true;
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 67, (SendOption) 1, -1);
      messageWriter.Write(SwapVotes.Swap1.get_TargetPlayerId());
      messageWriter.Write(SwapVotes.Swap2.get_TargetPlayerId());
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch("CCEPEINGBCN")]
    public static void Postfix2(OOCJALPKPEP __instance, ref Il2CppStructArray<byte> __result)
    {
      if (Object.op_Equality((Object) Utils.Swapper, (Object) null) || (Object.op_Equality((Object) SwapVotes.Swap1, (Object) null) || Object.op_Equality((Object) SwapVotes.Swap2, (Object) null)))
        return;
      byte[] numArray = new byte[FFGALNAPKCD.get_AllPlayerControls().get_Count() + 1];
      for (int index = 0; index < numArray.Length; ++index)
      {
        if (index == (int) SwapVotes.Swap1.get_TargetPlayerId() + 1)
          numArray[(int) SwapVotes.Swap2.get_TargetPlayerId() + 1] = ((Il2CppArrayBase<byte>) __result).get_Item(index);
        else if (index == (int) SwapVotes.Swap2.get_TargetPlayerId() + 1)
          numArray[(int) SwapVotes.Swap1.get_TargetPlayerId() + 1] = ((Il2CppArrayBase<byte>) __result).get_Item(index);
        else
          numArray[index] = ((Il2CppArrayBase<byte>) __result).get_Item(index);
      }
      __result = Il2CppStructArray<byte>.op_Implicit(numArray);
    }
  }
}
