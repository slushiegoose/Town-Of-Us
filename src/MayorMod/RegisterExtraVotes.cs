// Decompiled with JetBrains decompiler
// Type: TownOfUs.MayorMod.RegisterExtraVotes
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Hazel;
using Il2CppSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.MayorMod
{
  [HarmonyPatch(typeof (OOCJALPKPEP))]
  public class RegisterExtraVotes
  {
    [HarmonyPatch("Confirm")]
    public static bool Prefix(OOCJALPKPEP __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isMayor() || __instance.get_DCCFKHIPIOF() != 2)
        return true;
      __instance.set_DCCFKHIPIOF((OOCJALPKPEP.BAMDJGFKOFP) 1);
      return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch("Confirm")]
    public static void Postfix1(OOCJALPKPEP __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isMayor() || AllowExtraVotes.VoteBank <= 0)
        return;
      ((Component) __instance.get_SkipVoteButton()).get_gameObject().SetActive(true);
    }

    private static int AreaIndexOf(OOCJALPKPEP __instance, sbyte srcPlayerId)
    {
      for (int index = 0; index < ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Length(); ++index)
      {
        if ((int) ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index).get_TargetPlayerId() == (int) srcPlayerId)
          return index;
      }
      return -1;
    }

    private static bool SetVote(HDJGDMFCHDN area, sbyte suspectPlayerId)
    {
      if (area.get_didVote())
      {
        AllowExtraVotes.ExtraVotes.Add((byte) ((uint) suspectPlayerId + 1U));
        if (!FFGALNAPKCD.get_LocalPlayer().isMayor())
          --AllowExtraVotes.VoteBank;
        return false;
      }
      area.set_didVote(true);
      area.set_votedFor(suspectPlayerId);
      ((Renderer) area.get_Flag()).set_enabled(true);
      return true;
    }

    [HarmonyPatch("Update")]
    public static void Postfix(OOCJALPKPEP __instance)
    {
      if (!FFGALNAPKCD.get_LocalPlayer().isMayor())
        return;
      __instance.get_TimerText().set_Text("Can Vote: " + AllowExtraVotes.VoteBank.ToString() + " time(s) | " + __instance.get_TimerText().get_Text());
    }

    [HarmonyPrefix]
    [HarmonyPatch("CCEPEINGBCN")]
    public static bool Prefix2(OOCJALPKPEP __instance, ref Il2CppStructArray<byte> __result)
    {
      if (Object.op_Equality((Object) Utils.Mayor, (Object) null))
        return true;
      byte[] numArray = new byte[FFGALNAPKCD.get_AllPlayerControls().get_Count() + 1];
      using (IEnumerator<HDJGDMFCHDN> enumerator = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          HDJGDMFCHDN current = enumerator.Current;
          if (current.get_didVote())
          {
            int index = (int) current.get_votedFor() + 1;
            if (index >= 0 && index < numArray.Length)
              ++numArray[index];
          }
        }
      }
      foreach (byte extraVote in AllowExtraVotes.ExtraVotes)
        ++numArray[(int) extraVote];
      __result = Il2CppStructArray<byte>.op_Implicit(numArray);
      return false;
    }

    [HarmonyPatch("CastVote")]
    public static bool Prefix(OOCJALPKPEP __instance, [HarmonyArgument(0)] byte srcPlayerId, [HarmonyArgument(1)] sbyte suspectPlayerId)
    {
      if (Object.op_Equality((Object) Utils.Mayor, (Object) null) || (int) srcPlayerId != (int) Utils.Mayor.get_PlayerId())
        return true;
      int num = RegisterExtraVotes.AreaIndexOf(__instance, (sbyte) srcPlayerId);
      HDJGDMFCHDN area = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(num);
      if (area.get_isDead())
        return false;
      if ((int) FFGALNAPKCD.get_LocalPlayer().get_PlayerId() == (int) srcPlayerId || FMLLKEACGIO.get_Instance().get_GameMode() > 0)
        POIDGKLMIMP.get_Instance().PlaySound(__instance.get_VoteLockinSound(), false, 1f);
      bool flag = RegisterExtraVotes.SetVote(area, suspectPlayerId);
      ((NJAHILONGKN) ((Il2CppObjectBase) __instance).Cast<NJAHILONGKN>()).LEEGFDDPHJB((uint) (1 << num));
      __instance.EIGGEKBEOEA();
      if (flag)
        FFGALNAPKCD.get_LocalPlayer().RpcSendChatNote(srcPlayerId, (AHIDPDEBPDC) 0);
      return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("KGIPOIOFBJH")]
    public static bool Prefix3(OOCJALPKPEP __instance)
    {
      if (!((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).get_AmHost() || Object.op_Equality((Object) Utils.Mayor, (Object) null))
        return true;
      MessageWriter messageWriter = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 66, (SendOption) 1, -1);
      messageWriter.WriteBytesAndSize(Il2CppStructArray<byte>.op_Implicit(AllowExtraVotes.ExtraVotes.ToArray()));
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter);
      return true;
    }

    [HarmonyPatch("Start")]
    public static void Prefix()
    {
      AllowExtraVotes.ExtraVotes.Clear();
      ++AllowExtraVotes.VoteBank;
    }

    private static void Vote(OOCJALPKPEP __instance, HDJGDMFCHDN area2, int num, Component origin)
    {
      EGLJNOMOGNP.DCJMABDDJCF playerById = EGLJNOMOGNP.get_Instance().GetPlayerById((byte) area2.get_TargetPlayerId());
      SpriteRenderer spriteRenderer = (SpriteRenderer) Object.Instantiate<SpriteRenderer>((M0) __instance.get_PlayerVotePrefab());
      if (FFGALNAPKCD.get_GameOptions().get_AGGKAFILPGD())
      {
        Console.WriteLine("ANONS");
        FFGALNAPKCD.SetPlayerMaterialColors(LOCPGOACAJF.get_ICNEJBPIBDB(), (Renderer) spriteRenderer);
      }
      else
      {
        Console.WriteLine("NONANONS");
        FFGALNAPKCD.SetPlayerMaterialColors((int) playerById.get_EHAHBDFODKC(), (Renderer) spriteRenderer);
      }
      Transform transform = ((Component) spriteRenderer).get_transform();
      transform.SetParent(origin.get_transform());
      transform.set_localPosition(Vector3.op_Addition(__instance.get_FGJMDFDIKEK(), new Vector3((float) __instance.get_IOHLPLMJHIB().x * (float) num, 0.0f, 0.0f)));
      transform.set_localScale(Vector3.get_zero());
      ((MonoBehaviour) __instance).StartCoroutine(MFGGDFBIKLF.NJOHOOJGMBC((float) num * 0.5f, transform, 1f, 0.5f));
    }

    [HarmonyPatch("FBNKEAEKKJE")]
    public static bool Prefix(OOCJALPKPEP __instance, [HarmonyArgument(0)] byte[] statess)
    {
      byte[] array = ((IEnumerable<string>) string.Join<byte>(",", (IEnumerable<byte>) statess).Split(',', StringSplitOptions.None)).Select<string, byte>(new Func<string, byte>(byte.Parse)).ToArray<byte>();
      int[] numArray = new int[((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Length()];
      if (Object.op_Equality((Object) Utils.Mayor, (Object) null))
        return true;
      __instance.get_TitleText().set_Text(((GIGNEFLFPDE) Object.FindObjectOfType<GIGNEFLFPDE>()).GetString((StringNames) 190, Il2CppReferenceArray<Object>.op_Implicit(Array.Empty<Object>())));
      int num1 = 0;
      for (int index1 = 0; index1 < ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Length(); ++index1)
      {
        HDJGDMFCHDN hdjgdmfchdn = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index1);
        hdjgdmfchdn.ClearForResults();
        int num2 = 0;
        for (int index2 = 0; index2 < ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Length(); ++index2)
        {
          HDJGDMFCHDN area2 = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index2);
          byte num3 = array[(int) area2.get_TargetPlayerId()];
          if (((int) num3 & 128) <= 0)
          {
            int votedFor = (int) HDJGDMFCHDN.GetVotedFor(num3);
            if (votedFor == (int) hdjgdmfchdn.get_TargetPlayerId())
            {
              RegisterExtraVotes.Vote(__instance, area2, num2, (Component) hdjgdmfchdn);
              ++num2;
            }
            else if (index1 == 0 && votedFor == -1)
            {
              RegisterExtraVotes.Vote(__instance, area2, num1, (Component) __instance.get_SkippedVoting());
              ++num1;
            }
          }
        }
        numArray[index1] = num2;
      }
      foreach (byte extraVote in AllowExtraVotes.ExtraVotes)
      {
        HDJGDMFCHDN area2 = ((IEnumerable<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).First<HDJGDMFCHDN>((Func<HDJGDMFCHDN, bool>) (pv => (int) pv.get_TargetPlayerId() == (int) Utils.Mayor.get_PlayerId()));
        int num2 = (int) extraVote - 1;
        if (num2 == -1)
        {
          RegisterExtraVotes.Vote(__instance, area2, num1, (Component) __instance.get_SkippedVoting());
          ++num1;
        }
        else
        {
          for (int index = 0; index < ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Length(); ++index)
          {
            HDJGDMFCHDN hdjgdmfchdn = ((Il2CppArrayBase<HDJGDMFCHDN>) __instance.get_HBDFFAHBIGI()).get_Item(index);
            if (num2 == (int) hdjgdmfchdn.get_TargetPlayerId())
            {
              RegisterExtraVotes.Vote(__instance, area2, numArray[index], (Component) hdjgdmfchdn);
              ++numArray[index];
            }
          }
        }
      }
      return false;
    }
  }
}
