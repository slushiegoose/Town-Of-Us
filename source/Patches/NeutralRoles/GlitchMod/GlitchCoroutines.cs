using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownOfUs.Roles;
using Reactor.Extensions;

namespace TownOfUs.NeutralRoles.GlitchMod
{
    public static class GlitchCoroutines
    {
        public static bool IsHacked = false;

        public static IEnumerator Hack(Glitch role, PlayerControl target)
        {
            var amOwner = target.AmOwner;
            var lockImages = new List<GameObject>();

            var hackText = new GameObject("_Player").AddComponent<ImportantTextTask>();
            hackText.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
            
            hackText.Index = target.PlayerId;
            PlayerControl.LocalPlayer.myTasks.Insert(0, hackText);

            var duration = CustomGameOptions.HackDuration;

            void SetText()
            {
                hackText.Text = Utils.ColorText(
                    role.Color,
                    $"Hacked {(amOwner ? "For" : target.Data.PlayerName)} {duration} seconds.\n"
                );
            }

            SetText();

            var hudManager = HudManager.Instance;
            var hudKill = hudManager.KillButton;
            var killWasEnabled = false;
            var useButton = hudManager.UseButton;
            var reportButton = hudManager.ReportButton;

            if (amOwner)
            {
                IsHacked = true;
                try
                {
                    Minigame.Instance?.Close();
                    Minigame.Instance?.Close();
                    MapBehaviour.Instance?.Close();
                    MapBehaviour.Instance?.Close();
                }
                catch { }
                void AddLockIcon(Transform parent)
                {
                    var lockImage = new GameObject($"Lock{lockImages.Count}");
                    lockImage.AddComponent<SpriteRenderer>().sprite = Glitch.LockSprite;
                    lockImage.layer = 5;
                    lockImage.transform.SetParent(parent);
                    lockImage.transform.localPosition = new Vector3(0, 0, 0);
                    lockImages.Add(lockImage);
                }
                void LockAbility(KillButtonManager button)
                {
                    AddLockIcon(button.transform);
                    button.enabled = false;
                    button.SetTarget(null);
                }

                if (hudKill.isActiveAndEnabled)
                {
                    LockAbility(hudKill);
                    hudKill.SetTarget(null);
                    killWasEnabled = true;
                }

                foreach (var button in AbilityManager.Buttons)
                    LockAbility(button.KillButton);

                AddLockIcon(useButton.transform);
                useButton.enabled = false;
                useButton.SetTarget(null);

                AddLockIcon(reportButton.transform);
                reportButton.enabled = false;
            }

            while (duration > 0)
            {
                if (target.Data.IsDead || MeetingHud.Instance != null) break;
                SetText();
                yield return new WaitForSeconds(1f);
                duration--;
            }

            if (amOwner)
            {
                foreach (var lockImage in lockImages)
                    lockImage.gameObject.Destroy();

                foreach (var button in AbilityManager.Buttons)
                    button.KillButton.enabled = true;

                useButton.enabled = true;
                reportButton.enabled = true;
                if (killWasEnabled)
                    hudKill.enabled = true;
                IsHacked = false;
            }

            PlayerControl.LocalPlayer.myTasks.Remove(hackText);
        }
    }
}
