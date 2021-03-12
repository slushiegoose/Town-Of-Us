using System;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Collections.Generic;
using TownOfUs.JesterMod;
using TownOfUs.LoversMod;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class PerformKillButton
    
    {
        
        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.isShifter();
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = Methods.ShifterShiftTimer() == 0f;
            if (!flag2) return false;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (Vector2.Distance(Methods.ClosestPlayer.GetTruePosition(),
                PlayerControl.LocalPlayer.GetTruePosition()) > maxDistance) return false;
            var playerId = Methods.ClosestPlayer.PlayerId;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Shift, SendOption.Reliable, -1);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Shift(PlayerControl.LocalPlayer, Methods.ClosestPlayer);
            return false;
        }


        public static void Shift(PlayerControl shifter, PlayerControl other)
        {
            var role = Utils.GetRole(other);
            //TODO - Shift Animation

            List<PlayerTask> tasks1, tasks2;
            List<GameData.TaskInfo> taskinfos1, taskinfos2;

            var swapTasks = true;
            var lovers = false;
            
            switch (role)
            {
                case Roles.Sheriff:
                    Utils.Sheriff = shifter;
                    Utils.Shifter = other;
                    break;


                case Roles.Jester:
                    Utils.Jester = shifter;
                    Utils.Shifter = other;
                    break;
                    
                case Roles.Engineer:
                    Utils.Engineer = shifter;
                    Utils.Shifter = other;
                    break;
                
                case Roles.Lover1:
                    Utils.Lover1 = shifter;
                    Utils.Shifter = other;
                    lovers = true;
                    break;
                
                case Roles.Lover2:
                    Utils.Lover2 = shifter;
                    Utils.Shifter = other;
                    lovers = true;
                    break;
                
                case Roles.Mayor:
                    Utils.Mayor = shifter;
                    Utils.Shifter = other;
                    break;
                
                case Roles.Swapper:
                    Utils.Swapper = shifter;
                    Utils.Shifter = other;
                    break;
                
                case Roles.Investigator:
                    Utils.Investigator = shifter;
                    Utils.Shifter = other;
                    InvestigatorMod.Footprint.DestroyAll();
                    break;
                
                case Roles.Crewmate:
                    Utils.Shifter = other;
                    break;
                
                case Roles.TimeMaster:
                    Utils.TimeMaster = shifter;
                    Utils.Shifter = other;
                    break;
                    
                case Roles.Impostor:
                    shifter.Data.IsImpostor = true;
                    shifter.MurderPlayer(shifter);
                    shifter.Data.IsImpostor = false;
                    swapTasks = false;
                    break;
            }

            if (swapTasks)
            {
                tasks1 = other.myTasks;
                taskinfos1 = other.Data.Tasks;
                tasks2 = shifter.myTasks;
                taskinfos2 = shifter.Data.Tasks;

                shifter.myTasks = tasks1;
                shifter.Data.Tasks = taskinfos1;
                other.myTasks = tasks2;
                other.Data.Tasks = taskinfos2;

                if (other.AmOwner)
                {
                    var overlay = DestroyableSingleton<HudManager>.Instance.KillOverlay;
                    overlay.ShowOne(shifter.Data, other.Data);
                }

                if (lovers)
                {
                    var otherLover = shifter.OtherLover();
                    otherLover.myTasks[0].Cast<ImportantTextTask>().GenTaskText(otherLover);
                }
            }
            
            Methods.LastShifted = DateTime.Now;
            if (shifter.AmOwner)
            {
                DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
                DestroyableSingleton<HudManager>.Instance.KillButton.isActive = false;
            }


        }

        
    }
}