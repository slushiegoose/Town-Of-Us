namespace TownOfUs.Roles
{
    public class Tiebreaker : Modifier
    {
        public Tiebreaker(PlayerControl player) : base(player)
        {
            Name = "Tiebreaker";
            TaskText = () => "Your vote breaks ties";
            Color = new UnityEngine.Color(0.6f, 0.9f, 0.6f);
            ModifierType = ModifierEnum.Tiebreaker;
        }
    }
}