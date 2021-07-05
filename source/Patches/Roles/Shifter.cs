using Hazel;
using UnityEngine;
using TownOfUs.NeutralRoles.ShifterMod;

namespace TownOfUs.Roles
{
    public class Shifter : Role
    {
        public Shifter(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Shift around different roles";
            TaskText = () => "Steal other people's roles.\nFake Tasks:";
            RoleType = RoleEnum.Shifter;
            Faction = Faction.Neutral;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = ShiftCallback,
                    MaxTimer = CustomGameOptions.ShifterCd,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    Icon = TownOfUs.Shift,
                    Position = AbilityPositions.KillButton
                });
            }
        }

        public void ShiftCallback(PlayerControl target)
        {
            if (target.IsShielded())
            {
                Utils.RpcBreakShield(target);
                return;
            }
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Shift, SendOption.Reliable, -1);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Shift.ShiftRoles(this, GetRole(target));
        }

        public void Loses()
        {
            Player.Data.IsImpostor = true;
        }
    }
}
