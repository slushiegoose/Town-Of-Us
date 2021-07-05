using Hazel;
using UnityEngine;
using Reactor.Extensions;

namespace TownOfUs.Roles
{
    public class Swooper : Impostor
    {
        public bool Enabled;

        public Swooper(PlayerControl player) : base(player)
        {
            ImpostorText = () => "Turn invisible temporarily";
            TaskText = () => "Turn invisible and sneakily kill";
            RoleType = RoleEnum.Swooper;
            CreateButtons();
        }

        public override void CreateButtons()
        {
            if (Player.AmOwner)
            {
                AbilityManager.Add(new PlainAbilityData
                {
                    Callback = SwoopCallback,
                    MaxTimer = CustomGameOptions.SwoopCd,
                    Icon = TownOfUs.SwoopSprite,
                    Position = AbilityPositions.OverKillButton,
                    MaxDuration = CustomGameOptions.SwoopDuration,
                    OnDurationEnd = UnSwoop
                });
            }
        }

        public void SwoopCallback()
        {
            if (Player.AmOwner)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.Swoop, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            Enabled = true;
            var color = Color.clear;
            var localData = PlayerControl.LocalPlayer.Data;
            if (localData.IsImpostor || localData.IsDead) color.a = 0.1f;

            var hatManager = HatManager.Instance;

            Player.MyRend.color = color;

            Player.HatRenderer.SetHat(0, 0);
            Player.nameText.text = "";
            if (Player.MyPhysics.Skin.skin.ProdId != hatManager.AllSkins.ToArray()[0].ProdId)
                Player.MyPhysics.SetSkin(0);

            Player.CurrentPet?.gameObject.Destroy();

            var pet = Player.CurrentPet = Object.Instantiate(HatManager.Instance.AllPets.ToArray()[0]);
            pet.transform.position = Player.transform.position;
            pet.Source = Player;
            pet.Visible = Player.Visible;
        }

        public void UnSwoop()
        {
            if (Player.AmOwner)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.UnSwoop, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            Enabled = false;
            Utils.Unmorph(Player);
            Player.MyRend.color = Color.white;
        }
    }
}
