using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownOfUs.Roles;
using TownOfUs.Extensions;

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
                Minigame.Instance?.Close();
                Minigame.Instance?.Close();
                MapBehaviour.Instance?.Close();
                MapBehaviour.Instance?.Close();
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

        public static IEnumerator Mimic(Glitch __instance, PlayerControl mimicPlayer)
        {

            // TODO: add this
            yield return null;
            /*var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SetMimic, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(mimicPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Utils.Morph(__instance.Player, mimicPlayer, true);

            var mimicActivation = DateTime.UtcNow;
            var mimicText = new GameObject("_Player").AddComponent<ImportantTextTask>();
            mimicText.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
            mimicText.Text =
                $"{__instance.ColorString}Mimicking {mimicPlayer.Data.PlayerName} ({CustomGameOptions.MimicDuration}s)</color>\n";
            PlayerControl.LocalPlayer.myTasks.Insert(0, mimicText);

            while (true)
            {
                __instance.IsUsingMimic = true;
                __instance.MimicTarget = mimicPlayer;
                var totalMimickTime = (DateTime.UtcNow - mimicActivation).TotalMilliseconds / 1000;
                mimicText.Text =
                    $"{__instance.ColorString}Mimicking {mimicPlayer.Data.PlayerName} ({CustomGameOptions.MimicDuration - Math.Round(totalMimickTime)}s)</color>\n";
                if (totalMimickTime > CustomGameOptions.MimicDuration ||
                    PlayerControl.LocalPlayer.Data.IsDead ||
                    AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Ended)
                {
                    PlayerControl.LocalPlayer.myTasks.Remove(mimicText);
                    //System.Console.WriteLine("Unsetting mimic");
                    __instance.LastMimic = DateTime.UtcNow;
                    __instance.IsUsingMimic = false;
                    __instance.MimicTarget = null;
                    Utils.Unmorph(__instance.Player);

                    var writer2 = AmongUsClient.Instance.StartRpcImmediately(
                        PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RpcResetAnim, SendOption.Reliable,
                        -1);
                    writer2.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer2.Write(mimicPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                    yield break;
                }

                Utils.Morph(__instance.Player, mimicPlayer);
                __instance.MimicButton.SetCoolDown(CustomGameOptions.MimicDuration - (float)totalMimickTime,
                    CustomGameOptions.MimicDuration);

                yield return null;
            }*/
        }
    }
}
