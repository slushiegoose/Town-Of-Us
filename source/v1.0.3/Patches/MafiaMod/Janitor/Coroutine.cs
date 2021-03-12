using System;
using System.Collections;   
using UnityEngine;

namespace TownOfUs.MafiaMod.Janitor
{
    public class Coroutine
    {
        private static readonly int BodyColor = Shader.PropertyToID("_BodyColor");
        private static readonly int BackColor = Shader.PropertyToID("_BackColor");

        public static IEnumerator CleanCoroutine(DeadBody body)
        {
            KillButtonTarget.SetTarget(DestroyableSingleton<HudManager>.Instance.KillButton, null);
            PerformKillButton.LastCleaned = DateTime.UtcNow;
            var renderer = body.GetComponent<SpriteRenderer>();
            var backColour = renderer.material.GetColor(BackColor);
            var bodyColour = renderer.material.GetColor(BodyColor);
            var newColour = new Color(1f, 1f, 1f, 0f);
            for (var i = 0; i < 60; i++)
            {
                if (body == null) yield break;
                renderer.color = Color.Lerp(backColour, newColour, i / 60f);
                renderer.color = Color.Lerp(bodyColour, newColour, i / 60f);
                yield return null;

            }
            UnityEngine.Object.Destroy(body.gameObject);
            

        }
    }
}