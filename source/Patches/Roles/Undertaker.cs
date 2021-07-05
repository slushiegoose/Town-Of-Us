using Hazel;
using UnityEngine;
using TownOfUs.Extensions;

namespace TownOfUs.Roles
{
    public class Undertaker : Impostor, IVisualAlteration
    {
        public BodyAbilityData DragDropButton;
        public DeadBody CurrentlyDragging { get; set; }

        public Undertaker(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Drag bodies and hide them";
            TaskText = () => "Drag bodies around to hide them from being reported";
            RoleType = RoleEnum.Undertaker;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(DragDropButton = new BodyAbilityData
                {
                    Callback = DragDropCallback,
                    MaxTimer = PlayerControl.GameOptions.KillCooldown,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color.yellow,
                    Icon = TownOfUs.DragSprite,
                    Position = AbilityPositions.OverKillButton,
                });
            }
        }

        public bool TryGetModifiedAppearance(out VisualAppearance appearance)
        {
            appearance = Player.GetDefaultAppearance();
            var isDragging = CurrentlyDragging != null;
            if (isDragging)
            {
                appearance.SpeedFactor = 0.5f;
            }
            return isDragging;
        }

        public void DragDropCallback(DeadBody target)
        {
            target ??= CurrentlyDragging;
            var playerId = target.ParentId;
            var material = target.bodyRenderer.material;

            var rpc = CurrentlyDragging == null ? CustomRPC.Drag : CustomRPC.Drop;
            if (rpc == CustomRPC.Drag)
            {
                CurrentlyDragging = target;
                DragDropButton.Icon = TownOfUs.DropSprite;
                DragDropButton.IsHighlighted = () => true;
                DragDropButton.Timer = 1f;

                material.SetFloat("_Outline", 1f);
                material.SetColor("_OutlineColor", Color.green);
            }
            else
            {
                CurrentlyDragging = null;
                DragDropButton.Icon = TownOfUs.DragSprite;
                DragDropButton.IsHighlighted = null;
                DragDropButton.Timer = DragDropButton.MaxTimer;

                material.SetFloat("_Outline", 0f);
            }

            DragDropButton.UpdateSprite();

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)rpc, SendOption.Reliable, -1);
            if (rpc == CustomRPC.Drag) writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}
