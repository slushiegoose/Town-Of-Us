using System.Linq;
using HarmonyLib;
using Hazel;
using Il2CppSystem;
using Reactor;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class RpcHandling
    {
        public static readonly System.Random Rand = new System.Random();

        [HarmonyPatch(nameof(PlayerControl.HandleRpc))]
        public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {

            byte readByte;
            sbyte readSByte, readSByte2;
            switch ((CustomRPC) callId)
            {
              case CustomRPC.SetMayor:
                  readByte = reader.ReadByte();
                  Utils.Mayor = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetJester:
                  readByte = reader.ReadByte();
                  Utils.Jester = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetSheriff:
                  readByte = reader.ReadByte();
                  Utils.Sheriff = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetEngineer:
                  readByte = reader.ReadByte();
                  Utils.Engineer = Utils.PlayerById(readByte);
                  break;

              case CustomRPC.SetGodfather:
                  readByte = reader.ReadByte();
                  Utils.Godfather = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetMafioso:
                  readByte = reader.ReadByte();
                  Utils.Mafioso = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetJanitor:
                  readByte = reader.ReadByte();
                  Utils.Janitor = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetSwapper:
                  readByte = reader.ReadByte();
                  Utils.Swapper = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetShifter:
                  readByte = reader.ReadByte();
                  Utils.Shifter = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetInvestigator:
                  readByte = reader.ReadByte();
                  Utils.Investigator = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.SetTimeMaster:
                  readByte = reader.ReadByte();
                  Utils.TimeMaster = Utils.PlayerById(readByte);
                  break;
              
              case CustomRPC.JesterWin:
                  Utils.Jester.Data.IsDead = false;
                  break;
              
              case CustomRPC.LoveWin:
                  LoversMod.Methods.LoversWin();
                  break;
              
              case CustomRPC.JesterLose:
                  Utils.Jester.Data.IsImpostor = true;
                  break;
              
              case CustomRPC.ShifterLose:
                  Utils.Shifter.Data.IsImpostor = true;
                  break;
              
              case CustomRPC.NobodyWins:
                  LoversMod.Methods.NobodyWins();
                  break;
              
              case CustomRPC.SetCouple:
                  var id = reader.ReadByte();
                  var id2 = reader.ReadByte();
                  var b1 = reader.ReadByte();
                  var lover1 = Utils.PlayerById(id);
                  var lover2 = Utils.PlayerById(id2);
                  Utils.Lover1 = lover1;
                  Utils.Lover2 = lover2;
                  Utils.LoverImpostor = b1 == 0;
                  break;
              
              case CustomRPC.LoveSuicide:
                  var id3 = reader.ReadByte();
                  var player = Utils.PlayerById(id3);
                  player.MurderPlayer(player);
                  break;
              
              case CustomRPC.SheriffKill:
                  var player1 = Utils.PlayerById(reader.ReadByte());
                  var player2 = Utils.PlayerById(reader.ReadByte());
                  var flag = player1.isSheriff();
                  if (flag)
                  {
                      player1.MurderPlayer(player2);
                  }
                  SheriffMod.Methods.LastKilled = System.DateTime.UtcNow;

                  break;

              case CustomRPC.Start:
                  MayorMod.AllowExtraVotes.VoteBank = CustomGameOptions.MayorVoteBank;
                  EngineerMod.PerformKill.UsedThisRound = false;
                  EngineerMod.PerformKill.SabotageTime = DateTime.Now.AddSeconds(-100);
                  LoversMod.EndCriteria.LoveCoupleWins = false;
                  LoversMod.EndCriteria.NobodyWins = false;
                  JesterMod.EndCriteria.JesterVotedOut = false;
                  Utils.Null();
                  InvestigatorMod.Footprint.DestroyAll();
                  TimeMasterMod.RecordRewind.points.Clear();
                  break;
              
              case CustomRPC.JanitorClean:
                  readByte = reader.ReadByte();
                  var deadBodies = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                  foreach (var body in deadBodies)
                  {
                      if (body.ParentId == readByte)
                      {
                          Coroutines.Start(MafiaMod.Janitor.Coroutine.CleanCoroutine(body));
                      }
                  }

                  break;
              
              case CustomRPC.FixLights:
                  var lights = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                  lights.ActualSwitches = lights.ExpectedSwitches;
                  break;
              
              case CustomRPC.SetArrows:
                  EngineerMod.Arrows.GenArrows();
                  break;
                  
              case CustomRPC.RemoveArrows:
                  EngineerMod.Arrows.RemoveArrows();
                  break;
              
              case CustomRPC.SetExtraVotes:
                  MayorMod.AllowExtraVotes.ExtraVotes = reader.ReadBytesAndSize().ToList();
                  if (!PlayerControl.LocalPlayer.isMayor())
                  {
                      MayorMod.AllowExtraVotes.VoteBank -= MayorMod.AllowExtraVotes.ExtraVotes.Count;
                  }

                  break;
              
              case CustomRPC.SetSwaps:
                  readSByte = reader.ReadSByte();
                  SwapperMod.SwapVotes.Swap1 =
                      MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == readSByte);
                  readSByte2 = reader.ReadSByte();
                  SwapperMod.SwapVotes.Swap2 =
                      MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == readSByte2);
                  PluginSingleton<TownOfUs>.Instance.Log.LogMessage("Bytes received - " + readSByte + " - " + readSByte2);
                  break;
              
              case CustomRPC.Shift:
                  readByte = reader.ReadByte();
                  var other = Utils.PlayerById(readByte);
                  ShifterMod.PerformKillButton.Shift(Utils.Shifter, other);
                  break;
              case CustomRPC.Rewind:
                  TimeMasterMod.StartStop.StartRewind();
                  break;
              case CustomRPC.RewindRevive:
                  readByte = reader.ReadByte();
                  TimeMasterMod.RecordRewind.ReviveBody(Utils.PlayerById(readByte));
                  break;
            }
        }

        private static bool Check(int probability)
        {
            var num = Rand.Next(1, 101);
            return num <= probability;
        }

        [HarmonyPatch("RpcSetInfected")]
        public static void Postfix([HarmonyArgument(0)] Il2CppReferenceArray<GameData.PlayerInfo> infected)
        {
            //TODO - Instantiate role-specific stuff
            MayorMod.AllowExtraVotes.VoteBank = CustomGameOptions.MayorVoteBank;
            EngineerMod.PerformKill.UsedThisRound = false;
            EngineerMod.PerformKill.SabotageTime = DateTime.Now.AddSeconds(-100);
            LoversMod.EndCriteria.LoveCoupleWins = false;
            LoversMod.EndCriteria.NobodyWins = false;
            JesterMod.EndCriteria.JesterVotedOut = false;
            Utils.Null();
            InvestigatorMod.Footprint.DestroyAll();
            TimeMasterMod.RecordRewind.points.Clear();

            var startWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Start, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(startWriter);

            var crewmates = Utils.getCrewmates(infected);
            var mayorOn = Check(CustomGameOptions.MayorOn);
            var jesterOn = Check(CustomGameOptions.JesterOn);
            var sheriffOn = Check(CustomGameOptions.SheriffOn);
            var loversOn = Check(CustomGameOptions.LoversOn);
            var mafiaOn = Check(CustomGameOptions.MafiaOn);
            var engineerOn = Check(CustomGameOptions.EngineerOn);
            var swapperOn = Check(CustomGameOptions.SwapperOn);
            var shifterOn = Check(CustomGameOptions.ShifterOn);
            var investigatorOn = Check(CustomGameOptions.InvestigatorOn);
            var timeMasterOn = Check(CustomGameOptions.TimeMasterOn);
            
            
            if (mayorOn)
            {
                Utils.Mayor = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Mayor.PlayerId;
                crewmates.Remove(Utils.Mayor);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetMayor, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            
            if (crewmates.Count <= 0) return;
            
            if (jesterOn)
            {
                Utils.Jester = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Jester.PlayerId;
                crewmates.Remove(Utils.Jester);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetJester, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            if (crewmates.Count <= 0) return;
            
            if (sheriffOn)
            {
                Utils.Sheriff = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Sheriff.PlayerId;
                crewmates.Remove(Utils.Sheriff);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetSheriff, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                if (CustomGameOptions.ShowSheriff)
                {
                    Utils.Sheriff.nameText.Color = new Color(1f, 0.8f, 0f, 1f);
                }
            }
            
            if (crewmates.Count <= 0) return;
            
            if (engineerOn)
            {
                Utils.Engineer = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Engineer.PlayerId;
                crewmates.Remove(Utils.Engineer);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetEngineer, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            
            

            if (loversOn)
            {
                if (crewmates.Count <= 1) return;
                var impostors = Utils.getImpostors(infected);

                var b = (byte) RpcHandling.Rand.Next(0, 3);

                if (b == 0 & mafiaOn)
                {
                    b = 1;
                }

                var flag2 = b == 0;
                var num = RpcHandling.Rand.Next(0, crewmates.Count);
                var player1 = crewmates[num];
                crewmates.Remove(player1);
                PlayerControl player2;
                if (flag2)
                {
                    var num2 = RpcHandling.Rand.Next(0, impostors.Count);
                    player2 = impostors[num2];
                }
                else
                {
                    var num2 = RpcHandling.Rand.Next(0, crewmates.Count);
                    player2 = crewmates[num2];
                    crewmates.Remove(player2);
                }

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetCouple, SendOption.Reliable, -1);
                writer.Write(player1.PlayerId);
                writer.Write(player2.PlayerId);
                writer.Write(b);
                Utils.Lover1 = player1;
                Utils.Lover2 = player2;
                Utils.LoverImpostor = flag2;

                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            

            if (mafiaOn)
            {
                var impostors = Utils.getImpostors(infected);
                if (impostors.Count >= 3)
                {
                    Utils.Godfather = impostors[Rand.Next(0, impostors.Count)];
                    impostors.Remove(Utils.Godfather);
                    Utils.Mafioso = impostors[Rand.Next(0, impostors.Count)];
                    impostors.Remove(Utils.Mafioso);
                    Utils.Janitor = impostors[0];

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.SetGodfather, SendOption.Reliable, -1);
                    writer.Write(Utils.Godfather.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.SetMafioso, SendOption.Reliable, -1);
                    writer1.Write(Utils.Mafioso.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer1);
                    var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.SetJanitor, SendOption.Reliable, -1);
                    writer2.Write(Utils.Janitor.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                }
            }
            
            if (crewmates.Count <= 0) return;
            
            if (swapperOn)
            {
                Utils.Swapper = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Swapper.PlayerId;
                crewmates.Remove(Utils.Swapper);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetSwapper, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            if (crewmates.Count <= 0) return;
            
            if (shifterOn)
            {
                Utils.Shifter = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Shifter.PlayerId;
                crewmates.Remove(Utils.Shifter);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetShifter, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            
            if (crewmates.Count <= 0) return;
            if (investigatorOn)
            {
                Utils.Investigator = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.Investigator.PlayerId;
                crewmates.Remove(Utils.Investigator);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetInvestigator, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            
            if (crewmates.Count <= 0) return;
            if (timeMasterOn)
            {
                Utils.TimeMaster = crewmates[Rand.Next(0, crewmates.Count)];
                var playerId = Utils.TimeMaster.PlayerId;
                crewmates.Remove(Utils.TimeMaster);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.SetTimeMaster, SendOption.Reliable, -1);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        
    }
}