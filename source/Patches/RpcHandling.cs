using System;
using System.Collections.Generic;
using System.Linq;
using Essentials.Options;
using HarmonyLib;
using Hazel;
//using Il2CppSystem;
using Reactor;
using TownOfUs.Roles;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs
{
    public static class RpcHandling
    {
        //public static readonly System.Random Rand = new System.Random();

        private static readonly List<(Type, CustomRPC)> CrewmateRoles = new List<(Type, CustomRPC)>();
        private static readonly List<(Type, CustomRPC)> ImpostorRoles = new List<(Type, CustomRPC)>();
        private static readonly List<(Type, CustomRPC)> ModifiersToAdd = new List<(Type, CustomRPC)>();
        private static bool LoversOn = false;
        private static bool MafiaOn = false;
        private static bool FlashOn = false;
        private static bool ExecutionerOn = false;



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


                    case CustomRPC.SetMafia:
                        var godfather = Utils.PlayerById(reader.ReadByte());
                        var janitor = Utils.PlayerById(reader.ReadByte());
                        var g = new Roles.Godfather(godfather);
                        var j = new Roles.Janitor(janitor);
                        g.Janitor = j;
                        j.Godfather = g;

                        if (!CustomGameOptions.TwoMafia)
                        {
                            var mafioso = Utils.PlayerById(reader.ReadByte());
                            var m = new Roles.Mafioso(mafioso);
                            g.Mafioso = m;
                            j.Mafioso = m;
                            m.Godfather = g;
                            m.Janitor = j;
                        }

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
                                Coroutines.Start(MafiaMod.Janitor.Coroutine.CleanCoroutine(body, janitorRole));
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
                ImpostorRoles.Clear();
                ModifiersToAdd.Clear();


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
                MafiaOn = Check(CustomGameOptions.MafiaOn);
                FlashOn = Check(CustomGameOptions.FlashOn);
                ExecutionerOn = Check(CustomGameOptions.ExecutionerOn);

                if (Check(CustomGameOptions.MayorOn)) CrewmateRoles.Add((typeof(Mayor), CustomRPC.SetMayor));

                if (Check(CustomGameOptions.JesterOn)) CrewmateRoles.Add((typeof(Jester), CustomRPC.SetJester));

                if (Check(CustomGameOptions.SheriffOn)) CrewmateRoles.Add((typeof(Sheriff), CustomRPC.SetSheriff));

                if (Check(CustomGameOptions.EngineerOn)) CrewmateRoles.Add((typeof(Engineer), CustomRPC.SetEngineer));

                if (Check(CustomGameOptions.SwapperOn)) CrewmateRoles.Add((typeof(Swapper), CustomRPC.SetSwapper));

                if (Check(CustomGameOptions.ShifterOn)) CrewmateRoles.Add((typeof(Shifter), CustomRPC.SetShifter));

                if (Check(CustomGameOptions.InvestigatorOn))
                    CrewmateRoles.Add((typeof(Investigator), CustomRPC.SetInvestigator));

                if (Check(CustomGameOptions.TimeLordOn))
                    CrewmateRoles.Add((typeof(TimeLord), CustomRPC.SetTimeLord));

                if (Check(CustomGameOptions.MedicOn)) CrewmateRoles.Add((typeof(Medic), CustomRPC.SetMedic));

                if (Check(CustomGameOptions.GlitchOn)) CrewmateRoles.Add((typeof(Glitch), CustomRPC.SetGlitch));

                if (Check(CustomGameOptions.SeerOn)) CrewmateRoles.Add((typeof(Seer), CustomRPC.SetSeer));

                if (Check(CustomGameOptions.TorchOn)) ModifiersToAdd.Add((typeof(Torch), CustomRPC.SetTorch));

                if (Check(CustomGameOptions.DiseasedOn)) ModifiersToAdd.Add((typeof(Diseased), CustomRPC.SetDiseased));

                if (Check(CustomGameOptions.MorphlingOn))
                    ImpostorRoles.Add((typeof(Morphling), CustomRPC.SetMorphling));
                
                if (Check(CustomGameOptions.ChildOn)) CrewmateRoles.Add((typeof(Child), CustomRPC.SetChild));

                if (Check(CustomGameOptions.CamouflagerOn)) ImpostorRoles.Add((typeof(Camouflager), CustomRPC.SetCamouflager));
                
                if (Check(CustomGameOptions.SpyOn)) CrewmateRoles.Add((typeof(Spy), CustomRPC.SetSpy));
                
                GenEachRole(infected.ToList());

            }
        }

        private static void GenEachRole(List<GameData.PlayerInfo> infected)
        {
            
            //System.Console.WriteLine("REACHED HERE - GEN ROLES");
            CrewmateRoles.Shuffle();
            ModifiersToAdd.Shuffle();
            ImpostorRoles.Shuffle();

            var crewmates = Utils.getCrewmates(infected);
            var impostors = Utils.getImpostors(infected);
            var targets = Utils.getCrewmates(infected);
            
            
            foreach (var (role, rpc) in CrewmateRoles)
            {

                //System.Console.WriteLine(role);
                //System.Console.WriteLine(rpc);
                Role.Gen(role, crewmates, rpc);
            }

            if (ExecutionerOn)
            {
                var foundTarget = true;
                PlayerControl pc;
                while (true)
                {
                    
                    var rand = HashRandom.Method_1(targets.Count);
                    pc = targets[rand];
                    var pcRole = Role.GetRole(pc);
                    if (pcRole == null)
                    {

                        if (crewmates.Count != 1) break;
                    }
                    else
                    {
                        if (pcRole.Faction == Faction.Crewmates && !pc.Is(RoleEnum.Child)) break;
                    }
                    targets.Remove(pc);
                    if (targets.Count == 0)
                    {
                        foundTarget = false;
                        break;
                    };
                }

                if (foundTarget)
                {
                    var role = Role.Gen(typeof(Executioner), crewmates.Where(x => x != pc).ToList(), CustomRPC.SetExecutioner);
                    if (role != null)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte) CustomRPC.SetTarget, SendOption.Reliable, -1);
                        writer.Write(role.Player.PlayerId);
                        writer.Write(pc.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        ((Executioner) role).target = pc;
                    }
                }

            }
            
            
            foreach (var (role, rpc) in ImpostorRoles)
            {
                //System.Console.WriteLine(role);
                //System.Console.WriteLine(rpc);
                Role.Gen(role, impostors, rpc);
            }

            var crewmates2 = Utils.getCrewmates(infected).Where(x => !x.Is(RoleEnum.Glitch)).ToList();
            foreach (var (modifier, rpc) in ModifiersToAdd)
            {
                //System.Console.WriteLine(modifier);
                //System.Console.WriteLine(rpc);
                Modifier.Gen(modifier, crewmates2, rpc);
            }
            
            if (MafiaOn)
            {
                //System.Console.WriteLine("MAFIA");
                Godfather.Gen(impostors);
            }

            if (LoversOn)
            {
                //System.Console.WriteLine("LOVER1");
                Lover.Gen(crewmates, impostors);
            }

            if (FlashOn)
            {
                //System.Console.WriteLine("Flash");
                Modifier.Gen(typeof(Flash),
                    PlayerControl.AllPlayerControls.ToArray().Where(x => Modifier.GetModifier(x) == null).ToList(),
                    CustomRPC.SetFlash);
            }



        }
        
    }
}