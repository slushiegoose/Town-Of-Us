using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class ButtonBarry : Modifier
    {

        public bool ButtonUsed;
        public KillButtonManager ButtonButton;
        
        public ButtonBarry(PlayerControl player) : base(player)
        {
            Name = "Button Barry";
            TaskText = () => "Call a button from anywhere!";
            Color = new Color(0.9f, 0f, 1f, 1f);
            ModifierType = ModifierEnum.ButtonBarry;
        }
        
        
    }
}