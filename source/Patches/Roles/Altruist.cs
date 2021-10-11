using UnityEngine;
using Hazel;
using TownOfUs.CrewmateRoles.AltruistMod;
using Reactor;

namespace TownOfUs.Roles
{
    public class Altruist : Role
    {
        public bool CurrentlyReviving;
        public DeadBody CurrentTarget;

        public bool ReviveUsed;

        public Altruist(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Sacrifice yourself to save another";
            TaskText = () => "Revive a dead body at the cost of your own life.";
            RoleType = RoleEnum.Altruist;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                var killButton = HudManager.Instance.KillButton;
                AbilityManager.Add(new BodyAbilityData
                {
                    Callback = ReviveCallback,
                    MaxTimer = 10f,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color.yellow,
                    Icon = TownOfUs.ReviveSprite,
                    Position = AbilityPositions.KillButton
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
