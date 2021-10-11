using UnityEngine;
using Hazel;
using TownOfUs.ImpostorRoles.JanitorMod;
using Reactor;

namespace TownOfUs.Roles
{
    public class Janitor : Impostor
    {
        public Janitor(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Clean up bodies";
            TaskText = () => "Clean bodies to prevent Crewmates from discovering them.";
            RoleType = RoleEnum.Janitor;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new BodyAbilityData
                {
                    Callback = CleanCallback,
                    MaxTimer = CustomGameOptions.DragCd,
                    Range = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance],
                    TargetColor = Color.yellow,
                    Icon = TownOfUs.JanitorClean,
                    Position = AbilityPositions.OverKillButton,
                    SyncWithKill = true
                });
            }
        }

        public void CleanCallback(DeadBody target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.JanitorClean, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(target.ParentId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Coroutines.Start(JanitorCoroutines.CleanCoroutine(target, this));
        }
    }
}
