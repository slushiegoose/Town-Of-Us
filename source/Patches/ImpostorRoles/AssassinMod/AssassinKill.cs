using System.Linq;
using Hazel;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;
using UnityEngine.UI;
using TownOfUs.CrewmateRoles.MedicMod;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    public class AssassinKill
    {
        public static void RpcMurderPlayer(PlayerControl player)
        {
            PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(
                x => x.TargetPlayerId == player.PlayerId
            );
            RpcMurderPlayer(voteArea, player);
        }
        public static void RpcMurderPlayer(PlayerVoteArea voteArea, PlayerControl player)
        {
            MurderPlayer(voteArea, player);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.AssassinKill, SendOption.Reliable, -1);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void MurderPlayer(PlayerControl player, bool checkLover = true)
        {
            PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(
                x => x.TargetPlayerId == player.PlayerId
            );
            MurderPlayer(voteArea, player, checkLover);
        }
        public static void MurderPlayer(
            PlayerVoteArea voteArea,
            PlayerControl player,
            bool checkLover = true
        )
        {
            var hudManager = DestroyableSingleton<HudManager>.Instance;
            if (checkLover)
            {
                SoundManager.Instance.PlaySound(player.KillSfx, false, 0.8f);
                hudManager.KillOverlay.ShowKillAnimation(player.Data, player.Data);
            }
            var amOwner = player.AmOwner;
            if (amOwner)
            {
                hudManager.ShadowQuad.gameObject.SetActive(false);
                player.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                player.RpcSetScanner(false);
                ImportantTextTask importantTextTask = new GameObject("_Player").AddComponent<ImportantTextTask>();
                importantTextTask.transform.SetParent(AmongUsClient.Instance.transform, false);
                if (!PlayerControl.GameOptions.GhostsDoTasks)
                {
                    for (int i = 0;i < player.myTasks.Count;i++)
                    {
                        PlayerTask playerTask = player.myTasks.ToArray()[i];
                        playerTask.OnRemove();
                        Object.Destroy(playerTask.gameObject);
                    }

                    player.myTasks.Clear();
                    importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                        StringNames.GhostIgnoreTasks,
                        new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(0)
                    );
                }
                else
                {
                    importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                        StringNames.GhostDoTasks,
                        new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(0));
                }

                player.myTasks.Insert(0, importantTextTask);

                if (player.Is(RoleEnum.Swapper))
                {
                    var buttons = Role.GetRole<Swapper>(player).Buttons;
                    foreach (var button in buttons)
                    {
                        button.SetActive(false);
                        button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }
            }
            player.Die(DeathReason.Kill);
            if (checkLover && player.isLover() && CustomGameOptions.BothLoversDie)
                MurderPlayer(Role.GetRole<Lover>(player).OtherLover.Player, false);

            var meetingHud = MeetingHud.Instance;
            if (amOwner)
            {
                meetingHud.SetForegroundForDead();
            }
            var deadPlayer = new DeadPlayer
            {
                PlayerId = player.PlayerId,
                KillerId = player.PlayerId,
                KillTime = System.DateTime.UtcNow,
            };

            Murder.KilledPlayers.Add(deadPlayer);
            if (voteArea == null) return;
            if (voteArea.DidVote) voteArea.UnsetVote();
            voteArea.AmDead = true;
            voteArea.Overlay.gameObject.SetActive(true);
            voteArea.Overlay.color = Color.white;
            voteArea.XMark.gameObject.SetActive(true);
            voteArea.XMark.transform.localScale = Vector3.one;

            voteArea.Buttons.SetActive(false);
            var amHost = AmongUsClient.Instance.AmHost;
            foreach (var playerVoteArea in meetingHud.playerStates)
            {
                if (playerVoteArea.VotedFor != player.PlayerId) continue;
                playerVoteArea.UnsetVote();
                var voteAreaPlayer = Utils.PlayerById(playerVoteArea.TargetPlayerId);
                if (!voteAreaPlayer.AmOwner) continue;
                meetingHud.ClearVote();
            }

            if (!amHost) return;
            foreach (var role in Role.GetRoles(RoleEnum.Mayor))
            {
                var mayor = (Mayor)role;
                if (role.Player.PlayerId == player.PlayerId)
                {
                    mayor.ExtraVotes.Clear();
                }
                else
                {
                    var votesRegained = mayor.ExtraVotes.RemoveAll(x => x == player.PlayerId);

                    if (mayor.Player == PlayerControl.LocalPlayer)
                        mayor.VoteBank += votesRegained;

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.AddMayorVoteBank, SendOption.Reliable, -1);
                    writer.Write(mayor.Player.PlayerId);
                    writer.Write(votesRegained);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            }
            meetingHud.CheckForEndVoting();
        }
    }
}
