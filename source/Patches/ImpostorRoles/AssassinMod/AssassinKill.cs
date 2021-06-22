using System.Linq;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.ImpostorRoles.AssassinMod
{
    public class AssassinKill
    {
        public static void RpcMurderPlayer(PlayerControl toDie)
        {
            MurderPlayer(toDie);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.AssassinKill, SendOption.Reliable, -1);
            writer.Write(toDie.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        private static void RpcClearVote(int clientId)
        {
            if (AmongUsClient.Instance.ClientId == clientId)
            {
                MeetingHud.Instance.ClearVote();
                return;
            }

            var msg = AmongUsClient.Instance.StartRpcImmediately(MeetingHud.Instance.NetId, 25, SendOption.Reliable,
                clientId);
            AmongUsClient.Instance.FinishRpcImmediately(msg);
        }


        public static void MurderPlayer(PlayerControl toDie, bool loverDiedYet = false)
        {
            Utils.MurderPlayer(toDie, toDie);

            var body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == toDie.PlayerId);
            if (body != null) Object.Destroy(body.gameObject);

            if (!toDie.AmOwner)
            {
                DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(toDie.Data, toDie.Data);
            }
            else
            {
                MeetingHud.Instance.Glass.sprite = MeetingHud.Instance.CrackedGlass;
                MeetingHud.Instance.amDead = true;
                MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(false);
            }

            if (AmongUsClient.Instance.AmHost)
            {
                var num = MeetingHud.Instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == toDie.PlayerId);
                num.UnsetVote();
                foreach (var state2 in MeetingHud.Instance.playerStates)
                    if (!state2.AmDead && state2.DidVote && state2.VotedFor == (sbyte) toDie.PlayerId)
                    {
                        state2.UnsetVote();
                        var playerById = GameData.Instance.GetPlayerById(state2.TargetPlayerId);
                        if (playerById != null)
                        {
                            var clientId = AmongUsClient.Instance.GetClientIdFromCharacter(playerById.Object);
                            if (clientId != -1) RpcClearVote(clientId);
                        }
                    }
            }

            var state = MeetingHud.Instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == toDie.PlayerId);
            if (state != null)
            {
                state.AmDead = true;
                //System.Console.WriteLine(state.TargetPlayerId + " and I am the host: " + AmongUsClient.Instance.AmHost);
                state.Overlay.gameObject.SetActive(true);
                state.XMark.gameObject.SetActive(true);
                // state.Overlay.transform.GetChild(0).gameObject.SetActive(true);x

                //System.Console.WriteLine(state.Overlay.gameObject.active);
            }

            if (toDie.isLover() && !loverDiedYet && CustomGameOptions.BothLoversDie)
                MurderPlayer(Role.GetRole<Lover>(toDie).OtherLover.Player, true);
        }
    }
}
