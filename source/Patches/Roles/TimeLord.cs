using System;
using TownOfUs.CrewmateRoles.TimeLordMod;
using UnityEngine;
using Hazel;

namespace TownOfUs.Roles
{
    public class TimeLord : Role
    {
        public TimeLord(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Rewind Time";
            TaskText = () => "Rewind Time!";
            RoleType = RoleEnum.TimeLord;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlainAbilityData
                {
                    Callback = RewindCallback,
                    MaxTimer = CustomGameOptions.RewindCooldown,
                    Icon = TownOfUs.Rewind,
                    Position = AbilityPositions.KillButton,
                    IsHighlighted = () => true
                });
            }
        }

        public void RewindCallback()
        {
            if (Player.AmOwner)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.Rewind, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            StartStop.StartRewind();
        }
    }
}
