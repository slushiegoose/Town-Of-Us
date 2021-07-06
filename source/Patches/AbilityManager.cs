using UnityEngine;
using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;

namespace TownOfUs
{
    public static class AbilityManager
    {
        public static List<AbilityData> Buttons = new List<AbilityData>();

        public static void Add(AbilityData data)
        {
            data.Timer = data.MaxTimer;
            Buttons.Add(data);
        }

        [HarmonyPatch(typeof(KillButtonManager))]
        public static class KillButtonPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(KillButtonManager.PerformKill))]
            public static bool PerformKill(KillButtonManager __instance)
            {
                TownOfUs.LogMessage("Called PerformKill");
                if (
                    !__instance.isActiveAndEnabled ||
                    __instance.isCoolingDown
                ) return false;

                TownOfUs.LogMessage("Is active and not cooling down");

                var dataIdx = Buttons.FindIndex(x => x.KillButton == __instance);

                if (dataIdx == -1) return true;

                var data = Buttons[dataIdx];

                var isPlayerAbility = data is PlayerAbilityData;

                void Callback(object target)
                {
                    if (isPlayerAbility)
                        ((PlayerAbilityData)data).Callback((PlayerControl)target);
                    else
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
                        Callback(null);
                    return false;
                }

                object target = null;
                if (isPlayerAbility)
                    target = __instance.CurrentTarget;
                else
                    target = ((BodyAbilityData)data).Target;

                if (target != null)
                {
                    data.Timer = data.MaxTimer;
                    Callback(target);
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
                if (__instance.AmOwner)
                    HudManagerPatch.SetHudActive(true);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
            public static void FixedUpdate(PlayerControl __instance)
            {
                if (!__instance.AmOwner || __instance.Data.IsDead) return;
                var isImpostor = __instance.Data.IsImpostor;
                for (var i = 0;i < Buttons.Count;i++)
                {
                    var buttonData = Buttons[i];
                    if (
                        isImpostor && 
                        buttonData.KillButton == HudManager.Instance.KillButton
                    ) continue;

                    var button = buttonData.KillButton;

                    if (!float.IsNaN(buttonData.MaxDuration) && buttonData.DurationLeft != -1f)
                    {
                        var durationLeft = buttonData.DurationLeft = Mathf.Clamp(
                            buttonData.DurationLeft - Time.fixedDeltaTime,
                            0f,
                            buttonData.MaxDuration
                        );

                        if (durationLeft > 0f)
                        {
                            button.SetCoolDown(durationLeft, buttonData.MaxDuration);
                            continue;
                        }
                        else if (durationLeft == 0f)
                        {
                            buttonData.OnDurationEnd();
                            buttonData.Timer = buttonData.MaxTimer;
                            buttonData.DurationLeft = -1f;
                            button.SetCoolDown(buttonData.MaxTimer, buttonData.MaxTimer);
                            continue;
                        }
                    }

                    if (!__instance.CanMove)
                    {
                        if (Minigame.Instance != null)
                        {
                            if (Minigame.Instance.MyTask == null)
                                break;
                        }
                        else
                            break;
                    }


                    if (buttonData.Icon != null) button.renderer.sprite = buttonData.Icon;
                    button.transform.localPosition = buttonData.Position;

                    if (buttonData.Timer > 0f)
                    {
                        buttonData.Timer -= Time.fixedDeltaTime;
                        button.SetCoolDown(buttonData.Timer, buttonData.MaxTimer);
                    }

                    if (!float.IsNaN(buttonData.MaxDuration))
                    {
                        button.renderer.color = Palette.EnabledColor;
                        button.renderer.material.SetFloat("_Desat", 0f);
                        continue;
                    }

                    if (buttonData.IsHighlighted != null)
                    {
                        var highlighted = buttonData.IsHighlighted();
                        button.renderer.color =
                            highlighted ? Palette.EnabledColor : Palette.DisabledClear;
                        button.renderer.material.SetFloat("_Desat", highlighted ? 0f : 1f);
                        continue;
                    }

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

                        Utils.SetTarget(
                            ref playerAbility.Target,
                            button,
                            buttonData.Range,
                            targets
                        );

                        material = playerAbility.Target?.myRend.material;
                    }
                    else
                    {
                        bodyAbility = (BodyAbilityData)buttonData;
                        var targets = UnityEngine.Object.FindObjectsOfType<DeadBody>().ToList();
                        if (bodyAbility.TargetFilter != null)
                            targets = targets.Where(bodyAbility.TargetFilter).ToList();

                        var oldTarget = bodyAbility.Target;

                        DeadBody target = null;

                        Utils.SetTarget(
                            ref target,
                            button,
                            buttonData.Range,
                            targets
                        );

                        if (oldTarget != null && oldTarget.ParentId != target?.ParentId)
                            oldTarget.bodyRenderer.material.SetFloat("_Outline", 0f);

                        material = target?.bodyRenderer.material;

                        var hasTarget = target != null;
                        var renderer = button.renderer;
                        renderer.color = hasTarget ? Palette.EnabledColor : Palette.DisabledClear;
                        renderer.material.SetFloat("_Desat", hasTarget ? 0f : 1f);

                        bodyAbility.Target = target;
                    }

                    if (material != null)
                    {
                        material.SetFloat("_Outline", button.isActive ? 1 : 0);
                        material.SetColor("_OutlineColor", buttonData.TargetColor);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(HudManager))]
        public static class HudManagerPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(HudManager.SetHudActive))]
            public static void SetHudActive([HarmonyArgument(0)] bool isActive)
            {
                for (var i = 0;i < Buttons.Count;i++)
                    Buttons[i].KillButton.gameObject.SetActive(
                        isActive && !PlayerControl.LocalPlayer.Data.IsDead
                    );
            }
        }
    }

    public class BodyAbilityData : AbilityData
    {
        public DeadBody Target;
        public Action<DeadBody> Callback;
        public Func<DeadBody, bool> TargetFilter;
    }

    public class PlayerAbilityData : AbilityData
    {
        public PlayerControl Target;
        public Action<PlayerControl> Callback;
        public Func<PlayerControl, bool> TargetFilter;
    }

    public abstract class AbilityData
    {
        private float _Timer { get; set; }

        public KillButtonManager KillButton;

        public float MaxTimer;
        public float Timer
        {
            get =>_Timer;
            set => _Timer = Mathf.Clamp(value, 0f, MaxTimer);
        }
        public float Range;

        public Color TargetColor;

        public Func<bool> IsHighlighted;
        public float DurationLeft = -1f;
        public float MaxDuration = float.NaN;
        public Action OnDurationEnd;

        public Sprite Icon;
        public Vector3 Position;
    }
}
