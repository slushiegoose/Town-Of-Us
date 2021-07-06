using UnityEngine;

namespace TownOfUs.Roles
{
    public class Sheriff : Role
    {
        public Sheriff(PlayerControl player) : base(player)
        {
            Name = "Sheriff";
            ImpostorText = () => "Shoot the <color=#FF0000FF>Impostor</color>";
            TaskText = () => "Kill off the impostor but don't kill crewmates.";
            Color = Color.yellow;
            RoleType = RoleEnum.Sheriff;

            if (player.AmOwner)
            {
                var killButton = HudManager.Instance.KillButton;
                AbilityManager.Add(new AbilityData
                {
                    Callback = KillCallback,
                    KillButton = killButton,
                    MaxTimer = CustomGameOptions.SheriffKillCd,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    Position = TOUConstants.KillButtonPosition
                });
            }
        }

        public bool CanKill(PlayerControl player)
        {
            if (player.Data.IsImpostor) return true;

            var role = GetRole(player)?.RoleType;

            return
                role != null && (role == RoleEnum.Glitch ||
                (CustomGameOptions.SheriffKillsJester && role == RoleEnum.Jester) ||
                (CustomGameOptions.SheriffKillsArsonist && role == RoleEnum.Arsonist));
        }

        public void KillCallback(PlayerControl player)
        {
            var canKill = CanKill(player);

            if (player.isShielded())
            {
                Utils.RpcBreakShield(player);
                return;
            }

            if (canKill || CustomGameOptions.SheriffKillOther)
                Utils.RpcMurderPlayer(Player, player);

            if (!canKill)
                Utils.RpcMurderPlayer(Player, Player);
        }

        internal override bool Criteria()
        {
            return CustomGameOptions.ShowSheriff || base.Criteria();
        }
    }
}
