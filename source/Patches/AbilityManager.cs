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
                if (
                    !__instance.isActiveAndEnabled ||
                    __instance.isCoolingDown
                ) return false;

                var dataIdx = Buttons.FindIndex(x => x.KillButton == __instance);

                if (dataIdx == -1) return true;

                var data = Buttons[dataIdx];

                if (data.IsHighlighted != null)
                {
                    if (data.IsHighlighted())
                        data.Callback(null);
                    return false;
                }

                if (__instance.CurrentTarget != null)
                {
                    data.Callback(__instance.CurrentTarget);
                    data.Timer = data.MaxTimer;

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

                    var button = buttonData.KillButton;

                    if (buttonData.Icon != null) button.renderer.sprite = buttonData.Icon;
                    button.transform.localPosition = buttonData.Position;

                    if (buttonData.Timer > 0f)
                    {
                        buttonData.Timer -= Time.fixedDeltaTime;
                        button.SetCoolDown(buttonData.Timer, buttonData.MaxTimer);
                    }

                    if (buttonData.IsHighlighted != null)
                    {
                        var highlighted = buttonData.IsHighlighted();
                        button.renderer.color =
                            highlighted ? Palette.EnabledColor : Palette.DisabledClear;
                        button.renderer.material.SetFloat("_Desat", highlighted ? 0f : 1f);
                        return;
                    }
                    var targets = PlayerControl.AllPlayerControls.ToArray().ToList();
                    if (buttonData.TargetFilter != null)
                        targets = targets.Where(buttonData.TargetFilter).ToList();

                    Utils.SetTarget(
                        ref buttonData.Target,
                        button,
                        buttonData.Range,
                        targets
                    );

                    if (buttonData.Target != null)
                    {
                        var material = buttonData.Target.MyRend.material;
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

    public class AbilityData
    {
        private float _Timer { get; set; }

        public PlayerControl Target;

        public KillButtonManager KillButton;

        public float MaxTimer;
        public float Timer
        {
            get =>_Timer;
            set => _Timer = Mathf.Clamp(value, 0f, MaxTimer);
        }
        public float Range;


        public Color TargetColor;

        public Action<PlayerControl> Callback;
        public Func<PlayerControl, bool> TargetFilter;

        public Func<bool> IsHighlighted;

        public Sprite Icon;
        public Vector3 Position;
    }
}
