using System;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs.CustomHats
{
    public class HatAnimator : MonoBehaviour
    {
        public PlayerControl player;
        private float _animationTimer;
        private int _frame;
        private uint _lastHat = uint.MaxValue;

        internal HatCreation.HatData? CurrentHat;

        public HatAnimator(IntPtr ptr) : base(ptr)
        {
        }

        private void FixedUpdate()
        {
            if (player.Data.HatId != _lastHat)
            {
                _lastHat = player.Data.HatId;
                if (!HatCreation.IdToData.ContainsKey(player.Data.HatId))
                {
                    CurrentHat = null;
                }
                else
                {
                    CurrentHat = HatCreation.IdToData[player.Data.HatId];
                    _frame = 0;
                    _animationTimer = 1f / CurrentHat.Value.fps;
                }
            }

            if (CurrentHat == null) return;
            if (!CurrentHat.Value.animated) return;

            _animationTimer -= Time.fixedDeltaTime;
            if (_animationTimer < 0)
            {
                _frame++;
                _frame %= CurrentHat.Value.framecount;
                _animationTimer = 1f / CurrentHat.Value.fps;
            }


            if (player.HatRenderer.FrontLayer.sprite != HatCreation.EmptySprite)
                player.HatRenderer.FrontLayer.sprite = HatCreation.AnimatedHats[CurrentHat.Value][_frame];
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
    public static class AddAnimation
    {
        public static void Postfix(PlayerControl __instance)
        {
            var animator = __instance.HatRenderer.gameObject.AddComponent<HatAnimator>();
            animator.player = __instance;
        }
    }
}
