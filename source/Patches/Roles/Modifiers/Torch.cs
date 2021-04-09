using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Torch : Modifier 
    {
        public Torch(PlayerControl player) : base(player)
        {
            Name = "Torch";
            TaskText = () => "You can see in the dark.";
            Color = new Color(1f, 1f, 0.6f);
            ModifierType = ModifierEnum.Torch;
        }
    }
}