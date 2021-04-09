using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Reactor.Extensions;
using TownOfUs.Roles.Modifiers;
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
                PlayerName = value.Data.PlayerName;
            }
        }

        protected Func<string> ImpostorText;
        protected Func<string> TaskText;
        protected float Scale { get; set; } = 1f;
        protected internal Color Color { get; set; }
        protected internal RoleEnum RoleType { get; set; }

        protected internal bool Hidden { get; set; } = false;

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

        public List<KillButtonManager> ExtraButtons = new List<KillButtonManager>();
        public static uint NetId => PlayerControl.LocalPlayer.NetId;
        public string PlayerName { get; set; } 

        public string ColorString => "[" + Color.ToHtmlStringRGBA() + "]";


        //public static T Gen<T>()

        protected virtual bool Criteria()
        {
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.HatId == 0U ? 0.7f :
                CustomHats.HatCreation.TallIds.Contains(Player.Data.HatId) ? 1.2f : 1.05f,
                -0.5f
            );
            if (PlayerControl.LocalPlayer.Data.IsDead && CustomGameOptions.DeadSeeRoles) return Utils.ShowDeadBodies;
            if (Faction == Faction.Impostors && PlayerControl.LocalPlayer.Data.IsImpostor && CustomGameOptions.ImpostorSeeRoles) return true;
            return GetRole(PlayerControl.LocalPlayer) == this;
        }

        protected virtual void IntroPrefix(IntroCutscene._CoBegin_d__11 __instance)
        {
        }

        public static void NobodyWinsFunc()
        {

            Role.NobodyWins = true;
        }

        internal static bool NobodyEndCriteria(ShipStatus __instance)
        {
            bool CheckNoImpsNoCrews()
            {
                var alives = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead && !x.Data.Disconnected).ToList();
                if (alives.Count == 0) return false;
                var flag = alives.All(x =>
                {
                    var role = GetRole(x);
                    if (role == null) return false;
                    var flag2 = role.Faction == Faction.Neutral && !x.Is(RoleEnum.Glitch) && !x.Is(RoleEnum.Arsonist);
                    var flag3 = x.Is(RoleEnum.Arsonist) && ((Arsonist) role).IgniteUsed && alives.Count > 1;
                    
                    return flag2 || flag3;
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
                Utils.EndGame();
                return false;
            }

            return true;
        }

        internal virtual bool CheckEndCriteria(ShipStatus __instance)
        {


            return true;


        }

        protected virtual string NameText(PlayerVoteArea player = null)
        {
            
            if (CamouflageMod.CamouflageUnCamouflage.IsCamoed && player == null)
            {
                return "";
            }

            if (Player == null) return "";

            if (player != null && (MeetingHud.Instance.state == MeetingHud.VoteStates.Proceeding ||
                MeetingHud.Instance.state == MeetingHud.VoteStates.Results)) return Player.name;

            if (!CustomGameOptions.RoleUnderName && player == null) return Player.name;
            
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                (Player.Data.HatId == 0U) ? 1.05f :
                CustomHats.HatCreation.TallIds.Contains(Player.Data.HatId) ? 1.6f : 1.4f,
                -0.5f
            );
            return Player.name + "\n" + Name;
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
            bool createTask;
            try
            {
                var firstText = Player.myTasks.ToArray()[0].Cast<ImportantTextTask>();
                createTask = !firstText.Text.Contains(Name);
            }
            catch (InvalidCastException)
            {
                createTask = true;
            }

            if (createTask)
            {
                var task = new GameObject(Name + "Task").AddComponent<ImportantTextTask>();
                task.transform.SetParent(Player.transform, false);
                task.Text = $"{ColorString}Role: {Name}\n{TaskText()}[]";
                Player.myTasks.Insert(0, task);
                return;
            }

            Player.myTasks.ToArray()[0].Cast<ImportantTextTask>().Text = $"{ColorString}Role: {Name}\n{TaskText()}[]";
        }

        public static Role Gen(Type T, List<PlayerControl> crewmates, CustomRPC rpc)
        {

            if (crewmates.Count <= 0) return null;
            var rand = UnityEngine.Random.RandomRangeInt(0, crewmates.Count); //TODO - change
            var pc = crewmates[rand];

            var role = Activator.CreateInstance(T, new object[] {pc});
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

            [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.HMLCOCAJBGM))]
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

            [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.DLGJDGFGAEA))]
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


            [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
            public static class IntroCutscene_CoBegin__d_MoveNext
            {
                public static float TestScale;

                public static void Prefix(IntroCutscene._CoBegin_d__11 __instance)
                {
                    var role = GetRole(PlayerControl.LocalPlayer);

                    if (role != null)
                    {
                        role.IntroPrefix(__instance);
                        ;
                    }


                }


                public static void Postfix(IntroCutscene._CoBegin_d__11 __instance)
                {
                    var role = GetRole(PlayerControl.LocalPlayer);
                    var alpha = __instance.__this.Title.Color.a;
                    if (role != null && !role.Hidden)
                    {

                        __instance.__this.Title.Text = role.Name;
                        __instance.__this.Title.Color = role.Color;
                        __instance.__this.ImpostorText.Text = role.ImpostorText();
                        __instance.__this.ImpostorText.gameObject.SetActive(true);
                        __instance.__this.BackgroundBar.material.color = role.Color;
                        TestScale = Mathf.Max(__instance.__this.Title.scale, TestScale);
                        __instance.__this.Title.scale = TestScale / role.Scale;

                    }
                    /*else if (!__instance.isImpostor)
                    {
                        __instance.__this.ImpostorText.Text = "Haha imagine being a boring old crewmate";
                    }*/

                    if (ModifierText != null)
                    {
                        var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                        ModifierText.Text = "Modifier: " + modifier.Name;
                        ModifierText.Color = modifier.Color;
                        ModifierText.scale = Scale / 3f;
                        ModifierText.transform.position =
                            __instance.__this.transform.position - new Vector3(0f, 2.1f, 0f);
                        ModifierText.gameObject.SetActive(true);
                    }
                }
            }
        }

            

        [HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__78), nameof(PlayerControl._CoSetTasks_d__78.MoveNext))]
        public static class PlayerControl_SetTasks
        {
            public static void Postfix(PlayerControl._CoSetTasks_d__78 __instance)
            {
                if (__instance == null) return;
                var player = __instance.__this;
                var role = GetRole(player);
                var modifier = Modifier.GetModifier(player);


                if (modifier != null)
                {
                    var modTask = new GameObject(modifier.Name + "Task").AddComponent<ImportantTextTask>();
                    modTask.transform.SetParent(player.transform, false);
                    modTask.Text =
                        $"{modifier.ColorString}Modifier: {modifier.Name}\n{modifier.TaskText()}[]";
                    player.myTasks.Insert(0, modTask);
                }

                if (role == null || role.Hidden) return;
                if (role.RoleType == RoleEnum.Shifter && role.Player != PlayerControl.LocalPlayer) return;
                var task = new GameObject(role.Name + "Task").AddComponent<ImportantTextTask>();
                task.transform.SetParent(player.transform, false);
                task.Text = $"{role.ColorString}Role: {role.Name}\n{role.TaskText()}[]";
                player.myTasks.Insert(0, task);

            }
        }

        /*[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static class ButtonsFix
        {
            public static void Postfix(PlayerControl __instance)
            {
                if (__instance != PlayerControl.LocalPlayer) return;

                var role = GetRole(PlayerControl.LocalPlayer);
                if (role == null) return;
                var instance = DestroyableSingleton<HudManager>.Instance;
                var position = instance.KillButton.transform.position;
                foreach (var button in role.ExtraButtons)
                {
                    button.transform.position = new Vector3(position.x,
                        instance.ReportButton.transform.position.y, position.z);
                }

            }
        }*/
        
        
        
        
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
                    var reactorSystemType = __instance.Systems[SystemTypes.Reactor].Cast<ICriticalSabotage>();
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
                foreach (var role in AllRoles.Where(x => x.RoleType == RoleEnum.Snitch))
                {
                    ((Snitch)role).ImpArrows.DestroyAll();
                    ((Snitch)role).SnitchArrows.DestroyAll();
                }


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
                    if (role != null && role.Criteria())
                    {
                        player.NameText.Color = role.Color;
                        player.NameText.Text = role.NameText(player);
                    }
                    else
                    {
                        try
                        {
                            player.NameText.Text = role.Player.name;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            
            [HarmonyPriority(Priority.First)]
            private static void Postfix(HudManager __instance)
            {
                if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance);

                if (PlayerControl.AllPlayerControls.Count <= 1) return;
                if (PlayerControl.LocalPlayer == null) return;
                if (PlayerControl.LocalPlayer.Data == null) return;

                foreach (var player in PlayerControl.AllPlayerControls)
                {

                    if (!(player.Data.IsImpostor && PlayerControl.LocalPlayer.Data.IsImpostor))
                    {
                        player.nameText.Text = player.name;
                        player.nameText.Color = Color.white;
                    }

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

                }

            }
        }
    }
}