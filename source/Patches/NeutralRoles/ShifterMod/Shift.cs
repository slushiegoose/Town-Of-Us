using HarmonyLib;
using System.Collections;
using TownOfUs.CrewmateRoles.InvestigatorMod;
using TownOfUs.CrewmateRoles.SnitchMod;
using Reactor.Extensions;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;
using Reactor;

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
    public class Shift
    {
        public static IEnumerator ShowShift()
        {
            var hud = DestroyableSingleton<HudManager>.Instance;
            var overlay = hud.KillOverlay;
            var flame = overlay.flameParent.transform.GetChild(0).gameObject;
            var renderer = flame.GetComponent<SpriteRenderer>();

            renderer.sprite = TownOfUs.ShiftKill;
            overlay.flameParent.SetActive(true);
            yield return new WaitForLerp(
                0.16666667f,
                t => overlay.flameParent.transform.localScale = new Vector3(1f, t, 1f)
            );
            yield return new WaitForSeconds(1f);
            yield return new WaitForLerp(
                0.16666667f,
                t => overlay.flameParent.transform.localScale = new Vector3(1f, 1f - t, 1f)
            );

            overlay.flameParent.SetActive(false);
            overlay.showAll = null;
            renderer.sprite = TownOfUs.NormalKill;
        }

        public static void ShiftRoles(Shifter shifter, Role with)
        {
            var shifterPlayer = shifter.Player;
            var withPlayer = with.Player;

            var amShifter = shifterPlayer.AmOwner;
            var amBeingShifted = withPlayer.AmOwner;

            var isParticipant = amShifter || amBeingShifted;

            if (with.Faction == Faction.Impostors || with.RoleType == RoleEnum.Glitch)
            {
                Utils.MurderPlayer(shifterPlayer, shifterPlayer);
                return;
            }

            if (isParticipant)
            {
                var buttons = AbilityManager.Buttons;

                foreach (var buttonData in buttons)
                {
                    var button = buttonData.KillButton;
                    if (button == HudManager.Instance.KillButton)
                        button.gameObject.SetActive(false);
                    else
                        button.gameObject.Destroy();
                }

                buttons.Clear();
            }

            var shifterModifier = Modifier.GetModifier(shifterPlayer);
            var withModifier = Modifier.GetModifier(withPlayer);

            with.Player = shifterPlayer;

            if (shifterModifier != null) shifterModifier.Player = withPlayer;
            if (withModifier != null) withModifier.Player = shifterPlayer;

            var whoGetsShifter = CustomGameOptions.WhoShifts;

            if (
                whoGetsShifter == ShiftEnum.NonImpostors ||
                (with.RoleType == RoleEnum.Crewmate && whoGetsShifter == ShiftEnum.RegularCrewmates))
            {
                shifter.Player = withPlayer;
            }
            else
            {
                Role.RoleDictionary.Remove(withPlayer.PlayerId);
                new Crewmate(withPlayer);
            }

            with.CreateButtons();
            shifter.CreateButtons();
            shifterModifier?.CreateButtons();
            withModifier?.CreateButtons();
            if (isParticipant)
                AbilityManager.HudManagerPatch.SetHudActive(true);

            var shifterTasks = shifterPlayer.myTasks;
            var shifterTasksData = shifterPlayer.Data.Tasks;
            shifterPlayer.myTasks = withPlayer.myTasks;
            shifterPlayer.Data.Tasks = withPlayer.Data.Tasks;
            withPlayer.myTasks = shifterTasks;
            withPlayer.Data.Tasks = shifterTasksData;

            if (amBeingShifted)
            {
                Coroutines.Start(ShowShift());
                shifter.RegenTask();
                switch (with.RoleType)
                {
                    case RoleEnum.Investigator:
                        Footprint.DestroyAll((Investigator)with);
                        break;
                    case RoleEnum.Medic:
                        ((Medic)with).ShieldedPlayer?.myRend.material.SetFloat("_Outline", 0f);
                        break;
                    case RoleEnum.Arsonist:
                        foreach (var playerId in ((Arsonist)with).DousedPlayers)
                        {
                            var player = Utils.PlayerById(playerId);
                            player?.myRend.material.SetColor("_VisorColor", Palette.VisorColor);
                        }
                        break;
                    case RoleEnum.Spy:
                        var countOverlay =
                            MapBehaviour.Instance?.countOverlay ??
                            GameObject.Find("CountOverlay")?.GetComponent<MapCountOverlay>();
                        if (countOverlay != null)
                            foreach (var area in countOverlay.CountAreas)
                                foreach (var icon in area.myIcons)
                                {
                                    var sprite = icon.GetComponent<SpriteRenderer>();
                                    if (sprite != null)
                                        PlayerControl.SetPlayerMaterialColors(Color.yellow, sprite);
                                }
                        break;
                    case RoleEnum.Snitch:
                        var snitch = (Snitch)with;
                        snitch.ImpArrows.DestroyAll();
                        snitch.SnitchArrows.DestroyAll();
                        snitch.SnitchTargets.Clear();
                        snitch.ImpArrows.Clear();
                        break;
                }
            }
            else if (amShifter)
            {
                with.RegenTask();
                if (with.RoleType == RoleEnum.Snitch)
                    CompleteTask.Postfix(shifterPlayer);
            }

            if (with.RoleType == RoleEnum.Lover)
            {
                var otherLover = ((Lover)with).OtherLover;
                if (otherLover.Player.AmOwner)
                    otherLover.RegenTask();
            }

            Role.NamePatch.UpdateAll();
        }
    }
}
