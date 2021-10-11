using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Reactor;
using TownOfUs.Extensions;
using TownOfUs.CrewmateRoles.AltruistMod;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.CrewmateRoles.SwapperMod;
using TownOfUs.CrewmateRoles.TimeLordMod;
using TownOfUs.CustomOption;
using TownOfUs.ImpostorRoles.AssassinMod;
using TownOfUs.NeutralRoles.PhantomMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using JanitorCoroutines = TownOfUs.ImpostorRoles.JanitorMod.JanitorCoroutines;
using Object = UnityEngine.Object;
using Shift = TownOfUs.NeutralRoles.ShifterMod.Shift;
using Random = UnityEngine.Random; //using Il2CppSystem;
using TownOfUs.NeutralRoles.GlitchMod;

namespace TownOfUs
{
    public static class RpcHandling
    {
        private static readonly List<(Type, CustomRPC, int)> CrewmateRoles = new List<(Type, CustomRPC, int)>();
        private static readonly List<(Type, CustomRPC, int)> NeutralRoles = new List<(Type, CustomRPC, int)>();
        private static readonly List<(Type, CustomRPC, int)> ImpostorRoles = new List<(Type, CustomRPC, int)>();
        private static readonly List<(Type, CustomRPC, int)> CrewmateModifiers = new List<(Type, CustomRPC, int)>();
        private static readonly List<(Type, CustomRPC, int)> GlobalModifiers = new List<(Type, CustomRPC, int)>();
        private static bool LoversOn;
        private static bool PhantomOn;

        internal static bool Check(int probability)
        {
            if (probability == 0) return false;
            if (probability == 100) return true;
            var num = Random.RandomRangeInt(1, 101);
            return num <= probability;
        }

        private static void SortRoles(List<(Type, CustomRPC, int)> roles, int max = int.MaxValue)
        {
            roles.Shuffle();
            roles.Sort((a, b) =>
            {
                var a_ = a.Item3 == 100 ? 0 : 100;
                var b_ = b.Item3 == 100 ? 0 : 100;
                return a_.CompareTo(b_);
            });
            if (roles.Count > max)
                while (roles.Count > max)
                    roles.RemoveAt(roles.Count - 1);
        }

