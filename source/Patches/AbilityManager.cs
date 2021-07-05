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
                    __instance.isCoolingDown ||
                    __instance.CurrentTarget == null
                ) return false;

                var dataIdx = Buttons.FindIndex(x => x.KillButton == __instance);

                if (dataIdx == -1) return true;

                var data = Buttons[dataIdx];

                data.Callback(__instance.CurrentTarget);

                return false;
            }
        }

        [HarmonyPatch(typeof(PlayerControl))]
        public static class PlayerControlPatch
        {
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
                        buttonData.KillButton == HudManager.Instance.KillButton ||
                        buttonData.MaxTimer == 0f
                    ) continue;

                    if (!__instance.CanMove)
                    {
                        if (Minigame.Instance != null)
                        {
                            if (Minigame.Instance.MyTask == null) continue;
                        }
                        else
                            continue;
                    }

                    if (buttonData.Timer > 0f)
                    {
                        buttonData.Timer = Mathf.Clamp(
                            buttonData.Timer - Time.fixedDeltaTime,
                            0f,
                            buttonData.MaxTimer
                        );
                        var button = buttonData.KillButton;
                        button.SetCoolDown(buttonData.Timer, buttonData.MaxTimer);

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
                            material.SetFloat("_Outline", (button.isActive ? 1 : 0));
                            material.SetColor("_OutlineColor", buttonData.TargetColor);
                        }
                    }

                }
            }
        }

        [HarmonyPatch(typeof(HudManager))]
        public static class HudManagerPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(HudManager.SetHudActive))]
            public static void SetHudActive(
                HudManager __instance,
                [HarmonyArgument(0)] bool isActive)
            {
                for (var i = 0;i < Buttons.Count;i++)
                    Buttons[i].KillButton.gameObject.SetActive(
                        isActive && !PlayerControl.LocalPlayer.Data.IsDead
                    );
            }
        }
    }

    public struct AbilityData
    {
        private float _Timer;

        public PlayerControl Target;

        public KillButtonManager KillButton;

        public float MaxTimer;
        public float Timer
        {
            get => _Timer;
            set => _Timer = Mathf.Min(value, MaxTimer);
        }
        public float Range;


        public Color TargetColor;

        public Action<PlayerControl> Callback;
        public Func<PlayerControl, bool> TargetFilter;
    }
}
