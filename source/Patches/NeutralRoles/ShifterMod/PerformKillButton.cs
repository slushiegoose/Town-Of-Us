using System;
using System.Collections;
using System.Linq;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Collections.Generic;
using TownOfUs.CrewmateRoles.InvestigatorMod;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.CrewmateRoles.SnitchMod;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.NeutralRoles.ShifterMod
{
    public enum ShiftEnum
    {
        NonImpostors,
        RegularCrewmates,
        Nobody
    }

    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    [HarmonyPriority(Priority.Last)]
    public class PerformKillButton

    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Shifter);
            if (!flag) return true;
            var role = Role.GetRole<Shifter>(PlayerControl.LocalPlayer);
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
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
                var medic = role.ClosestPlayer.getMedic().Player.PlayerId;

                var writer1 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.AttemptSound, SendOption.Reliable, -1);
                writer1.Write(medic);
                writer1.Write(role.ClosestPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer1);
                if (CustomGameOptions.ShieldBreaks) role.LastShifted = DateTime.UtcNow;
                StopKill.BreakShield(medic, role.ClosestPlayer.PlayerId, CustomGameOptions.ShieldBreaks);

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
            overlay.showAll = null;
            renderer.sprite = TownOfUs.NormalKill;
        }


        public static void Shift(Shifter shifterRole, PlayerControl other)
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
            var resetShifter = false;
            var snitch = false;

            Role newRole;

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
                case RoleEnum.Executioner:
                case RoleEnum.Spy:
                case RoleEnum.Snitch:
                case RoleEnum.Arsonist:
                case RoleEnum.Crewmate:
                case RoleEnum.Altruist:

                    if (role == RoleEnum.Investigator) Footprint.DestroyAll(Role.GetRole<Investigator>(other));


                    newRole = Role.GetRole(other);
                    newRole.Player = shifter;

                    if (role == RoleEnum.Snitch) CompleteTask.Postfix(shifter);

                    var modifier = Modifier.GetModifier(other);
                    var modifier2 = Modifier.GetModifier(shifter);
                    if (modifier != null && modifier2 != null)
                    {
                        modifier.Player = shifter;
                        modifier2.Player = other;
                        Modifier.ModifierDictionary.Remove(other.PlayerId);
                        Modifier.ModifierDictionary.Remove(shifter.PlayerId);
                        Modifier.ModifierDictionary.Add(shifter.PlayerId, modifier);
                        Modifier.ModifierDictionary.Add(other.PlayerId, modifier2);
                    }
                    else if (modifier2 != null)
                    {
                        modifier2.Player = other;
                        Modifier.ModifierDictionary.Remove(shifter.PlayerId);
                        Modifier.ModifierDictionary.Add(other.PlayerId, modifier2);
                    }
                    else if (modifier != null)
                    {
                        modifier.Player = shifter;
                        Modifier.ModifierDictionary.Remove(other.PlayerId);
                        Modifier.ModifierDictionary.Add(shifter.PlayerId, modifier);
                    }


                    Role.RoleDictionary.Remove(shifter.PlayerId);
                    Role.RoleDictionary.Remove(other.PlayerId);

                    Role.RoleDictionary.Add(shifter.PlayerId, newRole);
                    lovers = role == RoleEnum.Lover;
                    snitch = role == RoleEnum.Snitch;

                    foreach (var exeRole in Role.AllRoles.Where(x => x.RoleType == RoleEnum.Executioner))
                    {
                        var executioner = (Executioner) exeRole;
                        var target = executioner.target;
                        if (other == target)
                        {
                            executioner.target.nameText.color = Color.white;
                            ;
                            executioner.target = shifter;

                            executioner.RegenTask();
                        }
                    }

                    if (CustomGameOptions.WhoShifts == ShiftEnum.NonImpostors ||
                        role == RoleEnum.Crewmate && CustomGameOptions.WhoShifts == ShiftEnum.RegularCrewmates)
                    {
                        resetShifter = true;
                        shifterRole.Player = other;
                        Role.RoleDictionary.Add(other.PlayerId, shifterRole);
                    }
                    else
                    {
                        new Crewmate(other);
                    }


                    break;

                case RoleEnum.Underdog:
                case RoleEnum.Undertaker:
                case RoleEnum.Assassin:
                case RoleEnum.Swooper:
                case RoleEnum.Miner:
                case RoleEnum.Morphling:
                case RoleEnum.Camouflager:
                case RoleEnum.Janitor:
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

                if (other.AmOwner) Coroutines.Start(ShowShift());

                if (lovers)
                {
                    var lover = Role.GetRole<Lover>(shifter);
                    var otherLover = lover.OtherLover;
                    otherLover.RegenTask();
                }

                if (snitch)
                {
                    var snitchRole = Role.GetRole<Snitch>(shifter);
                    snitchRole.ImpArrows.DestroyAll();
                    snitchRole.SnitchArrows.DestroyAll();
                    snitchRole.SnitchTargets.Clear();
                    CompleteTask.Postfix(shifter);
                    if (other.AmOwner)
                        foreach (var player in PlayerControl.AllPlayerControls)
                            player.nameText.color = Color.white;
                }

                if (resetShifter) shifterRole.RegenTask();
            }

            //System.Console.WriteLine(shifter.Is(RoleEnum.Sheriff));
            //System.Console.WriteLine(other.Is(RoleEnum.Sheriff));
            //System.Console.WriteLine(Roles.Role.GetRole(shifter));
            if (shifter.AmOwner || other.AmOwner)
            {
                if (shifter.Is(RoleEnum.Arsonist) && other.AmOwner)
                    Role.GetRole<Arsonist>(shifter).IgniteButton.Destroy();
                DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
                DestroyableSingleton<HudManager>.Instance.KillButton.isActive = false;

                Lights.SetLights();
            }
        }
    }
}
