using System;
using UnityEngine;
using Object = System.Object;

namespace TownOfUs.Roles
{
    public class Swooper : Role
    {
        public KillButtonManager _swoopButton;
        public DateTime LastSwooped;
        public bool IsSwooped => TimeRemaining > 0f;
        public float TimeRemaining = 0f;
        public bool Enabled;
        public KillButtonManager SwoopButton
        {
            get { return _swoopButton;}
            set
            {
                _swoopButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }

        public Swooper(PlayerControl player) : base(player)
        {
            Name = "Swooper";
            ImpostorText = () => "Turn invisible temporarily";
            TaskText = () => "Turn invisible and sneakily kill";
            Color = Palette.ImpostorRed;
            RoleType = RoleEnum.Swooper;
            Faction = Faction.Impostors;

        }

        public float SwoopTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastSwooped;;
            var num = CustomGameOptions.SwoopCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }

        public void Swoop()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            var color = Color.clear;
            if (PlayerControl.LocalPlayer.Data.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead)
            {
                color.a = 0.1f;
            }


            Player.GetComponent<SpriteRenderer>().color = color;
            
            Player.HatRenderer.SetHat(0, 0);
            Player.nameText.Text = "";
            if (Player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[0].ProdId)
            {
                Player.MyPhysics.SetSkin(0);
            }
            if (Player.CurrentPet != null)
            {
                UnityEngine.Object.Destroy(Player.CurrentPet.gameObject);
            }
            Player.CurrentPet =
                UnityEngine.Object.Instantiate(
                    DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[0]);
            Player.CurrentPet.transform.position = Player.transform.position;
            Player.CurrentPet.Source = Player;
            Player.CurrentPet.Visible = Player.Visible;
        }
        

        public void UnSwoop()
        {
            Enabled = false;
            LastSwooped = DateTime.UtcNow;
            Utils.Unmorph(Player);
            Player.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}