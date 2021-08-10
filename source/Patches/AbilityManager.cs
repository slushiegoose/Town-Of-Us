using UnityEngine;
using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.Roles;
namespace TownOfUs
{
    public static class AbilityManager
    {
        public static List<AbilityData> Buttons = new List<AbilityData>();

        private static bool _HotkeyPressed = false;
        private static bool HotkeyPressed
        {
            get
            {
                if (_HotkeyPressed)
                {
                    _HotkeyPressed = false;
                    return true;
                }
                return false;
            }
        }
        private static void ChangeButtonsState(bool active) => HudManager.Instance.SetHudActive(active);

        public static void EnableButtons() => ChangeButtonsState(true);
        public static void DisableButtons() => ChangeButtonsState(false);

        public static void Add(AbilityData data)
        {
            data.Timer = data.MaxTimer;
            var hudKill = HudManager.Instance.KillButton;
            if (Buttons.Count == 0 && data.Position == AbilityPositions.KillButton)
                data.KillButton = hudKill;
            else
                data.KillButton = UnityEngine.Object.Instantiate(hudKill, hudKill.transform.parent);

            var icon = data.Icon;
            if (icon != null)
            {
                data.KillButton.killText.gameObject.SetActive(false);
                data.UpdateSprite();
            }
            Buttons.Add(data);
        }

