using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InnerNet;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.CrewmateRoles.AltruistMod
{
    public class Coroutine
    {
        public static ArrowBehaviour Arrow;
        public static PlayerControl Target;
        public static Sprite Sprite => TownOfUs.Arrow;

        public static IEnumerator AltruistRevive(DeadBody target, Altruist role)
        {
            var parentId = target.ParentId;
            var position = target.TruePosition;

            var revived = new List<PlayerControl>();


            if (AmongUsClient.Instance.AmHost) Utils.RpcMurderPlayer(role.Player, role.Player);

            if (CustomGameOptions.AltruistTargetBody)
                if (target != null)
                    Object.Destroy(target.gameObject);

            var startTime = DateTime.UtcNow;
            while (true)
            {
                var now = DateTime.UtcNow;
                var seconds = (now - startTime).TotalSeconds;
                if (seconds < CustomGameOptions.ReviveDuration)
                    yield return null;
                else break;

                if (MeetingHud.Instance) yield break;
            }

            var altruistBody = Object.FindObjectsOfType<DeadBody>()
                .FirstOrDefault(b => b.ParentId == role.Player.PlayerId);
            if (altruistBody != null) Object.Destroy(altruistBody.gameObject);

            var player = Utils.PlayerById(parentId);

            // if (player == null || AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started)
            //     yield break;

            player.Revive();
            Murder.KilledPlayers.Remove(
                Murder.KilledPlayers.FirstOrDefault(x => x.PlayerId == player.PlayerId));
            revived.Add(player);
            player.NetTransform.SnapTo(position);

            if (target != null) Object.Destroy(target.gameObject);

            if (player.isLover() && CustomGameOptions.BothLoversDie)
            {
                var lover = Role.GetRole<Lover>(player).OtherLover.Player;

                lover.Revive();
                Murder.KilledPlayers.Remove(
                    Murder.KilledPlayers.FirstOrDefault(x => x.PlayerId == lover.PlayerId));
                revived.Add(lover);

                var loverBody = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == lover.PlayerId);

                if (loverBody != null)
                {
                    lover.NetTransform.SnapTo(loverBody.TruePosition);
                    Object.Destroy(loverBody.gameObject);
                }
            }

            if (revived.Any(x => x.AmOwner))
                try
                {
                    Minigame.Instance.Close();
                    Minigame.Instance.Close();
                }
                catch
                {
                }


            if (PlayerControl.LocalPlayer.Data.IsImpostor || PlayerControl.LocalPlayer.Is(RoleEnum.Glitch))
            {
                var gameObj = new GameObject();
                Arrow = gameObj.AddComponent<ArrowBehaviour>();
                gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                var renderer = gameObj.AddComponent<SpriteRenderer>();
                renderer.sprite = Sprite;
                Arrow.image = renderer;
                gameObj.layer = 5;
                Target = player;
                yield return Utils.FlashCoroutine(role.Color, 1f, 0.5f);
            }
        }
    }
}