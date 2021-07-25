using System;
using HarmonyLib;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.NeutralRoles.PhantomMod
{
    #region OpenDoorConsole
    [HarmonyPatch(typeof(OpenDoorConsole), nameof(OpenDoorConsole.CanUse))]
    public class OpenDoorConsoleCanUse
    {
        public static void Prefix(OpenDoorConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            ref bool __state)
        {
            __state = false;
            
            var playerControl = playerInfo.Object;
            if (playerControl.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(playerControl).Caught && playerInfo.IsDead)
            {
                playerInfo.IsDead = false;
                __state = true;
            }
        }

        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref bool __state)
        {
            if (__state)
                playerInfo.IsDead = true;
        }
    }
    
    [HarmonyPatch(typeof(OpenDoorConsole), nameof(OpenDoorConsole.Use))]
    public class OpenDoorConsoleUse
    {
        public static bool Prefix(OpenDoorConsole __instance)
        {
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out _);
            if (!canUse) return false;
            __instance.MyDoor.SetDoorway(true);
            return false;
        }
    }
    #endregion
    
    #region DoorConsole
    [HarmonyPatch(typeof(DoorConsole), nameof(DoorConsole.CanUse))]
    public class DoorConsoleCanUse
    {
        public static void Prefix(DoorConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            ref bool __state)
        {
            __state = false;
            
            var playerControl = playerInfo.Object;
            if (playerControl.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(playerControl).Caught && playerInfo.IsDead)
            {
                playerInfo.IsDead = false;
                __state = true;
            }
        }

        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref bool __state, 
            [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
        {
            if (__state)
                playerInfo.IsDead = true;
        }
    }
    
    [HarmonyPatch(typeof(DoorConsole), nameof(DoorConsole.Use))]
    public static class DoorConsoleUsePatch
    {
        public static bool Prefix(DoorConsole __instance)
        {
            
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out _);
            if (!canUse) return false;
            PlayerControl.LocalPlayer.NetTransform.Halt();
            var minigame = Object.Instantiate(__instance.MinigamePrefab, Camera.main.transform);
            minigame.transform.localPosition = new Vector3(0f, 0f, -50f);

            try
            {
                minigame.Cast<IDoorMinigame>().SetDoor(__instance.MyDoor);
            } catch (InvalidCastException) { /* ignored */ }
            
            minigame.Begin(null);
            return false;
        }
    }
    #endregion
    
    #region Ladder
    [HarmonyPatch(typeof(Ladder), nameof(Ladder.CanUse))]
    public class LadderCanUse
    {
        public static void Prefix(DoorConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            ref bool __state)
        {
            __state = false;
            var playerControl = playerInfo.Object;
            if (playerControl.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(playerControl).Caught && playerInfo.IsDead)
            {
                playerInfo.IsDead = false;
                __state = true;
            }
        }

        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref bool __state)
        {
            if (__state)
                playerInfo.IsDead = true;
        }
    }
    
    [HarmonyPatch(typeof(Ladder), nameof(Ladder.Use))]
    public class LadderUse
    {
        public static bool Prefix(Ladder __instance)
        {
            var data = PlayerControl.LocalPlayer.Data;
            __instance.CanUse(data, out var flag, out var _);
            if (flag) PlayerControl.LocalPlayer.MyPhysics.RpcClimbLadder(__instance);
            return false;
        }
    }
    #endregion

    #region PlatformConsole
    [HarmonyPatch(typeof(PlatformConsole), nameof(PlatformConsole.CanUse))]
    public class PlatformConsoleCanUse
    {
        public static void Prefix(
            PlatformConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            ref bool __state)
        {
            __state = false;
            var playerControl = playerInfo.Object;
            if (playerControl.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(playerControl).Caught && playerInfo.IsDead)
            {
                playerInfo.IsDead = false;
                __state = true;
            }
        }

        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref bool __state)
        {
            if (__state)
                playerInfo.IsDead = true;
        }
    }
    #endregion
    
    #region DeconControl
    [HarmonyPatch(typeof(DeconControl), nameof(DeconControl.CanUse))]
    public class DeconControlUse
    {
        public static void Prefix(DoorConsole __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            ref bool __state)
        {
            __state = false;
            
            var playerControl = playerInfo.Object;
            if (playerControl.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(playerControl).Caught && playerInfo.IsDead)
            {
                playerInfo.IsDead = false;
                __state = true;
            }
        }

        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref bool __state)
        {
            if (__state)
                playerInfo.IsDead = true;
        }
    }
    #endregion
    
    #region global::Console
    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public class ConsoleCanUsePatch
    {
        public static void Prefix(Console __instance,
            [HarmonyArgument(0)] GameData.PlayerInfo playerInfo,
            ref bool __state)
        {
            __state = false;
            
            var playerControl = playerInfo.Object;
            if (playerControl.Is(RoleEnum.Phantom) && !Role.GetRole<Phantom>(playerControl).Caught && playerInfo.IsDead)
            {
                playerInfo.IsDead = false;
                __state = true;
            }
        }

        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref bool __state)
        {
            if (__state)
                playerInfo.IsDead = true;
        }
    }
    
    [HarmonyPatch(typeof(Console), nameof(Console.Use))]
    public class ConsoleUsePatch
    {
        public static bool Prefix(Console __instance)
        {
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var couldUse);
            if (canUse)
            {
                PlayerTask playerTask = __instance.FindTask(PlayerControl.LocalPlayer);
                if (playerTask.MinigamePrefab)
                {
                    var minigame = Object.Instantiate(playerTask.GetMinigamePrefab());
                    minigame.transform.SetParent(Camera.main.transform, false);
                    minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
                    minigame.Console = __instance;
                    minigame.Begin(playerTask);
                }
            }

            return false;
        }
    }
    #endregion
    
}