        [HarmonyPatch(typeof(MeetingHud))]
        public static class MeetingPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(MeetingHud.Start))]
            public static void MeetingStart()
            {
                foreach (var button in Buttons)
                {
                    if (button.DurationLeft > 0f)
                    {
                        button.OnDurationEnd();
                        var maxTimer = button.Timer = button.MaxTimer;
                        button.DurationLeft = -1f;
                        button.KillButton.SetCoolDown(maxTimer, maxTimer);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ExileController))]
        public static class ExileControllerPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(ExileController.WrapUp))]
            public static void PostMeeting()
            {
                for (var i = 0;i < Buttons.Count;i++)
                {
                    var button = Buttons[i];
                    button.Timer = button.MaxTimer;
                    button.CooldownMultiplier = 1f;
                }
            }
        }

        [HarmonyPatch(typeof(KillButtonManager))]
        public static class KillButtonPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(KillButtonManager.PerformKill))]
            public static bool PerformKill(KillButtonManager __instance)
            {
                if (
                    !__instance.isActiveAndEnabled ||
                    __instance.isCoolingDown
                ) return false;

                var dataIdx = Buttons.FindIndex(x => x.KillButton == __instance);

                if (dataIdx == -1) return true;

                var data = Buttons[dataIdx];

                if (CamouflageUnCamouflage.IsCamoed && data.HiddenOnCamo) return false;

                var isPlainAbility = data is PlainAbilityData;
                var isPlayerAbility = !isPlainAbility && data is PlayerAbilityData;
                var isBodyAbility = !isPlayerAbility && data is BodyAbilityData;

                void Callback(object target)
                {
                    if (isPlainAbility)
                        ((PlainAbilityData)data).Callback();
                    else if (isPlayerAbility)
                        ((PlayerAbilityData)data).Callback((PlayerControl)target);
                    else if (isBodyAbility)
                        ((BodyAbilityData)data).Callback((DeadBody)target);
                }

                if (!float.IsNaN(data.MaxDuration))
                {
                    data.DurationLeft = data.MaxDuration;
                    Callback(null);
                }

                if (data.IsHighlighted != null)
                {
                    if (data.IsHighlighted())
                    {
                        if (!float.IsNaN(data.MaxTimer))
                            data.Timer = data.MaxTimer;
                        Callback(null);
                    }
                    return false;
                }

                if (!isPlainAbility)
                {
                    object target = null;
                    if (isPlayerAbility)
                        target = __instance.CurrentTarget;
                    else if (isBodyAbility)
                        target = ((BodyAbilityData)data).Target;

                    if (target != null)
                    {
                        if (data.SyncWithKill)
                            PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                        else
                            data.Timer = data.MaxTimer;

                        Callback(target);
                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(PlayerControl))]
        public static class PlayerControlPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(PlayerControl.Die))]
            public static void OnDeath(PlayerControl __instance)
            {
                for (var i = 0;i < Buttons.Count;i++)
                {
                    var buttonData = Buttons[i];
                    if (buttonData is PlainAbilityData) continue;
                    var material = (buttonData is PlayerAbilityData playerAbility
                        ? playerAbility.Target?.myRend
                        : ((BodyAbilityData) buttonData).Target?.bodyRenderer).material;
                    material?.SetFloat("_Outline", 0f);
                }
                
                if (__instance.AmOwner)
                    HudManagerPatch.SetHudActive(false);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PlayerControl.Revive))]
            public static void OnRevive(PlayerControl __instance)
            {
                if (__instance.AmOwner)
                    HudManagerPatch.SetHudActive(true);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
            public static void FixedUpdate(PlayerControl __instance)
            {
                if (
                    !__instance.AmOwner ||
                    __instance.Data.IsDead ||
                    // just in case
                    __instance.Data.Disconnected
                ) return;
                var isImpostor = __instance.Data.IsImpostor;
                var cannotDeplete = !__instance.CanMove && (Minigame.Instance == null || Minigame.Instance.MyTask == null);
                for (var i = 0;i < Buttons.Count;i++)
                {
                    var buttonData = Buttons[i];
                    var isHudKill = buttonData.KillButton == HudManager.Instance.KillButton;
                    if (
                        isImpostor && 
                        isHudKill
                    ) continue;

                    var button = buttonData.KillButton;

                    if (!button.isActiveAndEnabled) continue;
                    
                    if (buttonData.Position != AbilityPositions.KillButton)
                        button.transform.localPosition = TOUConstants.GetPosition(buttonData.Position);

                    var durationLeft = buttonData.DurationLeft;
                    if (durationLeft != -1f)
                    {
                        var maxDuration = buttonData.MaxDuration;

                        durationLeft = buttonData.DurationLeft = Mathf.Clamp(
                            buttonData.DurationLeft - Time.fixedDeltaTime,
                            0f,
                            maxDuration
                        );

                        if (durationLeft > 0f)
                            button.SetCoolDown(durationLeft, maxDuration);
                        else if (durationLeft == 0f)
                        {
                            buttonData.OnDurationEnd();
                            buttonData.Timer = buttonData.MaxTimer;
                            buttonData.DurationLeft = -1f;
                            button.SetCoolDown(buttonData.MaxTimer, buttonData.MaxTimer);
                        }
                        continue;
                    }

                    if (cannotDeplete)
                    {
                        buttonData.SetUseable(false);
                        continue;
                    }

                    if (buttonData.Timer > 0f || __instance.killTimer > 0f)
                    {
                        if (buttonData.SyncWithKill)
                            buttonData.Timer = __instance.killTimer;
                        else
                        {
                            buttonData.Timer -= Time.fixedDeltaTime;
                            if (isHudKill)
                                __instance.SetKillTimer(buttonData.Timer);
                        }

                        if (buttonData.Timer == 0f)
                        {
                            buttonData.CooldownMultiplier = 1f;
                        }

                        button.SetCoolDown(
                            buttonData.Timer,
                            buttonData.MaxTimer
                        );
                    }

                    if (CamouflageUnCamouflage.IsCamoed && buttonData.HiddenOnCamo)
                    {
                        buttonData.SetUseable(false);
                        continue;
                    }

                    if (!float.IsNaN(buttonData.MaxDuration))
                    {
                        buttonData.SetUseable(true);
                        continue;
                    }

                    if (buttonData.IsHighlighted != null)
                    {
                        buttonData.SetUseable(buttonData.IsHighlighted());
                        continue;
                    }

                    if (buttonData is PlainAbilityData) continue;

                    var targetAbility = (TargetAbilityData)buttonData;
                    var isPlayerAbility = buttonData is PlayerAbilityData;
                    PlayerAbilityData playerAbility = null;
                    BodyAbilityData bodyAbility = null;
                    Material material = null;
                    if (isPlayerAbility)
                    {
                        playerAbility = (PlayerAbilityData)buttonData;
                        var targets = PlayerControl.AllPlayerControls.ToArray().ToList();
                        if (playerAbility.TargetFilter != null)
                            targets = targets.Where(playerAbility.TargetFilter).ToList();
                        PlayerControl target = null;

                        Utils.SetClosestPlayer(
                            ref target,
                            targetAbility.Range,
                            targets
                        );

                        if (target != button.CurrentTarget)
                        {
                            button.CurrentTarget?.MyRend.material.SetFloat("_Outline", 0f);
                        }

                        button.CurrentTarget = target;

                        playerAbility.SetUseable(target != null);

                        material = target?.myRend.material;
                    }
                    else
                    {
                        bodyAbility = (BodyAbilityData)buttonData;
                        var targets = UnityEngine.Object.FindObjectsOfType<DeadBody>().ToList();
                        if (bodyAbility.TargetFilter != null)
                            targets = targets.Where(bodyAbility.TargetFilter).ToList();

                        var oldTarget = bodyAbility.Target;

                        DeadBody target = null;

                        Utils.SetClosestBody(
                            ref target,
                            targetAbility.Range,
                            targets
                        );

                        if (oldTarget != null && oldTarget.ParentId != target?.ParentId)
                            oldTarget.bodyRenderer.material.SetFloat("_Outline", 0f);

                        material = target?.bodyRenderer.material;

                        bodyAbility.SetUseable(target != null);

                        bodyAbility.Target = target;
                    }

                    if (material != null && material.GetFloat("_Outline") == 0f)
                    {
                        material.SetFloat("_Outline", button.isActive ? 1 : 0);
                        material.SetColor("_OutlineColor", targetAbility.TargetColor);
                    }
                }

                if (!isImpostor && Buttons.Count > 0 && HotkeyPressed)
                    Buttons[0].KillButton.PerformKill();

            }
        }

        [HarmonyPatch(typeof(HudManager))]
        public static class HudManagerPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(HudManager.Update))]
            public static void HotkeyPatch()
            {
                // See https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
                // for why this has to be done in `HudManager.Update`
                if (Input.GetKeyDown(KeyCode.Q))
                    _HotkeyPressed = true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(HudManager.SetHudActive))]
            public static void SetHudActive([HarmonyArgument(0)] bool isActive)
            {
                var isReallyActive = isActive && !PlayerControl.LocalPlayer.Data.IsDead;
                foreach (var button in Buttons) {
                    var killButton = button.KillButton;
                    killButton.gameObject.SetActive(isReallyActive);
                    if (isReallyActive && killButton.renderer.sprite.name != "kill")
                        killButton.killText.gameObject.SetActive(false);
                }
            }
        }
    }

    public class BodyAbilityData : TargetAbilityData
    {
        public DeadBody Target;
        public Action<DeadBody> Callback;
        public Func<DeadBody, bool> TargetFilter;
    }

    public class PlayerAbilityData : TargetAbilityData
    {
        public PlayerControl Target => KillButton?.CurrentTarget;
        public Action<PlayerControl> Callback;
        public Func<PlayerControl, bool> TargetFilter;
    }

    public class PlainAbilityData : AbilityData
    {
        public Action Callback;
    }

    public abstract class TargetAbilityData : AbilityData
    {
        public Color TargetColor;
        public float Range;
    }

    public abstract class AbilityData
    {
        private float _Timer { get; set; }
        private float _MaxTimer { get; set; } = float.NaN;

        public KillButtonManager KillButton;

        public float MaxTimer
        {
            get => _MaxTimer * CooldownMultiplier;
            set => _MaxTimer = value;
        }
        public float Timer
        {
            get => _Timer;
            set => _Timer = Mathf.Clamp(value, 0f, MaxTimer);
        }

        public float CooldownMultiplier = 1f;
        public Func<bool> IsHighlighted;
        public float DurationLeft = -1f;
        public float MaxDuration = float.NaN;
        public Action OnDurationEnd;
        public bool SyncWithKill = false;
        public bool HiddenOnCamo = false;

        public Sprite Icon;
        public AbilityPositions Position;

        public void UpdateSprite()
        {
            var renderer = KillButton?.renderer;
            if (renderer != null)
            {
                renderer.sprite = Icon;
                CooldownHelpers.SetCooldownNormalizedUvs(renderer);
            }
        }
        public void SetUseable(bool canUse)
        {
            var spriteRenderer = KillButton.renderer;
            var killText = KillButton.killText.isActiveAndEnabled
                ? KillButton.killText
                : null;
            if (canUse)
            {
                spriteRenderer.color = Palette.EnabledColor;
                spriteRenderer.material.SetFloat("_Desat", 0f);
                if (killText != null)
                    killText.color = Palette.EnabledColor;
            }
            else
            {
                spriteRenderer.color = Palette.DisabledClear;
                spriteRenderer.material.SetFloat("_Desat", 1f);
                if (killText != null)
                    killText.color = Palette.DisabledClear;
            }
        }
    }
}
