using System.Collections;
using System.Linq;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Roles;
using UnityEngine;
using Reactor.Extensions;

namespace TownOfUs.CrewmateRoles.AltruistMod
{
    public class AltruistCoroutine
    {
        public static ArrowBehaviour Arrow;
        public static PlayerControl Target;

        private static void Revive(PlayerControl player)
        {
            player.Revive();
            var killedPlayer = Murder.KilledPlayers.Find(x => x.PlayerId == player.PlayerId);
            player.NetTransform.SnapTo(killedPlayer.DeathPosition);
            Murder.KilledPlayers.Remove(killedPlayer);

            if (player.AmOwner)
            {
                Minigame.Instance?.Close();
                Minigame.Instance?.Close();
            }
        }

        public static IEnumerator AltruistRevive(DeadBody targetBody, Altruist role)
        {
            var targetId = targetBody.ParentId;
            var altruist = role.Player;
            var altruistId = altruist.PlayerId;

            Utils.MurderPlayer(altruist, altruist);

            var deathPosition = targetBody.TruePosition;
            var destroyTarget = CustomGameOptions.AltruistTargetBody;

            if (destroyTarget)
                targetBody.gameObject.Destroy();

            yield return new WaitForSeconds(CustomGameOptions.ReviveDuration);

            if (MeetingHud.Instance != null)
                yield break;

            var bodies = Object.FindObjectsOfType<DeadBody>();

            var altruistBody = bodies.FirstOrDefault(x => x.ParentId == altruistId);
            altruistBody?.gameObject.Destroy();

            if (!destroyTarget)
                targetBody.gameObject.Destroy();

            var targetPlayer = Utils.PlayerById(targetId);
            if (targetPlayer == null || targetPlayer.Data.Disconnected)
                yield break;

            Revive(targetPlayer);

            if (CustomGameOptions.BothLoversDie && targetPlayer.IsLover())
            {
                var lover = Role.GetRole<Lover>(targetPlayer).OtherLover.Player;
                var body = bodies.FirstOrDefault(x => x.ParentId == lover.PlayerId);
                body?.gameObject.Destroy();

                Revive(lover);
            }

            var localPlayer = PlayerControl.LocalPlayer;
            
            if (localPlayer.Data.IsImpostor || localPlayer.Is(RoleEnum.Glitch))
            {
                var gameObj = new GameObject();
                Arrow = gameObj.AddComponent<ArrowBehaviour>();
                gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                var renderer = gameObj.AddComponent<SpriteRenderer>();
                renderer.sprite = TownOfUs.Arrow;
                Arrow.image = renderer;
                gameObj.layer = 5;
                Target = targetPlayer;
                yield return Utils.FlashCoroutine(role.Color, 1f, 0.5f);
            }
        }
    }
}
