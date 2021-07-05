using UnityEngine;
using Hazel;

namespace TownOfUs.Roles
{
    public class Medic : Role
    {
        public bool UsedAbility = false;
        public PlayerControl ShieldedPlayer = null;
        public PlayerAbilityData ShieldButton;

        public Medic(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Create a shield to protect a crewmate";
            TaskText = () => "Protect a crewmate with a shield";
            RoleType = RoleEnum.Medic;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(ShieldButton = new PlayerAbilityData
                {
                    Callback = ShieldCallback,
                    MaxTimer = 10f,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    Icon = TownOfUs.MedicSprite,
                    Position = AbilityPositions.KillButton
                });
            }
        }

        public void ShieldCallback(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Protect, SendOption.Reliable, -1);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            ShieldedPlayer = target;
            UsedAbility = true;

            ShieldButton.IsHighlighted = () => false;
            NamePatch.UpdateDisplay(target);
        }
    }
}
