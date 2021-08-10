using Hazel;
using System.Collections.Generic;
using TownOfUs.CrewmateRoles.SeerMod;
using UnityEngine;
using TMPro;

namespace TownOfUs.Roles
{
    public class Seer : Role
    {
        public List<byte> Investigated = new List<byte>();

        public Seer(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Investigate roles";
            TaskText = () => "Investigate roles and find the Impostor";
            RoleType = RoleEnum.Seer;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlayerAbilityData
                {
                    Callback = RevealCallback,
                    MaxTimer = CustomGameOptions.SeerCd,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    TargetFilter = player => !Investigated.Contains(player.PlayerId),
                    Icon = TownOfUs.SeerSprite,
                    Position = AbilityPositions.KillButton
                });
            }
        }

        public override bool Criteria()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            return (Player.AmOwner && !Player.Data.Disconnected) || (
                Investigated.Contains(localPlayer.PlayerId) &&
                CheckSeeReveal(localPlayer)
            ) || base.Criteria();
        }

        public void RevealCallback(PlayerControl target)
        {
            var targetId = target.PlayerId;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Investigate, SendOption.Reliable, -1);
            writer.Write(targetId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Investigated.Add(targetId);
            NamePatch.UpdateSingle(target, false);
        }

        public bool CheckSeeReveal(PlayerControl player)
        {
            var role = GetRole(player);
            return CustomGameOptions.SeeReveal switch
            {
                SeeReveal.All => true,
                SeeReveal.Nobody => false,
                SeeReveal.ImpsAndNeut => role?.Faction == Faction.Neutral || player.Data.IsImpostor,
                SeeReveal.Crew => role?.Faction == Faction.Crewmates,
                _ => false,
            };
        }
    }
}
