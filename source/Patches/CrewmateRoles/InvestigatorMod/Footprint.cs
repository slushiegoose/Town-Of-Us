using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.RainbowMod;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.InvestigatorMod
{
    public class Footprint
    {
        public readonly PlayerControl Player;
        private GameObject _gameObject;
        private SpriteRenderer _spriteRenderer;
        private readonly float _time;
        private readonly Vector2 _velocity;

        public Color Color;
        public Vector3 Position;
        public Investigator Role;

        public Footprint(PlayerControl player, Investigator role)
        {
            Role = role;
            Position = player.transform.position;
            _velocity = player.gameObject.GetComponent<Rigidbody2D>().velocity;

            Player = player;
            _time = (int) Time.time;
            Color = Color.black;

            Start();
            role.AllPrints.Add(this);
        }

        public static float Duration => CustomGameOptions.FootprintDuration;

        public static bool Grey =>
            CustomGameOptions.AnonymousFootPrint || CamouflageUnCamouflage.IsCamoed;

        public static void DestroyAll(Investigator role)
        {
            while (role.AllPrints.Count != 0) role.AllPrints[0].Destroy();
        }


        private void Start()
        {
            _gameObject = new GameObject("Footprint");
            _gameObject.transform.position = Position;
            _gameObject.transform.Rotate(Vector3.forward * Vector2.SignedAngle(Vector2.up, _velocity));
            _gameObject.transform.SetParent(Player.transform.parent);

            _spriteRenderer = _gameObject.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite = TownOfUs.Footprint;
            _spriteRenderer.color = Color;
            _gameObject.transform.localScale *= new Vector2(1.2f, 1f) * (CustomGameOptions.FootprintSize / 10);


            _gameObject.SetActive(true);
        }

        private void Destroy()
        {
            Object.Destroy(_gameObject);
            Role.AllPrints.Remove(this);
        }

        public bool Update()
        {
            var currentTime = Time.time;
            var alpha = Mathf.Max(1f - (currentTime - _time) / Duration, 0f);

            if (alpha < 0 || alpha > 1)
                alpha = 0;

            
            if (Grey)
                Color = new Color(0.2f, 0.2f, 0.2f, 1f);
            else if (RainbowUtils.IsRainbow(Player.Data.ColorId))
                Color = RainbowUtils.Rainbow;
            else
                Color = Palette.PlayerColors[Player.Data.ColorId];

            Color = new Color(Color.r, Color.g, Color.b, alpha);
            _spriteRenderer.color = Color;

            if (_time + (int) Duration < currentTime)
            {
                Destroy();
                return true;
            }

            return false;
        }
    }
}
