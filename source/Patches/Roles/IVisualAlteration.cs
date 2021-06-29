namespace TownOfUs.Patches.Roles.Modifiers
{
    public interface IVisualAlteration
    {
        bool TryGetModifiedAppearance(out VisualAppearance appearance);
    }
}