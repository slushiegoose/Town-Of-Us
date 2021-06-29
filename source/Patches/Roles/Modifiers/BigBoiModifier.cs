using TownOfUs.Extensions;
using TownOfUs.Patches.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class BigBoiModifier : Modifier, IVisualAlteration
    {
        public BigBoiModifier(PlayerControl player) : base(player)
        {
            Name = "Giant";
            TaskText = () => "Super slow!";
            Color = new Color(1f, 0.5f, 0.5f, 1f);
            ModifierType = ModifierEnum.BigBoi;
        }

        public bool TryGetModifiedAppearance(out VisualAppearance appearance)
        {
            appearance = Player.GetDefaultAppearance();
            appearance.SpeedFactor = 0.5f;
            appearance.SizeFactor = new Vector3(1.0f, 1.0f, 1.0f);
            return true;
        }
    }
}