        private static void GenEachRole(List<GameData.PlayerInfo> infected)
        {
            if (TutorialManager.InstanceExists)
            {
                Role.LobbyBehaviour_Start.Postfix();
                var roleType = typeof(Glitch);
                var role = Role.Gen<Role>(
                    roleType, PlayerControl.LocalPlayer, Enum.Parse<CustomRPC>($"Set{roleType.Name}")
                );
                PlayerControl.LocalPlayer.Data.IsImpostor = role.Faction == Faction.Impostors;
                return;
            }
            var impostors = Utils.GetImpostors(infected);
            var crewmates = Utils.GetCrewmates(impostors);
            crewmates.Shuffle();
            impostors.Shuffle();

            SortRoles(CrewmateRoles);
            SortRoles(NeutralRoles, CustomGameOptions.MaxNeutralRoles);
            SortRoles(ImpostorRoles, Math.Min(impostors.Count, CustomGameOptions.MaxImpostorRoles));
            SortRoles(CrewmateModifiers, crewmates.Count);
            SortRoles(GlobalModifiers, crewmates.Count + impostors.Count);

            var crewAndNeutralRoles = new List<(Type, CustomRPC, int)>();
            crewAndNeutralRoles.AddRange(NeutralRoles);
            crewAndNeutralRoles.AddRange(CrewmateRoles);
            SortRoles(crewAndNeutralRoles, crewmates.Count);

            if (Check(CustomGameOptions.VanillaGame))
            {
                CrewmateRoles.Clear();
                NeutralRoles.Clear();
                CrewmateModifiers.Clear();
                GlobalModifiers.Clear();
                ImpostorRoles.Clear();
                LoversOn = false;
                PhantomOn = false;
            }

            PlayerControl executioner = null;

            foreach (var (type, rpc, _) in crewAndNeutralRoles)
            {
                if (rpc == CustomRPC.SetExecutioner)
                {
                    executioner = crewmates[Random.RandomRangeInt(0, crewmates.Count)];
                    crewmates.Remove(executioner);
                    continue;
                }
                    
                Role.Gen<Role>(type, crewmates, rpc);
            }

            if (LoversOn)
                Lover.Gen(crewmates, impostors);

            while (impostors.Count > 0 && ImpostorRoles.Count > 0)
            {
                var (type, rpc, _) = ImpostorRoles.TakeFirst();
                Role.Gen<Role>(type, impostors.TakeFirst(), rpc);
            }

            foreach (var crewmate in crewmates)
                Role.Gen<Role>(typeof(Crewmate), crewmate, CustomRPC.SetCrewmate);

            foreach (var impostor in impostors)
                Role.Gen<Role>(typeof(Impostor), impostor, CustomRPC.SetImpostor);

            if (executioner != null)
            {
                var targets = Utils.GetCrewmates(impostors).Where(
                    crewmate => Role.GetRole(crewmate)?.Faction == Faction.Crewmates
                ).ToList();
                if (targets.Count > 0)
                {
                    var exec = Role.Gen<Executioner>(
                        typeof(Executioner),
                        executioner,
                        CustomRPC.SetExecutioner
                    );
                    var target = exec.Target = targets[Random.RandomRangeInt(0, targets.Count)];

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.SetTarget, SendOption.Reliable, -1);
                    writer.Write(executioner.PlayerId);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
                else
                    Role.Gen<Role>(typeof(Crewmate), executioner, CustomRPC.SetExecutioner);
            }

            var canHaveModifier = PlayerControl.AllPlayerControls.ToArray().ToList();
            canHaveModifier.Shuffle();

            foreach (var (type, rpc, _) in GlobalModifiers)
            {
                if (rpc == CustomRPC.SetButtonBarry)
                {
                    var swapperIdx = canHaveModifier.FindIndex(player => player.Is(RoleEnum.Swapper));
                    if (swapperIdx != -1)
                    {
                        var swapper = canHaveModifier[swapperIdx];
                        canHaveModifier.RemoveAt(swapperIdx);
                        Role.Gen<Modifier>(type, canHaveModifier, rpc);
                        canHaveModifier.Add(swapper);
                        canHaveModifier.Shuffle();
                        continue;
                    }
                }
                Role.Gen<Modifier>(type, canHaveModifier, rpc);
            }

            canHaveModifier.RemoveAll(player => !player.Data.IsImpostor);
            canHaveModifier.Shuffle();

            while (canHaveModifier.Count > 0 && CrewmateModifiers.Count > 0)
            {
                var (type, rpc, _) = CrewmateModifiers.TakeFirst();
                Role.Gen<Modifier>(type, canHaveModifier, rpc);
            }

            if (PhantomOn)
            {
                var vanilla = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(RoleEnum.Crewmate)).ToList();
                var toChooseFrom = crewmates.Count > 0
                    ? crewmates
                    : PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(Faction.Crewmates) && !x.IsLover())
                        .ToList();
                var rand = Random.RandomRangeInt(0, toChooseFrom.Count);
                var pc = toChooseFrom[rand];

