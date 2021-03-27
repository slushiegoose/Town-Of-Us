using System;
using System.Collections;
using System.Linq;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Collections.Generic;
using TownOfUs.JesterMod;
using TownOfUs.LoversMod;
using UnityEngine;

namespace TownOfUs.ShifterMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    [HarmonyPriority(Priority.Last)]
    public class PerformKillButton

    {

        public static bool Prefix(KillButtonManager __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Shifter);
            if (!flag) return true;
            var role = Roles.Role.GetRole<Roles.Shifter>(PlayerControl.LocalPlayer);
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var flag2 = role.ShifterShiftTimer() == 0f;
            if (!flag2) return false;
            if (!__instance.enabled) return false;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (Vector2.Distance(role.ClosestPlayer.GetTruePosition(),
                PlayerControl.LocalPlayer.GetTruePosition()) > maxDistance) return false;
            if (role.ClosestPlayer == null) return false;
            var playerId = role.ClosestPlayer.PlayerId;
            if (role.ClosestPlayer.isShielded())
            {
                if (CustomGameOptions.PlayerMurderIndicator)
                {
                    var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.AttemptSound, Hazel.SendOption.None, -1);
                    writer1.Write(role.ClosestPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer1);
                    MedicMod.StopKill.BreakShield(role.ClosestPlayer.PlayerId, false);
                }

                return false;
            }

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Shift, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Shift(role, role.ClosestPlayer);
            return false;
        }

        public static IEnumerator ShowShift()
        {
            var wait = new WaitForSeconds(0.83333336f);
            var hud = DestroyableSingleton<HudManager>.Instance;
            var overlay = hud.KillOverlay;
            var transform = overlay.flameParent.transform;
            var flame = transform.GetChild(0).gameObject;
            var renderer = flame.GetComponent<SpriteRenderer>();

            renderer.sprite = TownOfUs.ShiftKill;
            var background = overlay.background;
            overlay.flameParent.SetActive(true);
            yield return new WaitForLerp(0.16666667f,
                delegate(float t) { overlay.flameParent.transform.localScale = new Vector3(1f, t, 1f); });
            yield return new WaitForSeconds(1f);
            yield return new WaitForLerp(0.16666667f,
                delegate(float t) { overlay.flameParent.transform.localScale = new Vector3(1f, 1f - t, 1f); });
            overlay.flameParent.SetActive(false);
            overlay.Field_6 = null;
            renderer.sprite = TownOfUs.NormalKill;
            yield break;



        }



        public static void Shift(Roles.Shifter shifterRole, PlayerControl other)
        {
            var role = Utils.GetRole(other);
            //System.Console.WriteLine(role);
            //TODO - Shift Animation
            shifterRole.LastShifted = DateTime.UtcNow;
            var shifter = shifterRole.Player;
            List<PlayerTask> tasks1, tasks2;
            List<GameData.TaskInfo> taskinfos1, taskinfos2;

            var swapTasks = true;
            var lovers = false;

            Roles.Role newRole;

            switch (role)
            {

                case RoleEnum.Sheriff:
                case RoleEnum.Jester:
                case RoleEnum.Engineer:
                case RoleEnum.Lover:
                case RoleEnum.Mayor:
                case RoleEnum.Swapper:
                case RoleEnum.Investigator:
                case RoleEnum.TimeLord:
                case RoleEnum.Medic:
                case RoleEnum.Seer:
                case RoleEnum.Child:
                case RoleEnum.Executioner:
                case RoleEnum.Spy:

                    if (role == RoleEnum.Investigator)
                    {
                        InvestigatorMod.Footprint.DestroyAll(Roles.Role.GetRole<Roles.Investigator>(other));
                    }

                    newRole = Roles.Role.GetRole(other);
                    newRole.Player = shifter;

                    Roles.Role.RoleDictionary.Remove(shifter.PlayerId);
                    Roles.Role.RoleDictionary.Remove(other.PlayerId);

                    Roles.Role.RoleDictionary.Add(shifter.PlayerId, newRole);
                    lovers = role == RoleEnum.Lover;

                    foreach (var exeRole in Roles.Role.AllRoles.Where(x => x.RoleType == RoleEnum.Executioner))
                    {
                        var executioner = (Roles.Executioner) exeRole;
                        var target = executioner.target;
                        if (other == target)
                        {
                            executioner.target = shifter;
                            executioner.RegenTask();
                        }

                    }



                    break;



                case RoleEnum.Crewmate:
                    shifterRole.Player = other;

                    Roles.Role.RoleDictionary.Add(other.PlayerId, shifterRole);
                    Roles.Role.RoleDictionary.Remove(shifter.PlayerId);
                    break;

                case RoleEnum.Morphling:
                case RoleEnum.Camouflager:
                case RoleEnum.Godfather:
                case RoleEnum.Janitor:
                case RoleEnum.Mafioso:
                case RoleEnum.LoverImpostor:
                case RoleEnum.Impostor:
                case RoleEnum.Glitch:
                case RoleEnum.Shifter:
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
                    Reactor.Coroutines.Start(ShowShift());
                }

                if (lovers)
                {
                    var lover = Roles.Role.GetRole<Roles.Lover>(shifter);
                    var otherLover = lover.OtherLover;
                    otherLover.RegenTask();
                }
            }

            //System.Console.WriteLine(shifter.Is(RoleEnum.Sheriff));
            //System.Console.WriteLine(other.Is(RoleEnum.Sheriff));
            //System.Console.WriteLine(Roles.Role.GetRole(shifter));
            if (shifter.AmOwner || other.AmOwner)
            {
                DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
                DestroyableSingleton<HudManager>.Instance.KillButton.isActive = false;
            }



        }
    }
}