using UnityEngine;

namespace TownOfUs.Roles
{
    public class Drunk : Modifier
    {
        public Drunk(PlayerControl player) : base(player)
        {
            Name = "Drunk";
            TaskText = () => "Inverrrrrted contrrrrols";
            Color = new Color(0.46f, 0.5f, 0f, 1f);
            ModifierType = ModifierEnum.Drunk;
        }
    }
}