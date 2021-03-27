using UnityEngine;

namespace TownOfUs.Roles
{
    public class Flash : Modifier
    {
        public Flash(PlayerControl player) : base(player)
        {
            Name = "Flash";
            TaskText = () => "Superspeed!";
            Color = new Color(1f, 0.5f, 0.5f, 1f);
            ModifierType = ModifierEnum.Flash;
        }
    }
}