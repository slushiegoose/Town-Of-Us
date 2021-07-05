using UnityEngine;
using Hazel;

namespace TownOfUs.Roles.Modifiers
{
    public class ButtonBarry : Modifier
    {
        public PlainAbilityData ButtonButton;

        public bool ButtonUsed;

        public ButtonBarry(PlayerControl player) : base(player)
        {
            Name = "Button Barry";
            TaskText = () => "Call a button from anywhere!";
            Color = new Color(0.9f, 0f, 1f, 1f);
            ModifierType = ModifierEnum.ButtonBarry;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(ButtonButton = new PlainAbilityData
                {
                    Callback = ButtonCallback,
                    IsHighlighted = () => !ButtonUsed,
                    MaxTimer = 10f,
                    Icon = TownOfUs.ButtonSprite,
                    Position = Player.Is(RoleEnum.Glitch)
                        ? AbilityPositions.BottomLeftC
                        : AbilityPositions.BottomLeftA
                });
            }
        }

        public void ButtonCallback()
        {
            ButtonUsed = true;
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.BarryButton, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            if (AmongUsClient.Instance.AmHost)
            {
                var meetingRoom = MeetingRoomManager.Instance;
                meetingRoom.reporter = PlayerControl.LocalPlayer;
                meetingRoom.target = null;
                AmongUsClient.Instance.DisconnectHandlers.AddUnique(
                    meetingRoom.Cast<IDisconnectHandler>()
                );
                HudManager.Instance.OpenMeetingRoom(PlayerControl.LocalPlayer);
                PlayerControl.LocalPlayer.RpcStartMeeting(null);
            }
        }
    }
}
