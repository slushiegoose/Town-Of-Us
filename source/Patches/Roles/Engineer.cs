using UnityEngine;
using TownOfUs.CrewmateRoles.EngineerMod;
using Hazel;

namespace TownOfUs.Roles
{
    public class Engineer : Role
    {
        public bool UsedThisGame = false;
        public bool UsedThisRound = false;

        public Engineer(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Maintain important systems on the ship";
            TaskText = () => "Vent and fix a sabotage from anywhere!";
            RoleType = RoleEnum.Engineer;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlainAbilityData
                {
                    Callback = FixCallback,
                    IsHighlighted = CanUseFix,
                    MaxTimer = 10f,
                    Icon = TownOfUs.EngineerFix,
                    Position = AbilityPositions.KillButton,
                });
            }
        }

        public bool CanUseFix() {
            var used = CustomGameOptions.EngineerFixPer == EngineerFixPer.Game
                ? UsedThisGame
                : UsedThisRound;
            if (used) return false;

            var system = ShipStatus.Instance
                .Systems[SystemTypes.Sabotage]
                .Cast<SabotageSystemType>();
            return !system.dummy.IsActive && system.AnyActive;
        }

        public void FixCallback()
        {
            var mapId = (MapTypes)PlayerControl.GameOptions.MapId;

            var shipStatus = ShipStatus.Instance;
            var player = Player;
            
            // What map is this specific to?
            // No map has a sabotage in lab?
            shipStatus.RepairSystem(SystemTypes.Laboratory, player, 16);
            shipStatus.RepairSystem(SystemTypes.Reactor, player, 16);
            shipStatus.RepairSystem(SystemTypes.LifeSupp, player, 16);

            if (mapId == MapTypes.MiraHQ)
            {
                shipStatus.RepairSystem(SystemTypes.Comms, player, 16 | 0);
                shipStatus.RepairSystem(SystemTypes.Comms, player, 16 | 1);
            }
            else
                shipStatus.RepairSystem(SystemTypes.Comms, player, 16);

            if (mapId == MapTypes.Airship)
                shipStatus.Systems[SystemTypes.Reactor].Cast<HeliSabotageSystem>().ClearSabotage();

            var lights = shipStatus.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();

            lights.ActualSwitches = lights.ExpectedSwitches;

            if (player.AmOwner)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.EngineerFix, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            UsedThisGame = UsedThisRound = true;
        }
    }
}
