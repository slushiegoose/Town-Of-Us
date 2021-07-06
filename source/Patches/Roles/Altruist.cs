using UnityEngine;
using Hazel;
using TownOfUs.CrewmateRoles.AltruistMod;

namespace TownOfUs.Roles
{
    public class Altruist : Role
    {
        public bool CurrentlyReviving;
        public DeadBody CurrentTarget;

        public bool ReviveUsed;

        public Altruist(PlayerControl player) : base(player)
        {
            Name = "Altruist";
            ImpostorText = () => "Sacrifice yourself to save another";
            TaskText = () => "Revive a dead body at the cost of your own life.";
            Color = new Color(0.4f, 0f, 0f, 1f);
            RoleType = RoleEnum.Altruist;

            if (player.AmOwner)
            {
                var killButton = HudManager.Instance.KillButton;
                AbilityManager.Add(new BodyAbilityData
                {
                    Callback = ReviveCallback,
                    MaxTimer = 10f,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color.yellow,
                    Icon = TownOfUs.ReviveSprite,
                    Position = TOUConstants.KillButtonPosition
                });
            }
        }

        public void ReviveCallback(DeadBody target)
        {
            var playerId = target.ParentId;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.AltruistRevive, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Coroutines.Start(AltruistCoroutine.AltruistRevive(target, this));
        }
    }
}
