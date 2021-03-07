// Decompiled with JetBrains decompiler
// Type: TownOfUs.RpcHandling
// Assembly: TownOfUs, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 167B09F5-14AA-4A43-BCE6-062AB9919D13
// Assembly location: C:\temp\TownOfUs-2020.12.9s.dll

using HarmonyLib;
using Hazel;
using Il2CppSystem;
using Reactor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TownOfUs.EngineerMod;
using TownOfUs.InvestigatorMod;
using TownOfUs.MafiaMod.Janitor;
using TownOfUs.MayorMod;
using TownOfUs.SwapperMod;
using TownOfUs.TimeMasterMod;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs
{
  [HarmonyPatch(typeof (FFGALNAPKCD))]
  public static class RpcHandling
  {
    public static readonly Random Rand = new Random();

    [HarmonyPatch("HandleRpc")]
    public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
    {
      switch (callId)
      {
        case 43:
          Utils.Jester = Utils.PlayerById(reader.ReadByte());
          break;
        case 44:
          byte id1 = reader.ReadByte();
          byte id2 = reader.ReadByte();
          byte num1 = reader.ReadByte();
          FFGALNAPKCD ffgalnapkcd1 = Utils.PlayerById(id1);
          FFGALNAPKCD ffgalnapkcd2 = Utils.PlayerById(id2);
          Utils.Lover1 = ffgalnapkcd1;
          Utils.Lover2 = ffgalnapkcd2;
          Utils.LoverImpostor = num1 == (byte) 0;
          break;
        case 45:
          Utils.Mayor = Utils.PlayerById(reader.ReadByte());
          break;
        case 46:
          Utils.Sheriff = Utils.PlayerById(reader.ReadByte());
          break;
        case 47:
          Utils.Engineer = Utils.PlayerById(reader.ReadByte());
          break;
        case 48:
          Utils.Swapper = Utils.PlayerById(reader.ReadByte());
          break;
        case 49:
          Utils.Shifter = Utils.PlayerById(reader.ReadByte());
          break;
        case 50:
          Utils.Investigator = Utils.PlayerById(reader.ReadByte());
          break;
        case 51:
          Utils.TimeMaster = Utils.PlayerById(reader.ReadByte());
          break;
        case 52:
          Utils.Godfather = Utils.PlayerById(reader.ReadByte());
          break;
        case 53:
          Utils.Janitor = Utils.PlayerById(reader.ReadByte());
          break;
        case 54:
          Utils.Mafioso = Utils.PlayerById(reader.ReadByte());
          break;
        case 55:
          Utils.Jester.get_Data().set_DLPCKPBIJOE(false);
          break;
        case 56:
          TownOfUs.LoversMod.Methods.LoversWin();
          break;
        case 57:
          Utils.Jester.get_Data().set_DAPKNDBLKIA(true);
          break;
        case 58:
          Utils.Shifter.get_Data().set_DAPKNDBLKIA(true);
          break;
        case 59:
          TownOfUs.LoversMod.Methods.NobodyWins();
          break;
        case 60:
          FFGALNAPKCD ffgalnapkcd3 = Utils.PlayerById(reader.ReadByte());
          ffgalnapkcd3.MurderPlayer(ffgalnapkcd3);
          break;
        case 61:
          FFGALNAPKCD player = Utils.PlayerById(reader.ReadByte());
          FFGALNAPKCD ffgalnapkcd4 = Utils.PlayerById(reader.ReadByte());
          if (player.isSheriff())
            player.MurderPlayer(ffgalnapkcd4);
          TownOfUs.SheriffMod.Methods.LastKilled = DateTime.UtcNow;
          break;
        case 62:
          byte num2 = reader.ReadByte();
          using (IEnumerator<DDPGLPLGFOI> enumerator = ((Il2CppArrayBase<DDPGLPLGFOI>) Object.FindObjectsOfType<DDPGLPLGFOI>()).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              DDPGLPLGFOI current = enumerator.Current;
              if ((int) current.get_ParentId() == (int) num2)
                Coroutines.Start(Coroutine.CleanCoroutine(current));
            }
            break;
          }
        case 63:
          ILEEIKKPGLG ileeikkpglg = (ILEEIKKPGLG) ((Il2CppObjectBase) HLBNNHFCNAJ.get_Instance().get_Systems().get_Item((LJFDDJHBOGF) 7)).Cast<ILEEIKKPGLG>();
          ileeikkpglg.set_LNOCHNPPIFJ(ileeikkpglg.get_HBLBMJBKOKA());
          break;
        case 64:
          Arrows.GenArrows();
          break;
        case 65:
          Arrows.RemoveArrows();
          break;
        case 66:
          AllowExtraVotes.ExtraVotes = ((IEnumerable<byte>) reader.ReadBytesAndSize()).ToList<byte>();
          if (FFGALNAPKCD.get_LocalPlayer().isMayor())
            break;
          AllowExtraVotes.VoteBank -= AllowExtraVotes.ExtraVotes.Count;
          break;
        case 67:
          sbyte readSByte = reader.ReadSByte();
          SwapVotes.Swap1 = ((IEnumerable<HDJGDMFCHDN>) OOCJALPKPEP.get_Instance().get_HBDFFAHBIGI()).First<HDJGDMFCHDN>((Func<HDJGDMFCHDN, bool>) (x => (int) x.get_TargetPlayerId() == (int) readSByte));
          sbyte readSByte2 = reader.ReadSByte();
          SwapVotes.Swap2 = ((IEnumerable<HDJGDMFCHDN>) OOCJALPKPEP.get_Instance().get_HBDFFAHBIGI()).First<HDJGDMFCHDN>((Func<HDJGDMFCHDN, bool>) (x => (int) x.get_TargetPlayerId() == (int) readSByte2));
          PluginSingleton<TownOfUs.TownOfUs>.get_Instance().get_Log().LogMessage((object) ("Bytes received - " + readSByte.ToString() + " - " + readSByte2.ToString()));
          break;
        case 68:
          FFGALNAPKCD other = Utils.PlayerById(reader.ReadByte());
          TownOfUs.ShifterMod.PerformKillButton.Shift(Utils.Shifter, other);
          break;
        case 69:
          StartStop.StartRewind();
          break;
        case 70:
          RecordRewind.ReviveBody(Utils.PlayerById(reader.ReadByte()));
          break;
        case 71:
          AllowExtraVotes.VoteBank = CustomGameOptions.MayorVoteBank;
          PerformKill.UsedThisRound = false;
          DateTime now = DateTime.get_Now();
          PerformKill.SabotageTime = ((DateTime) ref now).AddSeconds(-100.0);
          TownOfUs.LoversMod.EndCriteria.LoveCoupleWins = false;
          TownOfUs.LoversMod.EndCriteria.NobodyWins = false;
          TownOfUs.JesterMod.EndCriteria.JesterVotedOut = false;
          Utils.Null();
          Footprint.DestroyAll();
          RecordRewind.points.Clear();
          break;
      }
    }

    private static bool Check(int probability) => RpcHandling.Rand.Next(1, 101) <= probability;

    [HarmonyPatch("RpcSetInfected")]
    public static void Postfix(
      [HarmonyArgument(0)] Il2CppReferenceArray<EGLJNOMOGNP.DCJMABDDJCF> infected)
    {
      AllowExtraVotes.VoteBank = CustomGameOptions.MayorVoteBank;
      PerformKill.UsedThisRound = false;
      DateTime now = DateTime.get_Now();
      PerformKill.SabotageTime = ((DateTime) ref now).AddSeconds(-100.0);
      TownOfUs.LoversMod.EndCriteria.LoveCoupleWins = false;
      TownOfUs.LoversMod.EndCriteria.NobodyWins = false;
      TownOfUs.JesterMod.EndCriteria.JesterVotedOut = false;
      Utils.Null();
      Footprint.DestroyAll();
      RecordRewind.points.Clear();
      MessageWriter messageWriter1 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 71, (SendOption) 1, -1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter1);
      List<FFGALNAPKCD> crewmates = Utils.getCrewmates((IEnumerable<EGLJNOMOGNP.DCJMABDDJCF>) infected);
      bool flag1 = RpcHandling.Check(CustomGameOptions.MayorOn);
      bool flag2 = RpcHandling.Check(CustomGameOptions.JesterOn);
      bool flag3 = RpcHandling.Check(CustomGameOptions.SheriffOn);
      bool flag4 = RpcHandling.Check(CustomGameOptions.LoversOn);
      bool flag5 = RpcHandling.Check(CustomGameOptions.MafiaOn);
      bool flag6 = RpcHandling.Check(CustomGameOptions.EngineerOn);
      bool flag7 = RpcHandling.Check(CustomGameOptions.SwapperOn);
      bool flag8 = RpcHandling.Check(CustomGameOptions.ShifterOn);
      bool flag9 = RpcHandling.Check(CustomGameOptions.InvestigatorOn);
      bool flag10 = RpcHandling.Check(CustomGameOptions.TimeMasterOn);
      if (flag1)
      {
        Utils.Mayor = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Mayor.get_PlayerId();
        crewmates.Remove(Utils.Mayor);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 45, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (crewmates.Count <= 0)
        return;
      if (flag2)
      {
        Utils.Jester = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Jester.get_PlayerId();
        crewmates.Remove(Utils.Jester);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 43, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (crewmates.Count <= 0)
        return;
      if (flag3)
      {
        Utils.Sheriff = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Sheriff.get_PlayerId();
        crewmates.Remove(Utils.Sheriff);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 46, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
        if (CustomGameOptions.ShowSheriff)
          Utils.Sheriff.get_nameText().set_Color(new Color(1f, 0.8f, 0.0f, 1f));
      }
      if (crewmates.Count <= 0)
        return;
      if (flag6)
      {
        Utils.Engineer = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Engineer.get_PlayerId();
        crewmates.Remove(Utils.Engineer);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 47, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (flag4)
      {
        if (crewmates.Count <= 1)
          return;
        List<FFGALNAPKCD> impostors = Utils.getImpostors((IEnumerable<EGLJNOMOGNP.DCJMABDDJCF>) infected);
        byte num = (byte) RpcHandling.Rand.Next(0, 3);
        if (num == (byte) 0 & flag5)
          num = (byte) 1;
        bool flag11 = num == (byte) 0;
        int index1 = RpcHandling.Rand.Next(0, crewmates.Count);
        FFGALNAPKCD ffgalnapkcd1 = crewmates[index1];
        crewmates.Remove(ffgalnapkcd1);
        FFGALNAPKCD ffgalnapkcd2;
        if (flag11)
        {
          int index2 = RpcHandling.Rand.Next(0, impostors.Count);
          ffgalnapkcd2 = impostors[index2];
        }
        else
        {
          int index2 = RpcHandling.Rand.Next(0, crewmates.Count);
          ffgalnapkcd2 = crewmates[index2];
          crewmates.Remove(ffgalnapkcd2);
        }
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 44, (SendOption) 1, -1);
        messageWriter2.Write(ffgalnapkcd1.get_PlayerId());
        messageWriter2.Write(ffgalnapkcd2.get_PlayerId());
        messageWriter2.Write(num);
        Utils.Lover1 = ffgalnapkcd1;
        Utils.Lover2 = ffgalnapkcd2;
        Utils.LoverImpostor = flag11;
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (flag5)
      {
        List<FFGALNAPKCD> impostors = Utils.getImpostors((IEnumerable<EGLJNOMOGNP.DCJMABDDJCF>) infected);
        if (impostors.Count >= 3)
        {
          Utils.Godfather = impostors[RpcHandling.Rand.Next(0, impostors.Count)];
          impostors.Remove(Utils.Godfather);
          Utils.Mafioso = impostors[RpcHandling.Rand.Next(0, impostors.Count)];
          impostors.Remove(Utils.Mafioso);
          Utils.Janitor = impostors[0];
          MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 52, (SendOption) 1, -1);
          messageWriter2.Write(Utils.Godfather.get_PlayerId());
          ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
          MessageWriter messageWriter3 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 54, (SendOption) 1, -1);
          messageWriter3.Write(Utils.Mafioso.get_PlayerId());
          ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter3);
          MessageWriter messageWriter4 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 53, (SendOption) 1, -1);
          messageWriter4.Write(Utils.Janitor.get_PlayerId());
          ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter4);
        }
      }
      if (crewmates.Count <= 0)
        return;
      if (flag7)
      {
        Utils.Swapper = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Swapper.get_PlayerId();
        crewmates.Remove(Utils.Swapper);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 48, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (crewmates.Count <= 0)
        return;
      if (flag8)
      {
        Utils.Shifter = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Shifter.get_PlayerId();
        crewmates.Remove(Utils.Shifter);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 49, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (crewmates.Count <= 0)
        return;
      if (flag9)
      {
        Utils.Investigator = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
        byte playerId = Utils.Investigator.get_PlayerId();
        crewmates.Remove(Utils.Investigator);
        MessageWriter messageWriter2 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 50, (SendOption) 1, -1);
        messageWriter2.Write(playerId);
        ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter2);
      }
      if (crewmates.Count <= 0 || !flag10)
        return;
      Utils.TimeMaster = crewmates[RpcHandling.Rand.Next(0, crewmates.Count)];
      byte playerId1 = Utils.TimeMaster.get_PlayerId();
      crewmates.Remove(Utils.TimeMaster);
      MessageWriter messageWriter5 = ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).StartRpcImmediately(((NJAHILONGKN) FFGALNAPKCD.get_LocalPlayer()).get_NetId(), (byte) 51, (SendOption) 1, -1);
      messageWriter5.Write(playerId1);
      ((KHNHJFFECBP) FMLLKEACGIO.get_Instance()).FinishRpcImmediately(messageWriter5);
    }
  }
}
