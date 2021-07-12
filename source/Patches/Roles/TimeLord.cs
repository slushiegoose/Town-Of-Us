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
            Name = "Time Lord";
            ImpostorText = () => "Rewind Time";
            TaskText = () => "Rewind Time!";
            Color = new Color(0f, 0f, 1f, 1f);
            RoleType = RoleEnum.TimeLord;
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
                    Position = TOUConstants.KillButtonPosition,
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
