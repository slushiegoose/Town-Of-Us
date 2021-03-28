using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
//using Il2CppSystem;
using Reactor;
using Reactor.Extensions;
using TownOfUs.Roles;
using UnhollowerBaseLib;
using UnityEngine;
using Coroutine = TownOfUs.JanitorMod.Coroutine;

namespace TownOfUs
{
    public static class RpcHandling
    {
        //public static readonly System.Random Rand = new System.Random();

        private static readonly List<(Type, CustomRPC)> CrewmateRoles = new List<(Type, CustomRPC)>();
        private static readonly List<(Type, CustomRPC)> NeutralRoles = new List<(Type, CustomRPC)>();
        private static readonly List<(Type, CustomRPC)> ImpostorRoles = new List<(Type, CustomRPC)>();
        private static readonly List<(Type, CustomRPC)> CrewmateModifiers = new List<(Type, CustomRPC)>();
        private static readonly List<(Type, CustomRPC)> GlobalModifiers = new List<(Type, CustomRPC)>();
        private static bool LoversOn = false;



        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class HandleRpc
        {
            public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {

                //if (callId >= 43) //System.Console.WriteLine("Received " + callId);
                byte readByte, readByte1, readByte2;
                sbyte readSByte, readSByte2;
                switch ((CustomRPC) callId)
                {
                    case CustomRPC.SetMayor:
                        readByte = reader.ReadByte();
                        new Roles.Mayor(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetJester:
                        readByte = reader.ReadByte();
                        new Roles.Jester(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetSheriff:
                        readByte = reader.ReadByte();
                        new Roles.Sheriff(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetEngineer:
                        readByte = reader.ReadByte();
                        new Roles.Engineer(Utils.PlayerById(readByte));
                        break;


                    case CustomRPC.SetJanitor:
                        new Roles.Janitor(Utils.PlayerById(reader.ReadByte()));

                        break;

                    case CustomRPC.SetSwapper:
                        readByte = reader.ReadByte();
                        new Roles.Swapper(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetShifter:
                        readByte = reader.ReadByte();
                        new Roles.Shifter(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetInvestigator:
                        readByte = reader.ReadByte();
                        new Roles.Investigator(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetTimeLord:
                        readByte = reader.ReadByte();
                        new Roles.TimeLord(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetTorch:
                        readByte = reader.ReadByte();
                        new Roles.Torch(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetDiseased:
                        readByte = reader.ReadByte();
                        new Roles.Diseased(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetFlash:
                        readByte = reader.ReadByte();
                        new Roles.Flash(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetMedic:
                        readByte = reader.ReadByte();
                        new Roles.Medic(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetMorphling:
                        readByte = reader.ReadByte();
                        new Roles.Morphling(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.LoveWin:
                        var winnerlover = Utils.PlayerById(reader.ReadByte());
                        Role.GetRole<Lover>(winnerlover).Win();
                        break;


                    case CustomRPC.JesterLose:
                        foreach (var role in Role.AllRoles)
                        {
                            if (role.RoleType == RoleEnum.Jester)
                            {
                                ((Jester) role).Loses();
                            }
                        }

                        break;

                    case CustomRPC.GlitchLose:
                        foreach (var role in Role.AllRoles)
                        {
                            if (role.RoleType == RoleEnum.Glitch)
                            {
                                ((Glitch) role).Loses();
                            }
                        }

                        break;

                    case CustomRPC.ShifterLose:
                        foreach (var role in Role.AllRoles)
                        {
                            if (role.RoleType == RoleEnum.Shifter)
                            {
                                ((Shifter) role).Loses();
                            }
                        }

                        break;

                    case CustomRPC.ExecutionerLose:
                        foreach (var role in Role.AllRoles)
                        {
                            if (role.RoleType == RoleEnum.Executioner)
                            {
                                ((Executioner) role).Loses();
                            }
                        }

                        break;

                    case CustomRPC.NobodyWins:
                        Role.NobodyWinsFunc();
                        break;

                    case CustomRPC.SetCouple:
                        var id = reader.ReadByte();
                        var id2 = reader.ReadByte();
                        var b1 = reader.ReadByte();
                        var lover1 = Utils.PlayerById(id);
                        var lover2 = Utils.PlayerById(id2);

                        var roleLover1 = new Roles.Lover(lover1, 1, b1 == 0);
                        var roleLover2 = new Roles.Lover(lover2, 2, b1 == 0);

                        roleLover1.OtherLover = roleLover2;
                        roleLover2.OtherLover = roleLover1;

                        break;

                    case CustomRPC.Start:
                        /*
                        EngineerMod.PerformKill.UsedThisRound = false;
                        EngineerMod.PerformKill.SabotageTime = DateTime.UtcNow.AddSeconds(-100);
                        */
                        MedicMod.Murder.KilledPlayers.Clear();
                        Role.NobodyWins = false;
                        TimeLordMod.RecordRewind.points.Clear();
                        break;

                    case CustomRPC.JanitorClean:
                        readByte1 = reader.ReadByte();
                        var janitorPlayer = Utils.PlayerById(readByte1);
                        var janitorRole = Roles.Role.GetRole<Roles.Janitor>(janitorPlayer);
                        readByte = reader.ReadByte();
                        var deadBodies = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in deadBodies)
                        {
                            if (body.ParentId == readByte)
                            {
                                Coroutines.Start(Coroutine.CleanCoroutine(body, janitorRole));
                            }
                        }

                        break;
                    case CustomRPC.EngineerFix:
                        var engineer = Utils.PlayerById(reader.ReadByte());
                        Roles.Role.GetRole<Roles.Engineer>(engineer).UsedThisRound = true;
                        break;



                    case CustomRPC.FixLights:
                        var lights = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                        lights.ActualSwitches = lights.ExpectedSwitches;
                        break;

                    case CustomRPC.SetExtraVotes:

                        var mayor = Utils.PlayerById(reader.ReadByte());
                        var mayorRole = Roles.Role.GetRole<Mayor>(mayor);
                        mayorRole.ExtraVotes = reader.ReadBytesAndSize().ToList();
                        if (!mayor.Is(RoleEnum.Mayor))
                        {
                            mayorRole.VoteBank -= mayorRole.ExtraVotes.Count;
                        }

                        break;

                    case CustomRPC.SetSwaps:
                        readSByte = reader.ReadSByte();
                        SwapperMod.SwapVotes.Swap1 =
                            MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == readSByte);
                        readSByte2 = reader.ReadSByte();
                        SwapperMod.SwapVotes.Swap2 =
                            MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == readSByte2);
                        PluginSingleton<TownOfUs>.Instance.Log.LogMessage("Bytes received - " + readSByte + " - " +
                                                                          readSByte2);
                        break;

                    case CustomRPC.Shift:
                        readByte1 = reader.ReadByte();
                        readByte2 = reader.ReadByte();
                        var shifter = Utils.PlayerById(readByte1);
                        var other = Utils.PlayerById(readByte2);
                        ShifterMod.PerformKillButton.Shift(Role.GetRole<Shifter>(shifter), other);
                        break;
                    case CustomRPC.Rewind:
                        readByte = reader.ReadByte();
                        var TimeLordPlayer = Utils.PlayerById(readByte);
                        var TimeLordRole = Role.GetRole<Roles.TimeLord>(TimeLordPlayer);
                        TimeLordMod.StartStop.StartRewind(TimeLordRole);
                        break;
                    case CustomRPC.Protect:
                        readByte1 = reader.ReadByte();
                        readByte2 = reader.ReadByte();

                        var medic = Utils.PlayerById(readByte1);
                        var shield = Utils.PlayerById(readByte2);
                        Role.GetRole<Medic>(medic).ShieldedPlayer = shield;
                        Role.GetRole<Medic>(medic).UsedAbility = true;
                        break;
                    case CustomRPC.RewindRevive:
                        readByte = reader.ReadByte();
                        TimeLordMod.RecordRewind.ReviveBody(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.AttemptSound:
                        readByte = reader.ReadByte();
                        MedicMod.StopKill.BreakShield(readByte, false);
                        break;
                    case CustomRPC.SetGlitch:
                        byte GlitchId = reader.ReadByte();
                        PlayerControl GlitchPlayer = Utils.PlayerById(GlitchId);
                        new Roles.Glitch(GlitchPlayer);
                        break;
                    case CustomRPC.BypassKill:
                        PlayerControl killer = Utils.PlayerById(reader.ReadByte());
                        PlayerControl target = Utils.PlayerById(reader.ReadByte());

                        Utils.MurderPlayer(killer, target);
                        break;
                    case CustomRPC.SetMimic:
                        PlayerControl glitchPlayer = Utils.PlayerById(reader.ReadByte());
                        PlayerControl mimicPlayer = Utils.PlayerById(reader.ReadByte());
                        var glitchRole = Roles.Role.GetRole<Roles.Glitch>(glitchPlayer);
                        glitchRole.MimicTarget = mimicPlayer;
                        glitchRole.IsUsingMimic = true;
                        Utils.Morph(glitchPlayer, mimicPlayer);
                        break;
                    case CustomRPC.RpcResetAnim:
                        PlayerControl animPlayer = Utils.PlayerById(reader.ReadByte());
                        var theGlitchRole = Roles.Role.GetRole<Roles.Glitch>(animPlayer);
                        theGlitchRole.MimicTarget = null;
                        theGlitchRole.IsUsingMimic = false;
                        Utils.Unmorph(theGlitchRole.Player);
                        break;
                    case CustomRPC.GlitchWin:
                        var theGlitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
                        ((Roles.Glitch) theGlitch)?.Wins();
                        break;
                    case CustomRPC.SetHacked:
                        PlayerControl hackPlayer = Utils.PlayerById(reader.ReadByte());
                        if (hackPlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            var glitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
                            ((Roles.Glitch) glitch)?.SetHacked(hackPlayer);
                        }

                        break;
                    case CustomRPC.Investigate:
                        var seer = Utils.PlayerById(reader.ReadByte());
                        var otherPlayer = Utils.PlayerById(reader.ReadByte());
                        Roles.Role.GetRole<Roles.Seer>(seer).Investigated.Add(otherPlayer.PlayerId);
                        Roles.Role.GetRole<Roles.Seer>(seer).LastInvestigated = DateTime.UtcNow;
                        break;
                    case CustomRPC.SetSeer:
                        new Roles.Seer(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Morph:
                        var morphling = Utils.PlayerById(reader.ReadByte());
                        var morphTarget = Utils.PlayerById(reader.ReadByte());
                        var morphRole = Roles.Role.GetRole<Morphling>(morphling);
                        morphRole.TimeRemaining = CustomGameOptions.MorphlingDuration;
                        morphRole.MorphedPlayer = morphTarget;
                        break;
                    case CustomRPC.SetExecutioner:
                        new Roles.Executioner(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetTarget:
                        var executioner = Utils.PlayerById(reader.ReadByte());
                        var exeTarget = Utils.PlayerById(reader.ReadByte());
                        var exeRole = Roles.Role.GetRole<Executioner>(executioner);
                        exeRole.target = exeTarget;
                        break;
                    case CustomRPC.SetChild:
                        new Roles.Child(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetCamouflager:
                        new Roles.Camouflager(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Camouflage:
                        var camouflager = Utils.PlayerById(reader.ReadByte());
                        var camouflagerRole = Roles.Role.GetRole<Camouflager>(camouflager);
                        camouflagerRole.TimeRemaining = CustomGameOptions.CamouflagerDuration;
                        Utils.Camouflage();
                        break;
                    case CustomRPC.SetSpy:
                        new Roles.Spy(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.ExecutionerToJester:
                        ExecutionerMod.TargetColor.ExeToJes(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetSnitch:
                        new Roles.Snitch(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetMiner:
                        new Roles.Miner(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Mine:
                        var ventId = reader.ReadInt32();
                        var miner = Utils.PlayerById(reader.ReadByte());
                        var minerRole = Roles.Role.GetRole<Roles.Miner>(miner);
                        var pos = reader.ReadVector2();
                        var zAxis = reader.ReadSingle();
                        MinerMod.PerformKill.SpawnVent(ventId, minerRole, pos, zAxis);
                        break;
                    case CustomRPC.SetSwooper:
                        new Roles.Swooper(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Swoop:
                        var swooper = Utils.PlayerById(reader.ReadByte());
                        var swooperRole = Role.GetRole<Swooper>(swooper);
                        swooperRole.TimeRemaining = CustomGameOptions.SwoopDuration;
                        swooperRole.Swoop();
                        break;
                    case CustomRPC.SetTiebreaker:
                        new Tiebreaker(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetDrunk:
                        new Drunk(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetArsonist:
                        new Arsonist(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Douse:
                        var arsonist = Utils.PlayerById(reader.ReadByte());
                        var douseTarget = Utils.PlayerById(reader.ReadByte());
                        var arsonistRole = Roles.Role.GetRole<Roles.Arsonist>(arsonist);
                        arsonistRole.DousedPlayers.Add(douseTarget.PlayerId);
                        arsonistRole.LastDoused = DateTime.UtcNow;
                        
                        break;
                    case CustomRPC.Ignite:
                        var theArsonist = Utils.PlayerById(reader.ReadByte());
                        var theArsonistRole = Roles.Role.GetRole<Roles.Arsonist>(theArsonist);
                        ArsonistMod.PerformKill.Ignite(theArsonistRole);
                        break;

                    case CustomRPC.ArsonistWin:
                        var theArsonistTheRole = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Arsonist);
                        ((Roles.Arsonist) theArsonistTheRole)?.Wins();
                        break;
                    case CustomRPC.ArsonistLose:
                        foreach (var role in Role.AllRoles)
                        {
                            if (role.RoleType == RoleEnum.Arsonist)
                            {
                                ((Arsonist) role).Loses();
                            }
                        }

                        break;
                    case CustomRPC.SetImpostor:
                        new Impostor(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetCrewmate:
                        new Crewmate(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SyncCustomSettings:
                        CustomOption.Rpc.ReceiveRpc(reader);
                        break;
                }
            }
        }

        private static bool Check(int probability)
        {
            //System.Console.WriteLine("Check");
            var num = HashRandom.Method_1( 101) + 1;
            return num <= probability;
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
        public static class RpcSetInfected
        {
            public static void Prefix([HarmonyArgument(0)] Il2CppReferenceArray<GameData.PlayerInfo> infected)
            {
                //System.Console.WriteLine("REACHED HERE");
                
                Role.NobodyWins = false;
                CrewmateRoles.Clear();
                NeutralRoles.Clear();
                ImpostorRoles.Clear();
                CrewmateModifiers.Clear();
                GlobalModifiers.Clear();
                


                //TODO - Instantiate role-specific stuff
                /*EngineerMod.PerformKill.UsedThisRound = false;
                EngineerMod.PerformKill.SabotageTime = DateTime.UtcNow.AddSeconds(-100);*/
                TimeLordMod.RecordRewind.points.Clear();
                MedicMod.Murder.KilledPlayers.Clear();

                var startWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.Start, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(startWriter);

                var crewmates = Utils.getCrewmates(infected);


                LoversOn = Check(CustomGameOptions.LoversOn);
                

                if (Check(CustomGameOptions.MayorOn)) CrewmateRoles.Add((typeof(Mayor), CustomRPC.SetMayor));

                if (Check(CustomGameOptions.JesterOn)) NeutralRoles.Add((typeof(Jester), CustomRPC.SetJester));

                if (Check(CustomGameOptions.SheriffOn)) CrewmateRoles.Add((typeof(Sheriff), CustomRPC.SetSheriff));

                if (Check(CustomGameOptions.EngineerOn)) CrewmateRoles.Add((typeof(Engineer), CustomRPC.SetEngineer));

                if (Check(CustomGameOptions.SwapperOn)) CrewmateRoles.Add((typeof(Swapper), CustomRPC.SetSwapper));

                if (Check(CustomGameOptions.ShifterOn)) NeutralRoles.Add((typeof(Shifter), CustomRPC.SetShifter));

                if (Check(CustomGameOptions.InvestigatorOn))
                    CrewmateRoles.Add((typeof(Investigator), CustomRPC.SetInvestigator));

                if (Check(CustomGameOptions.TimeLordOn))
                    CrewmateRoles.Add((typeof(TimeLord), CustomRPC.SetTimeLord));

                if (Check(CustomGameOptions.MedicOn)) CrewmateRoles.Add((typeof(Medic), CustomRPC.SetMedic));

                if (Check(CustomGameOptions.GlitchOn)) NeutralRoles.Add((typeof(Glitch), CustomRPC.SetGlitch));

                if (Check(CustomGameOptions.SeerOn)) CrewmateRoles.Add((typeof(Seer), CustomRPC.SetSeer));

                if (Check(CustomGameOptions.TorchOn)) CrewmateModifiers.Add((typeof(Torch), CustomRPC.SetTorch));

                if (Check(CustomGameOptions.DiseasedOn)) CrewmateModifiers.Add((typeof(Diseased), CustomRPC.SetDiseased));

                if (Check(CustomGameOptions.MorphlingOn))
                    ImpostorRoles.Add((typeof(Morphling), CustomRPC.SetMorphling));
                
                if (Check(CustomGameOptions.ChildOn)) CrewmateRoles.Add((typeof(Child), CustomRPC.SetChild));

                if (Check(CustomGameOptions.CamouflagerOn)) ImpostorRoles.Add((typeof(Camouflager), CustomRPC.SetCamouflager));
                
                if (Check(CustomGameOptions.SpyOn)) CrewmateRoles.Add((typeof(Spy), CustomRPC.SetSpy));
                
                if (Check(CustomGameOptions.SnitchOn)) CrewmateRoles.Add((typeof(Snitch), CustomRPC.SetSnitch));
                
                if (Check(CustomGameOptions.MinerOn)) ImpostorRoles.Add((typeof(Miner), CustomRPC.SetMiner));
                
                if (Check(CustomGameOptions.SwooperOn)) ImpostorRoles.Add((typeof(Swooper), CustomRPC.SetSwooper));
                
                if (Check(CustomGameOptions.TiebreakerOn)) GlobalModifiers.Add((typeof(Tiebreaker), CustomRPC.SetTiebreaker));
                
                if (Check(CustomGameOptions.FlashOn)) GlobalModifiers.Add((typeof(Flash), CustomRPC.SetFlash));
                
                if (Check(CustomGameOptions.JanitorOn)) ImpostorRoles.Add((typeof(Janitor), CustomRPC.SetJanitor));
                
                if (Check(CustomGameOptions.DrunkOn)) GlobalModifiers.Add((typeof(Drunk), CustomRPC.SetDrunk));
                
                if (Check(CustomGameOptions.ArsonistOn)) NeutralRoles.Add((typeof(Arsonist), CustomRPC.SetArsonist));
                
                if (Check(CustomGameOptions.ExecutionerOn)) NeutralRoles.Add((typeof(Executioner), CustomRPC.SetExecutioner));
                
                GenEachRole(infected.ToList());

            }
        }


        private static void GenExe(List<GameData.PlayerInfo> infected, List<PlayerControl> crewmates)
        {
            PlayerControl pc;
            var targets = Utils.getCrewmates(infected).Where(x =>
            {
                var role = Role.GetRole(x);
                if (role == null) return true;
                return role.Faction == Faction.Crewmates && !x.Is(RoleEnum.Child);
            }).ToList();
            if (targets.Count != 0)
            {
                var rand = HashRandom.Method_1(targets.Count);
                pc = targets[rand];
                var role = Role.Gen(typeof(Executioner), crewmates.Where(x => x.PlayerId != pc.PlayerId).ToList(),
                    CustomRPC.SetExecutioner);
                if (role != null)
                {
                    crewmates.Remove(role.Player);
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.SetTarget, SendOption.Reliable, -1);
                    writer.Write(role.Player.PlayerId);
                    writer.Write(pc.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    ((Executioner) role).target = pc;
                }
            }
        }

        private static void GenEachRole(List<GameData.PlayerInfo> infected)
        {
            
            //System.Console.WriteLine("REACHED HERE - GEN ROLES");
            CrewmateRoles.Shuffle();
            NeutralRoles.Shuffle();
            var neutralRoles = NeutralRoles.Take(CustomGameOptions.MaxNeutralRoles).ToList();
            var crewAndNeutRoles = neutralRoles;
            crewAndNeutRoles.AddRange(CrewmateRoles);
            crewAndNeutRoles.Shuffle();
            
            CrewmateModifiers.Shuffle();
            GlobalModifiers.Shuffle();
            
            ImpostorRoles.Shuffle();
            var impRoles = ImpostorRoles.Take(CustomGameOptions.MaxImpostorRoles);

            var crewmates = Utils.getCrewmates(infected);
            var impostors = Utils.getImpostors(infected);

            var executionerOn = false;
            
            foreach (var (role, rpc) in crewAndNeutRoles)
            {

                if (rpc == CustomRPC.SetExecutioner)
                {
                    executionerOn = true;
                    continue;
                }

                //System.Console.WriteLine(role);
                //System.Console.WriteLine(rpc);
                Role.Gen(role, crewmates, rpc);
            }

            if (executionerOn)
            {
                GenExe(infected, crewmates);
            }
       
            
            foreach (var (role, rpc) in impRoles)
            {
                //System.Console.WriteLine(role);
                //System.Console.WriteLine(rpc);
                Role.Gen(role, impostors, rpc);
            }

            var crewmates2 = Utils.getCrewmates(infected).Where(x => !x.Is(RoleEnum.Glitch) || !x.Is(RoleEnum.Morphling)).ToList();
            foreach (var (modifier, rpc) in CrewmateModifiers)
            {
                //System.Console.WriteLine(modifier);
                //System.Console.WriteLine(rpc);
                Modifier.Gen(modifier, crewmates2, rpc);
            }

            var global = PlayerControl.AllPlayerControls.ToArray().Where(x => Modifier.GetModifier(x) == null).ToList();
            foreach (var (modifier, rpc) in GlobalModifiers)
            {
                Modifier.Gen(modifier, global, rpc);
            }
            
            if (LoversOn)
            {
                //System.Console.WriteLine("LOVER1");
                Lover.Gen(crewmates, impostors);
            }

            while (true)
            {
                if (crewmates.Count == 0) break;
                Role.Gen(typeof(Crewmate), crewmates, CustomRPC.SetCrewmate);
            }

            while (true)
            {
                if (impostors.Count == 0) break;
                Role.Gen(typeof(Impostor), impostors, CustomRPC.SetImpostor);

            }



        }
        
    }
}
