using System;
using TownOfUs.Extensions;
using TownOfUs.Roles.Modifiers;
using UnityEngine;
using Hazel;

namespace TownOfUs.Roles
{
    public class Morphling : Role, IVisualAlteration
    {
        public PlayerControl MorphedPlayer;
        public PlayerControl SampledPlayer;

        public PlayerAbilityData MorphButton;

        public Morphling(PlayerControl player) : base(player)
        {
            Name = "Morphling";
            ImpostorText = () => "Transform into crewmates";
            TaskText = () => "Morph into crewmates to be disguised";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Morphling;
            Faction = Faction.Impostors;

            if (player.AmOwner)
            {
                AbilityManager.Add(MorphButton = new PlayerAbilityData
                {
                    Callback = SampleMorphCallback,
                    MaxTimer = CustomGameOptions.MorphlingCd,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color,
                    Icon = TownOfUs.SampleSprite,
                    Position = TOUConstants.OverKillbutton,
                    OnDurationEnd = UnMorph
                });
            }
        }

        public void UnMorph()
        {
            Utils.Unmorph(Player);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.ResetAnim, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public void Morph()
        {
            Utils.Morph(Player, SampledPlayer);
        }


        public void SampleMorphCallback(PlayerControl target)
        {
            if (SampledPlayer == null)
            {
                SampledPlayer = target;
                MorphButton.Icon = TownOfUs.MorphSprite;
                MorphButton.Timer = 1f;
                MorphButton.MaxDuration = CustomGameOptions.MorphlingDuration;
            }
            else
            {
                Morph();
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.Morph, SendOption.Reliable, -1);
                writer.Write(SampledPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        public bool TryGetModifiedAppearance(out VisualAppearance appearance)
        {
            if (MorphedPlayer != null)
            {
                appearance = MorphedPlayer.GetDefaultAppearance();
                var modifier = Modifier.GetModifier(MorphedPlayer);
                if (modifier is IVisualAlteration alteration)
                    alteration.TryGetModifiedAppearance(out appearance);
                return true;
            }

            appearance = Player.GetDefaultAppearance();
            return false;
        }
    }
}
