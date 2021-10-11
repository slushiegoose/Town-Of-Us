using UnityEngine;
using Hazel;

namespace TownOfUs.Roles
{
    public class Camouflager : Impostor

    {
        public bool Enabled;

        public Camouflager(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Camouflage and turn everyone grey";
            TaskText = () => "Camouflage and get secret kills";
            RoleType = RoleEnum.Camouflager;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                var killButton = HudManager.Instance.KillButton;
                AbilityManager.Add(new PlainAbilityData
                {
                    Callback = CamouCallback,
                    MaxTimer = CustomGameOptions.CamouflagerCd,
                    Icon = TownOfUs.Camouflage,
                    Position = AbilityPositions.OverKillButton,
                    MaxDuration = CustomGameOptions.CamouflagerDuration,
                    OnDurationEnd = CamouEnd
                });
            }
        }

        public void CamouCallback()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                   (byte)CustomRPC.Camouflage,
                   SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Enabled = true;
            Utils.Camouflage();
        }

        public void CamouEnd()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                   (byte)CustomRPC.UnCamouflage,
                   SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Enabled = false;
            Utils.UnCamouflage();
        }
    }
}