                SetPhantom.WillBePhantom = pc;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetPhantom, SendOption.Reliable, -1);
                writer.Write(pc.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            else
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetPhantom, SendOption.Reliable, -1);
                writer.Write(byte.MaxValue);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }


        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class HandleRpc
        {
            public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                byte readByte, readByte1, readByte2;
                sbyte readSByte, readSByte2;
                switch ((CustomRPC) callId)
                {
                    case CustomRPC.SetMayor:
                        readByte = reader.ReadByte();
                        new Mayor(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetJester:
                        readByte = reader.ReadByte();
                        new Jester(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetSheriff:
                        readByte = reader.ReadByte();
                        new Sheriff(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetEngineer:
                        readByte = reader.ReadByte();
                        new Engineer(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetJanitor:
                        new Janitor(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetSwapper:
                        readByte = reader.ReadByte();
                        new Swapper(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetShifter:
                        readByte = reader.ReadByte();
                        new Shifter(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetInvestigator:
                        readByte = reader.ReadByte();
                        new Investigator(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetTimeLord:
                        readByte = reader.ReadByte();
                        new TimeLord(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetTorch:
                        readByte = reader.ReadByte();
                        new Torch(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetDiseased:
                        readByte = reader.ReadByte();
                        new Diseased(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetFlash:
                        readByte = reader.ReadByte();
                        new Flash(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetMedic:
                        readByte = reader.ReadByte();
                        new Medic(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetMorphling:
                        readByte = reader.ReadByte();
                        new Morphling(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.LoveWin:
                        var winnerlover = Utils.PlayerById(reader.ReadByte());
                        Role.GetRole<Lover>(winnerlover).Win();
                        break;

                    case CustomRPC.JesterLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Jester)
                                ((Jester) role).Loses();

                        break;

                    case CustomRPC.PhantomLose:
                        Role.GetRole<Phantom>()?.Loses();
                        break;

                    case CustomRPC.GlitchLose:
                        Role.GetRole<Glitch>()?.Loses();
                        break;

                    case CustomRPC.ShifterLose:
                        Role.GetRole<Shifter>()?.Loses();
                        break;

                    case CustomRPC.ExecutionerLose:
                        Role.GetRole<Executioner>()?.Loses();
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

                        var roleLover1 = new Lover(lover1, 1, b1 == 0);
                        var roleLover2 = new Lover(lover2, 2, b1 == 0);

                        roleLover1.OtherLover = roleLover2;
                        roleLover2.OtherLover = roleLover1;
                        break;

                    case CustomRPC.Start:
                        Utils.ShowDeadBodies = false;
                        Murder.KilledPlayers.Clear();
                        Role.NobodyWins = false;
                        RecordRewind.RewindPoints.Clear();
                        break;

                    case CustomRPC.JanitorClean:
                        readByte1 = reader.ReadByte();
                        var janitorPlayer = Utils.PlayerById(readByte1);
                        var janitorRole = Role.GetRole<Janitor>(janitorPlayer);
                        readByte = reader.ReadByte();
                        var deadBodies = Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in deadBodies)
                            if (body.ParentId == readByte)
                            {
                                Coroutines.Start(JanitorCoroutines.CleanCoroutine(body, janitorRole));
                                break;
                            }
                        break;

                    case CustomRPC.EngineerFix:
                        Role.GetRole<Engineer>().FixCallback();
                        break;

                    case CustomRPC.SetExtraVotes:
                        var mayorRole = Role.GetRole<Mayor>();
                        mayorRole.ExtraVotes = reader.ReadBytesAndSize().ToList();
                        mayorRole.VoteBank -= mayorRole.ExtraVotes.Count;
                        break;

                    case CustomRPC.SetSwaps:
                        readSByte = reader.ReadSByte();
                        SwapVotes.Swap1 = MeetingHud.Instance.playerStates.FirstOrDefault(
                            x => x.TargetPlayerId == readSByte
                        );
                        readSByte2 = reader.ReadSByte();
                        SwapVotes.Swap2 = MeetingHud.Instance.playerStates.FirstOrDefault(
                            x => x.TargetPlayerId == readSByte2
                        );
                        break;

                    case CustomRPC.Shift:
                        Shift.ShiftRoles(
                            Role.GetRole<Shifter>(),
                            Role.GetRole(Utils.PlayerById(reader.ReadByte()))
                        );
                        break;

                    case CustomRPC.Rewind:
                        StartStop.StartRewind();
                        break;

                    case CustomRPC.Protect:
                        readByte2 = reader.ReadByte();

                        var medic = Role.GetRole<Medic>();
                        var shielded = Utils.PlayerById(readByte2);

                        medic.ShieldedPlayer = shielded;
                        medic.UsedAbility = true;
                        Role.NamePatch.UpdateDisplay(shielded);
                        break;

                    case CustomRPC.RewindRevive:
                        readByte = reader.ReadByte();
                        RecordRewind.ReviveBody(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.AttemptSound:
                        StopKill.BreakShield(
                            Role.GetRole<Medic>(),
                            Utils.PlayerById(reader.ReadByte()),
                            CustomGameOptions.ShieldBreaks
                        );
                        break;

                    case CustomRPC.SetGlitch:
                        var GlitchId = reader.ReadByte();
                        var GlitchPlayer = Utils.PlayerById(GlitchId);
                        new Glitch(GlitchPlayer);
                        break;

                    case CustomRPC.BypassKill:
                        var killer = Utils.PlayerById(reader.ReadByte());
                        var target = Utils.PlayerById(reader.ReadByte());

                        Utils.MurderPlayer(killer, target);
                        break;

                    case CustomRPC.AssassinKill:
                        var toDie = Utils.PlayerById(reader.ReadByte());
                        AssassinKill.MurderPlayer(toDie);
                        break;

                    case CustomRPC.SetMimic:
                        var glitchPlayer = Utils.PlayerById(reader.ReadByte());
                        var mimicPlayer = Utils.PlayerById(reader.ReadByte());
                        var glitchRole = Role.GetRole<Glitch>(glitchPlayer);
                        glitchRole.MimicedAs = mimicPlayer;
                        Utils.Morph(glitchPlayer, mimicPlayer);
                        break;

                    case CustomRPC.ResetAnim:
                        var animPlayer = Utils.PlayerById(reader.ReadByte());
                        var animRole = Role.GetRole(animPlayer);
                        switch (animRole?.RoleType) {
                            case RoleEnum.Morphling:
                                ((Morphling)animRole).MorphedPlayer = null;
                                break;
                            case RoleEnum.Glitch:
                                ((Glitch)animRole).MimicedAs = null;
                                break;
                        }

                        Utils.Unmorph(animPlayer);
                        break;

                    case CustomRPC.GlitchWin:
                        Role.GetRole<Glitch>()?.Wins();
                        break;

                    case CustomRPC.SetHacked:
                        var hackPlayer = Utils.PlayerById(reader.ReadByte());
                        if (hackPlayer.AmOwner)
                            Coroutines.Start(GlitchCoroutines.Hack(Role.GetRole<Glitch>(), hackPlayer));
                        break;

                    case CustomRPC.Investigate:
                        var otherPlayer = Utils.PlayerById(reader.ReadByte());
                        var seer = Role.GetRole<Seer>();
                        seer.Investigated.Add(otherPlayer.PlayerId);
                        Role.NamePatch.UpdateSingle(seer.Player);
                        break;

                    case CustomRPC.SetSeer:
                        new Seer(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.Morph:
                        var morphTarget = Utils.PlayerById(reader.ReadByte());
                        var morphRole = Role.GetRole<Morphling>();
                        morphRole.SampledPlayer = morphRole.MorphedPlayer = morphTarget;
                        morphRole.Morph();
                        break;

                    case CustomRPC.SetExecutioner:
                        new Executioner(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetTarget:
                        var executioner = Utils.PlayerById(reader.ReadByte());
                        var exeTarget = Utils.PlayerById(reader.ReadByte());
                        var exeRole = Role.GetRole<Executioner>(executioner);
                        exeRole.Target = exeTarget;
                        break;

                    case CustomRPC.SetCamouflager:
                        new Camouflager(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.Camouflage:
                        var camouflager1 = Utils.PlayerById(reader.ReadByte());
                        var camouflagerRole1 = Role.GetRole<Camouflager>(camouflager1);
                        camouflagerRole1.Enabled = true;
                        Utils.Camouflage();
                        break;

                    case CustomRPC.UnCamouflage:
                        var camouflager2 = Utils.PlayerById(reader.ReadByte());
                        var camouflagerRole2 = Role.GetRole<Camouflager>(camouflager2);
                        camouflagerRole2.Enabled = false;
                        Utils.UnCamouflage();
                        break;

                    case CustomRPC.SetSpy:
                        new Spy(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetSnitch:
                        new Snitch(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetMiner:
                        new Miner(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.Mine:
                        var ventId = reader.ReadInt32();
                        var position = reader.ReadVector3();
                        var roomType = reader.ReadByte();
                        var room = roomType == byte.MaxValue
                            ? null
                            : ShipStatus.Instance.FastRooms[(SystemTypes)roomType];
                        Role.GetRole<Miner>().SpawnVent(position, ventId, room);
                        break;

                    case CustomRPC.SetSwooper:
                        new Swooper(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.Swoop:
                        Role.GetRole<Swooper>().SwoopCallback();
                        break;

                    case CustomRPC.UnSwoop:
                        Role.GetRole<Swooper>().UnSwoop();
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
                        var arsonistRole = Role.GetRole<Arsonist>(arsonist);
                        arsonistRole.DousedPlayers.Add(douseTarget.PlayerId);
                        break;

                    case CustomRPC.Ignite:
                        var theArsonistRole = Role.GetRole<Arsonist>();
                        theArsonistRole.IgniteCallback();
                        break;

                    case CustomRPC.ArsonistWin:
                        Role.GetRole<Arsonist>()?.Wins();
                        break;

                    case CustomRPC.ArsonistLose:
                        Role.GetRole<Arsonist>()?.Loses();
                        break;

                    case CustomRPC.SetImpostor:
                        new Impostor(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetCrewmate:
                        new Crewmate(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SyncCustomSettings:
                        Rpc.ReceiveRpc(reader);
                        break;

                    case CustomRPC.SetAltruist:
                        new Altruist(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetBigBoi:
                        new BigBoiModifier(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.AltruistRevive:
                        readByte1 = reader.ReadByte();
                        var altruistPlayer = Utils.PlayerById(readByte1);
                        var altruistRole = Role.GetRole<Altruist>(altruistPlayer);
                        readByte = reader.ReadByte();
                        var theDeadBodies = Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in theDeadBodies)
                            if (body.ParentId == readByte)
                            {
                                if (body.ParentId == PlayerControl.LocalPlayer.PlayerId)
                                    Coroutines.Start(Utils.FlashCoroutine(
                                        altruistRole.Color,
                                        CustomGameOptions.ReviveDuration,
                                        0.5f
                                    ));

                                Coroutines.Start(AltruistCoroutine.AltruistRevive(body, altruistRole));
                                break;
                            }

                        break;
                    case CustomRPC.FixAnimation:
                        var player = Utils.PlayerById(reader.ReadByte());
                        player.MyPhysics.ResetMoveState();
                        player.Collider.enabled = true;
                        player.moveable = true;
                        player.NetTransform.enabled = true;
                        break;

                    case CustomRPC.SetButtonBarry:
                        new ButtonBarry(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.BarryButton:
                        var buttonBarry = Utils.PlayerById(reader.ReadByte());
                        if (AmongUsClient.Instance.AmHost)
                        {
                            MeetingRoomManager.Instance.reporter = buttonBarry;
                            MeetingRoomManager.Instance.target = null;
                            AmongUsClient.Instance.DisconnectHandlers.AddUnique(MeetingRoomManager.Instance
                                .Cast<IDisconnectHandler>());
                            if (ShipStatus.Instance.CheckTaskCompletion()) return;

                            DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(buttonBarry);
                            buttonBarry.RpcStartMeeting(null);
                        }
                        break;

                    case CustomRPC.SetUndertaker:
                        new Undertaker(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.Drag:
                        var dienerRole = Role.GetRole<Undertaker>();
                        readByte = reader.ReadByte();
                        var dienerBodies = Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in dienerBodies)
                            if (body.ParentId == readByte)
                            {
                                dienerRole.CurrentlyDragging = body;
                                break;
                            }
                        break;

                    case CustomRPC.Drop:
                        var dienerRole2 = Role.GetRole<Undertaker>();
                        dienerRole2.CurrentlyDragging = null;
                        break;

                    case CustomRPC.SetAssassin:
                        new Assassin(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetUnderdog:
                        new Underdog(Utils.PlayerById(reader.ReadByte()));
                        break;

                    case CustomRPC.SetPhantom:
                        readByte = reader.ReadByte();
                        SetPhantom.WillBePhantom = readByte == byte.MaxValue ? null : Utils.PlayerById(readByte);
                        break;

                    case CustomRPC.PhantomDied:
                        var phantom = SetPhantom.WillBePhantom;
                        Role.RoleDictionary.Remove(phantom.PlayerId);
                        var phantomRole = new Phantom(phantom);
                        phantomRole.RegenTask();
                        phantom.gameObject.layer = LayerMask.NameToLayer("Players");
                        SetPhantom.RemoveTasks(phantom);
                        SetPhantom.AddCollider(phantomRole);
                        PlayerControl.LocalPlayer.MyPhysics.ResetMoveState();
                        break;

                    case CustomRPC.CatchPhantom:
                        var phantomPlayer = Utils.PlayerById(reader.ReadByte());
                        Role.GetRole<Phantom>(phantomPlayer).Caught = true;
                        break;

                    case CustomRPC.PhantomWin:
                        Role.GetRole<Phantom>(Utils.PlayerById(reader.ReadByte())).CompletedTasks = true;
                        break;
                    
                    case CustomRPC.AddMayorVoteBank:
                        Role.GetRole<Mayor>(Utils.PlayerById(reader.ReadByte())).VoteBank += reader.ReadInt32();
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
        public static class RpcSetInfected
        {
            public static void Prefix([HarmonyArgument(0)] ref Il2CppReferenceArray<GameData.PlayerInfo> infected)
            {
                Utils.ShowDeadBodies = false;
                Role.NobodyWins = false;
                CrewmateRoles.Clear();
                NeutralRoles.Clear();
                ImpostorRoles.Clear();
                CrewmateModifiers.Clear();
                GlobalModifiers.Clear();

                RecordRewind.RewindPoints.Clear();
                Murder.KilledPlayers.Clear();

                var startWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.Start, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(startWriter);

                LoversOn = Check(CustomGameOptions.LoversOn);
                PhantomOn = Check(CustomGameOptions.PhantomOn);

                #region Crewmate Roles
                if (Check(CustomGameOptions.MayorOn))
                    CrewmateRoles.Add((typeof(Mayor), CustomRPC.SetMayor, CustomGameOptions.MayorOn));

                if (Check(CustomGameOptions.SheriffOn))
                    CrewmateRoles.Add((typeof(Sheriff), CustomRPC.SetSheriff, CustomGameOptions.SheriffOn));

                if (Check(CustomGameOptions.EngineerOn))
                    CrewmateRoles.Add((typeof(Engineer), CustomRPC.SetEngineer, CustomGameOptions.EngineerOn));

                if (Check(CustomGameOptions.SwapperOn))
                    CrewmateRoles.Add((typeof(Swapper), CustomRPC.SetSwapper, CustomGameOptions.SwapperOn));

                if (Check(CustomGameOptions.InvestigatorOn))
                    CrewmateRoles.Add((typeof(Investigator), CustomRPC.SetInvestigator, CustomGameOptions.InvestigatorOn));

                if (Check(CustomGameOptions.TimeLordOn))
                    CrewmateRoles.Add((typeof(TimeLord), CustomRPC.SetTimeLord, CustomGameOptions.TimeLordOn));

                if (Check(CustomGameOptions.MedicOn))
                    CrewmateRoles.Add((typeof(Medic), CustomRPC.SetMedic, CustomGameOptions.MedicOn));

                if (Check(CustomGameOptions.SeerOn))
                    CrewmateRoles.Add((typeof(Seer), CustomRPC.SetSeer, CustomGameOptions.SeerOn));

                if (Check(CustomGameOptions.SpyOn))
                    CrewmateRoles.Add((typeof(Spy), CustomRPC.SetSpy, CustomGameOptions.SpyOn));

                if (Check(CustomGameOptions.SnitchOn))
                    CrewmateRoles.Add((typeof(Snitch), CustomRPC.SetSnitch, CustomGameOptions.SnitchOn));

                if (Check(CustomGameOptions.AltruistOn))
                    CrewmateRoles.Add((typeof(Altruist), CustomRPC.SetAltruist, CustomGameOptions.AltruistOn));

                if (Check(CustomGameOptions.ArsonistOn))
                    NeutralRoles.Add((typeof(Arsonist), CustomRPC.SetArsonist, CustomGameOptions.ArsonistOn));

                if (Check(CustomGameOptions.ExecutionerOn))
                    NeutralRoles.Add((typeof(Executioner), CustomRPC.SetExecutioner, CustomGameOptions.ExecutionerOn));
                #endregion
                #region Neutral Roles
                if (Check(CustomGameOptions.JesterOn))
                    NeutralRoles.Add((typeof(Jester), CustomRPC.SetJester, CustomGameOptions.JesterOn));

                if (Check(CustomGameOptions.ShifterOn))
                    NeutralRoles.Add((typeof(Shifter), CustomRPC.SetShifter, CustomGameOptions.ShifterOn));

                if (Check(CustomGameOptions.GlitchOn))
                    NeutralRoles.Add((typeof(Glitch), CustomRPC.SetGlitch, CustomGameOptions.GlitchOn));
                #endregion
                #region Impostor Roles
                if (Check(CustomGameOptions.UndertakerOn))
                    ImpostorRoles.Add((typeof(Undertaker), CustomRPC.SetUndertaker, CustomGameOptions.UndertakerOn));

                if (Check(CustomGameOptions.AssassinOn))
                    ImpostorRoles.Add((typeof(Assassin), CustomRPC.SetAssassin, CustomGameOptions.AssassinOn));

                if (Check(CustomGameOptions.UnderdogOn))
                    ImpostorRoles.Add((typeof(Underdog), CustomRPC.SetUnderdog, CustomGameOptions.UnderdogOn));

                if (Check(CustomGameOptions.MorphlingOn))
                    ImpostorRoles.Add((typeof(Morphling), CustomRPC.SetMorphling, CustomGameOptions.MorphlingOn));

                if (Check(CustomGameOptions.CamouflagerOn))
                    ImpostorRoles.Add((typeof(Camouflager), CustomRPC.SetCamouflager, CustomGameOptions.CamouflagerOn));

                if (Check(CustomGameOptions.MinerOn))
                    ImpostorRoles.Add((typeof(Miner), CustomRPC.SetMiner, CustomGameOptions.MinerOn));

                if (Check(CustomGameOptions.SwooperOn))
                    ImpostorRoles.Add((typeof(Swooper), CustomRPC.SetSwooper, CustomGameOptions.SwooperOn));

                if (Check(CustomGameOptions.JanitorOn))
                    ImpostorRoles.Add((typeof(Janitor), CustomRPC.SetJanitor, CustomGameOptions.JanitorOn));
                #endregion
                #region Crewmate Modifiers
                if (Check(CustomGameOptions.TorchOn))
                    CrewmateModifiers.Add((typeof(Torch), CustomRPC.SetTorch, CustomGameOptions.TorchOn));

                if (Check(CustomGameOptions.DiseasedOn))
                    CrewmateModifiers.Add((typeof(Diseased), CustomRPC.SetDiseased, CustomGameOptions.DiseasedOn));
                #endregion
                #region Global Modifiers
                if (Check(CustomGameOptions.TiebreakerOn))
                    GlobalModifiers.Add((typeof(Tiebreaker), CustomRPC.SetTiebreaker, CustomGameOptions.TiebreakerOn));

                if (Check(CustomGameOptions.FlashOn))
                    GlobalModifiers.Add((typeof(Flash), CustomRPC.SetFlash, CustomGameOptions.FlashOn));

                if (Check(CustomGameOptions.DrunkOn))
                    GlobalModifiers.Add((typeof(Drunk), CustomRPC.SetDrunk, CustomGameOptions.DrunkOn));

                if (Check(CustomGameOptions.BigBoiOn))
                    GlobalModifiers.Add((typeof(BigBoiModifier), CustomRPC.SetBigBoi, CustomGameOptions.BigBoiOn));

                if (Check(CustomGameOptions.ButtonBarryOn))
                    GlobalModifiers.Add(
                        (typeof(ButtonBarry), CustomRPC.SetButtonBarry, CustomGameOptions.ButtonBarryOn));
                #endregion
                GenEachRole(infected.ToList());
            }
        }
    }
}
