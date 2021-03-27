using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace TownOfUs.Roles
{
    public abstract class Role
    {
        private bool Equals(Role other)
        {
            return Equals(Player, other.Player) && RoleType == other.RoleType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Role)) return false;
            return Equals((Role) obj);
        }
        

        public override int GetHashCode()
        {
            return HashCode.Combine(Player, (int) RoleType);
        }

        public static readonly Dictionary<byte, Role> RoleDictionary = new Dictionary<byte, Role>();
        public static IEnumerable<Role> AllRoles => RoleDictionary.Values.ToList();
        protected internal string Name { get; set; }

        private PlayerControl _player { get; set; }

        public PlayerControl Player
        {
            get => _player;
            set
            {
                if (_player != null)
                {
                    _player.nameText.Color = Color.white;
                }

                _player = value;
            }
        }
        protected Func<string> ImpostorText;
        protected Func<string> TaskText;
        protected float Scale { get; set; } = 1f;
        protected internal Color Color { get; set; }
        protected internal RoleEnum RoleType { get; set; }
        //public static Faction Faction;
        protected internal Faction Faction { get; set; } = Faction.Crewmates;

        protected internal Color FactionColor
        {
            get
            {
                return Faction switch
                {
                    Faction.Crewmates => Color.green,
                    Faction.Impostors => Color.red,
                    Faction.Neutral => CustomGameOptions.NeutralRed ? Color.red : Color.grey,
                    _ => Color.white
                };
            }
        }

        public static bool NobodyWins = false;
        
        public static uint NetId => PlayerControl.LocalPlayer.NetId;
        public string PlayerName;

        public string ColorString => "[" + Color.ToHtmlStringRGBA() + "]";

        
        //public static T Gen<T>()

        protected virtual bool Criteria()
        {
            return GetRole(PlayerControl.LocalPlayer) == this;
        }

        protected virtual void IntroPrefix(IntroCutscene.CoBegin__d __instance)
        {
        }

        public static void NobodyWinsFunc()
        {

            Role.NobodyWins = true;
        }

        protected static bool NobodyEndCriteria(ShipStatus __instance)
        {
            bool CheckNoImpsNoCrews()
            {
                var alives = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead).ToList();
                
                var flag = alives.All(x =>
                {
                    var role = GetRole(x);
                    if (role == null) return false;
                    var flag = role.Faction == Faction.Neutral && !x.Is(RoleEnum.Glitch);
                    System.Console.WriteLine("CheckFlag - " + flag + " - " + x.name);
                    return flag;
                });

                return flag;
            }
            
            if (CheckNoImpsNoCrews())
            {
                System.Console.WriteLine("NO IMPS NO CREWS");
                var messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.NobodyWins, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                
                NobodyWinsFunc();
                ShipStatus.RpcEndGame((GameOverReason)2, false);
                return false;
            }

            return true;
        }
        
        protected virtual bool CheckEndCriteria(ShipStatus __instance)
        {
            

            return true;


        }

        protected internal virtual string NameText()
        {
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed & !MeetingHud.Instance)
            {
                return "";
            }
            return Player.name;
        }

        public static bool operator ==(Role a, Role b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.RoleType == b.RoleType && a.Player.PlayerId == b.Player.PlayerId;
        }

        public static bool operator !=(Role a, Role b)
        {
            return !(a == b);
        }

        public void RegenTask()
        {
            Player.myTasks[0].Cast<ImportantTextTask>().Text = $"{ColorString}Role: {Name}\n{TaskText()}[]";
        }
        
        public static Role Gen(Type T, List<PlayerControl> crewmates, CustomRPC rpc)
        {
            
            if (crewmates.Count <= 0) return null;
            var rand = HashRandom.Method_1(crewmates.Count); //TODO - change
            var pc = crewmates[rand];

            var role = Activator.CreateInstance(T, new object[]{pc});
            var playerId = pc.PlayerId;
            crewmates.Remove(pc);
    
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) rpc,
                SendOption.Reliable, -1);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return role as Role;
        }

        protected Role(PlayerControl player)
        {
            Player = player;
            PlayerName = player.Data.PlayerName;
            RoleDictionary.Add(player.PlayerId, this);
        }
        
        
        public static Role GetRole(PlayerControl player)
        {
            return (from entry in RoleDictionary where entry.Key == player.PlayerId select entry.Value)
                .FirstOrDefault();
        }

        public static T GetRole<T>(PlayerControl player) where T : Role
        {
            return GetRole(player) as T;
        }

        public static Role GetRole(PlayerVoteArea area)
        {
            var player = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(x => x.PlayerId == area.TargetPlayerId);
            return player == null ? null : GetRole(player);
        }

        public static IEnumerable<Role> GetRoles(RoleEnum roletype)
        {
            return AllRoles.Where(x => x.RoleType == roletype);
        }
        
        public static class IntroCutScenePatch
        {
            public static TextRenderer ModifierText;

            public static float Scale; 
            [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
            public static class IntroCutscene_BeginCrewmate
            {
                public static void Postfix(IntroCutscene __instance)
                {
                    //System.Console.WriteLine("REACHED HERE - CREW");
                    var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                    if (modifier != null)
                    {
                        ModifierText = Object.Instantiate(__instance.Title, __instance.Title.transform.parent, false);
                        //System.Console.WriteLine("MODIFIER TEXT PLEASE WORK");
                        Scale = ModifierText.scale;
                    }
                    else
                    {
                        ModifierText = null;
                    }
                }
            }
            
            [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
            public static class IntroCutscene_BeginImpostor
            {
                public static void Postfix(IntroCutscene __instance)
                {
                    //System.Console.WriteLine("REACHED HERE - IMP");
                    var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                    if (modifier != null)
                    {
                        ModifierText = Object.Instantiate(__instance.Title, __instance.Title.transform.parent, false);
                        //System.Console.WriteLine("MODIFIER TEXT PLEASE WORK");
                        Scale = ModifierText.scale;
                    }
                    else
                    {
                        ModifierText = null;
                    }
                }
            }


            [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
            public static class IntroCutscene_CoBegin__d_MoveNext
            {
                public static float TestScale;
                public static void Prefix(IntroCutscene.CoBegin__d __instance)
                {
                    var role = GetRole(PlayerControl.LocalPlayer);
                    
                    if (role != null)
                    {
                        role.IntroPrefix(__instance);
                        ;
                    }

                    
                }


                public static void Postfix(IntroCutscene.CoBegin__d __instance)
                {
                    var role = GetRole(PlayerControl.LocalPlayer);
                    var alpha = __instance.__this.Title.Color.a;
                    if (role != null)
                    {
                       
                        __instance.__this.Title.Text = role.Name;
                        __instance.__this.Title.Color = role.Color;
                        __instance.__this.ImpostorText.Text = role.ImpostorText();
                        __instance.__this.ImpostorText.gameObject.SetActive(true);
                        __instance.__this.BackgroundBar.material.color = role.Color;
                        TestScale = Mathf.Max(__instance.__this.Title.scale, TestScale);
                        __instance.__this.Title.scale = TestScale / role.Scale;

                    }

                    if (ModifierText != null)
                    {
                        var modifier = Roles.Modifier.GetModifier(PlayerControl.LocalPlayer);
                        ModifierText.Text = "Modifier: " + modifier.Name;
                        ModifierText.Color = modifier.Color;
                        ModifierText.scale = Scale / 3f;
                        ModifierText.transform.position=
                            __instance.__this.transform.position - new Vector3(0f, 2.1f, 0f);
                        ModifierText.gameObject.SetActive(true);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
        public static class PlayerControl_SetTasks
        {
            public static void Postfix(PlayerControl __instance)
            {
                if (__instance == null) return;
                var role = GetRole(__instance);
                var modifier = Modifier.GetModifier(__instance);
                

                if (modifier != null)
                {
                    var modTask = new GameObject(modifier.Name + "Task").AddComponent<ImportantTextTask>();
                    modTask.transform.SetParent(__instance.transform, false);
                    modTask.Text =
                        $"{modifier.ColorString}Modifier: {modifier.Name}\n{modifier.TaskText()}[]";
                    __instance.myTasks.Insert(0, modTask);
                }                
                if (role == null) return;
                if (role.RoleType == RoleEnum.Shifter && role.Player != PlayerControl.LocalPlayer) return;
                var task = new GameObject(role.Name + "Task").AddComponent<ImportantTextTask>();
                task.transform.SetParent(__instance.transform, false);
                task.Text = $"{role.ColorString}Role: {role.Name}\n{role.TaskText()}[]";
                __instance.myTasks.Insert(0, task);

            }
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
        public static class ShipStatus_CheckEndCriteria
        {
            public static bool Prefix(ShipStatus __instance)
            {
                //System.Console.WriteLine("CHECKENDCRITERIA");
                if (!AmongUsClient.Instance.AmHost) return false;
                if (__instance.Systems.ContainsKey(SystemTypes.LifeSupp))
                {
                    var lifeSuppSystemType = __instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (lifeSuppSystemType.Countdown < 0f)
                    {
                        return true;
                    }
                }
                
                if (__instance.Systems.ContainsKey(SystemTypes.Laboratory))
                {
                    var reactorSystemType = __instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    if (reactorSystemType.Countdown < 0f)
                    {
                        return true;
                    }
                }
                if (__instance.Systems.ContainsKey(SystemTypes.Reactor))
                {
                    var reactorSystemType = __instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    if (reactorSystemType.Countdown < 0f)
                    {
                        return true;
                    }
                }

                if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
                {
                    return true;
                }

                bool result = true;
                foreach (var role in AllRoles)
                {
                    //System.Console.WriteLine(role.Name);
                    var isend = role.CheckEndCriteria(__instance);
                    //System.Console.WriteLine(isend);
                    if (!isend)
                    {
                        result = false;
                    }
                }

                if (!NobodyEndCriteria(__instance))
                {
                    result = false;
                }

                return result;
                //return true;
            }
        }

        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        public static class LobbyBehaviour_Start
        {
            private static void Postfix(LobbyBehaviour __instance)
            {
                RoleDictionary.Clear();
                Modifier.ModifierDictionary.Clear();
            }
        }

        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
            new[] {typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>)})]
        public static class TranslationController_GetString
        {
            public static void Postfix(ref string __result, [HarmonyArgument(0)] StringNames name)
            {
                if (ExileController.Instance == null || ExileController.Instance.exiled == null) return;

                switch (name)
                {
                    case StringNames.ExileTextPN:
                    case StringNames.ExileTextSN:
                    case StringNames.ExileTextPP:
                    case StringNames.ExileTextSP:
                    {
                        var info = ExileController.Instance.exiled;
                        var role = Role.GetRole(info.Object);
                        if (role == null) return;
                        var roleName = role.RoleType == RoleEnum.Glitch ? role.Name : $"The {role.Name}";
                        __result = $"{info.PlayerName} was {roleName}.";
                        return;
                    }
                }
            }
        }
        
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class HudManager_Update
        {
            private static void UpdateMeeting(MeetingHud __instance)
            {
                foreach (var player in __instance.playerStates)
                {
                    var role = GetRole(player);
                    if (role == null) continue;
                    if (role.Criteria())
                    {
                        player.NameText.Color = role.Color;
                        player.NameText.Text = role.NameText();
                    }
                }
            }

            
            private static void Postfix(HudManager __instance)
            {
                if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance);

                if (PlayerControl.AllPlayerControls.Count <= 1) return;
                if (PlayerControl.LocalPlayer == null) return;
                if (PlayerControl.LocalPlayer.Data == null) return;
                
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    
                    var role = GetRole(player);
                    if (role != null)
                    {
                        if (role.Criteria())
                        {
                            player.nameText.Color = role.Color;
                            player.nameText.Text = role.NameText();
                            continue;
                        }
                    }

                    if (PlayerControl.LocalPlayer.Data.IsImpostor && player.Data.IsImpostor) continue;
                    if (player.IsCrewmate()) player.nameText.Color = Color.white;

                }

            }
        }
    } 
    
}