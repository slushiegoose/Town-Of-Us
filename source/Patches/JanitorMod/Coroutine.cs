using System;
using System.Collections;
using UnityEngine;

namespace TownOfUs.JanitorMod
{
    public class Coroutine
    {
        private static readonly int BodyColor = Shader.PropertyToID("_BodyColor");
        private static readonly int BackColor = Shader.PropertyToID("_BackColor");

        public static IEnumerator CleanCoroutine(DeadBody body, Roles.Janitor role)
        {
            KillButtonTarget.SetTarget(DestroyableSingleton<HudManager>.Instance.KillButton, null, role);
            role.Player.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
            var renderer = body.GetComponent<SpriteRenderer>();
            var backColor = renderer.material.GetColor(BackColor);
            var bodyColor = renderer.material.GetColor(BodyColor);
            var newColor = new Color(1f, 1f, 1f, 0f);
            for (var i = 0; i < 60; i++)
            {
                if (body == null) yield break;
                renderer.color = Color.Lerp(backColor, newColor, i / 60f);
                renderer.color = Color.Lerp(bodyColor, newColor, i / 60f);
                yield return null;

            }
            UnityEngine.Object.Destroy(body.gameObject);
            

        }
    }
}